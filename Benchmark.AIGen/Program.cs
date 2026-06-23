using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

return await App.RunAsync(args);

internal static class App
{
    public static async Task<int> RunAsync(string[] args)
    {
        try
        {
            if (args.Length == 0 || args[0] != "generate-api-endpoint")
            {
                PrintUsage();
                return 1;
            }

            var contractPath = GetOption(args, "--contract");
            var root = GetOption(args, "--root")
                       ?? RepositoryRootFinder.FindFrom(Directory.GetCurrentDirectory());
            var preview = HasFlag(args, "--preview");
            var apply = HasFlag(args, "--apply");

            if (string.IsNullOrWhiteSpace(contractPath))
            {
                throw new InvalidOperationException("Missing required option: --contract");
            }

            if (preview == apply)
            {
                throw new InvalidOperationException("Specify exactly one of --preview or --apply.");
            }

            root = Path.GetFullPath(root);
            contractPath = Path.GetFullPath(contractPath);

            var contract = await ContractLoader.LoadAsync(contractPath);

            new ContractValidator().Validate(contract);

            var symbolValidator = new SymbolValidator(root);
            symbolValidator.Validate(contract);

            var pipeline = new ApiGenerationPipeline();
            var artifacts = pipeline.Generate(contract);

            var codeGuard = new GeneratedCodeGuard();

            foreach (var artifact in artifacts)
            {
                codeGuard.Validate(artifact);
            }

            var outputRoot = Directory.GetCurrentDirectory();

            var writer = new ArtifactWriter(outputRoot);
            writer.Write(artifacts, preview);

            Console.WriteLine();
            Console.WriteLine("Generated artifacts:");

            foreach (var artifact in artifacts)
            {
                Console.WriteLine($"- {artifact.RelativePath}");
            }

            Console.WriteLine();

            if (preview)
            {
                Console.WriteLine("Preview written under:");
                Console.WriteLine(Path.Combine(outputRoot, "artifacts", "ai-preview"));
                Console.WriteLine();
                Console.WriteLine("Review the files, then run with --apply.");
            }
            else
            {
                Console.WriteLine("Files written to repository.");
                Console.WriteLine("Now run:");
                Console.WriteLine("dotnet build");
                Console.WriteLine("dotnet test");
                Console.WriteLine("dotnet format --verify-no-changes");
            }

            return 0;
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine("Generation failed:");
            Console.Error.WriteLine(ex.Message);
            return 2;
        }
    }

    private static string? GetOption(string[] args, string name)
    {
        var index = Array.IndexOf(args, name);

        if (index < 0 || index + 1 >= args.Length)
        {
            return null;
        }

        return args[index + 1];
    }

    private static bool HasFlag(string[] args, string name)
    {
        return args.Contains(name, StringComparer.OrdinalIgnoreCase);
    }

    private static void PrintUsage()
    {
        Console.WriteLine("Usage:");
        Console.WriteLine("dotnet run --project tools/Benchmark.AiGen -- generate-api-endpoint --contract <path> --preview");
        Console.WriteLine("dotnet run --project tools/Benchmark.AiGen -- generate-api-endpoint --contract <path> --apply");
    }
}

internal static class ContractLoader
{
    public static async Task<ApiEndpointGenerationContract> LoadAsync(string path)
    {
        if (!File.Exists(path))
        {
            throw new FileNotFoundException("Contract file was not found.", path);
        }

        var json = await File.ReadAllTextAsync(path);

        var options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };

        options.Converters.Add(new JsonStringEnumConverter());

        var contract = JsonSerializer.Deserialize<ApiEndpointGenerationContract>(json, options);

        if (contract is null)
        {
            throw new InvalidOperationException("Could not deserialize contract.");
        }

        return contract;
    }
}

internal sealed class ContractValidator
{
    private static readonly HashSet<string> AllowedHttpVerbs = new(StringComparer.OrdinalIgnoreCase)
    {
        "GET",
        "POST",
        "PUT",
        "PATCH",
        "DELETE"
    };

    private static readonly HashSet<string> AllowedBindingSources = new(StringComparer.OrdinalIgnoreCase)
    {
        "FromBody",
        "FromQuery"
    };

    public void Validate(ApiEndpointGenerationContract contract)
    {
        Require(contract.Service == "Benchmark.Api", "service must be Benchmark.Api.");
        Require(!string.IsNullOrWhiteSpace(contract.FeatureName), "featureName is required.");
        Require(!string.IsNullOrWhiteSpace(contract.Area), "area is required.");

        Require(contract.Controller.Name.EndsWith("Controller"), "controller.name must end with Controller.");
        Require(contract.Controller.Namespace == "Benchmark.Api.Controllers", "controller.namespace must be Benchmark.Api.Controllers.");
        Require(contract.Controller.RoutePrefix.StartsWith("api/", StringComparison.Ordinal), "controller.routePrefix must start with api/.");

        Require(AllowedHttpVerbs.Contains(contract.Endpoint.HttpVerb), "endpoint.httpVerb is invalid.");
        Require(contract.Endpoint.MethodName.EndsWith("Async"), "endpoint.methodName should end with Async.");
        Require(AllowedBindingSources.Contains(contract.Endpoint.BindingSource), "endpoint.bindingSource must be FromBody or FromQuery.");

        Require(contract.RequestDto.Namespace.StartsWith("Benchmark.Api.DTOs.", StringComparison.Ordinal), "requestDto.namespace must start with Benchmark.Api.DTOs.");
        Require(contract.RequestDto.Name.EndsWith("RequestDto"), "requestDto.name must end with RequestDto.");

        Require(
            contract.MediatorRequest.TypeName.EndsWith("Command") ||
            contract.MediatorRequest.TypeName.EndsWith("Query"),
            "mediatorRequest.typeName must end with Command or Query.");

        Require(
            contract.MediatorRequest.OptionsTypeName.EndsWith("CommandOptions") ||
            contract.MediatorRequest.OptionsTypeName.EndsWith("QueryOptions"),
            "mediatorRequest.optionsTypeName must end with CommandOptions or QueryOptions.");

        Require(contract.Response.HttpStatus is >= 200 and <= 299, "response.httpStatus must be a 2xx status for this MVP.");

        Require(
            contract.Mapping.Source == contract.RequestDto.Name,
            "mapping.source must match requestDto.name.");

        Require(
            contract.Mapping.Destination == contract.MediatorRequest.OptionsTypeName,
            "mapping.destination must match mediatorRequest.optionsTypeName.");
    }

    private static void Require(bool condition, string message)
    {
        if (!condition)
        {
            throw new InvalidOperationException($"Contract validation failed: {message}");
        }
    }
}

internal sealed class SymbolValidator
{
    private readonly string _root;

    public SymbolValidator(string root)
    {
        _root = root;
    }

    public void Validate(ApiEndpointGenerationContract contract)
    {
        if (contract.Mode == GenerationMode.AddMethodToExistingController)
        {
            var controllerPath = Path.Combine(
                _root,
                "src",
                "Benchmark.Api",
                "Controllers",
                $"{contract.Controller.Name}.cs");

            if (!File.Exists(controllerPath))
            {
                throw new InvalidOperationException(
                    $"Controller does not exist: {controllerPath}");
            }

            var controllerCode = File.ReadAllText(controllerPath);

            if (!controllerCode.Contains($"partial class {contract.Controller.Name}", StringComparison.Ordinal))
            {
                throw new InvalidOperationException(
                    $"Existing controller must be partial: {contract.Controller.Name}");
            }
        }
        
        RequireSymbolExists(contract.MediatorRequest.TypeName);
        RequireSymbolExists(contract.MediatorRequest.OptionsTypeName);
        RequireSymbolExists(contract.Response.TypeName);
    }

    private void RequireSymbolExists(string symbolName)
    {
        if (!SymbolExists(symbolName))
        {
            throw new InvalidOperationException(
                $"Required symbol was not found in solution source files: {symbolName}");
        }
    }

    private bool SymbolExists(string symbolName)
    {
        var files = Directory
            .EnumerateFiles(_root, "*.cs", SearchOption.AllDirectories)
            .Where(path =>
                !path.Contains($"{Path.DirectorySeparatorChar}bin{Path.DirectorySeparatorChar}", StringComparison.OrdinalIgnoreCase) &&
                !path.Contains($"{Path.DirectorySeparatorChar}obj{Path.DirectorySeparatorChar}", StringComparison.OrdinalIgnoreCase));

        foreach (var file in files)
        {
            var text = File.ReadAllText(file);

            if (text.Contains($"class {symbolName}", StringComparison.Ordinal) ||
                text.Contains($"record {symbolName}", StringComparison.Ordinal) ||
                text.Contains($"struct {symbolName}", StringComparison.Ordinal) ||
                text.Contains($"interface {symbolName}", StringComparison.Ordinal))
            {
                return true;
            }
        }

        return false;
    }
}

internal sealed class ApiGenerationPipeline
{
    private readonly GenerateRequestDtoSkill _requestDtoSkill = new();
    private readonly GenerateAutoMapperProfileSkill _profileSkill = new();
    private readonly GenerateControllerShellSkill _controllerShellSkill = new();
    private readonly GenerateControllerMethodSkill _controllerMethodSkill = new();

    public IReadOnlyList<GeneratedArtifact> Generate(ApiEndpointGenerationContract contract)
    {
        var artifacts = new List<GeneratedArtifact>
        {
            _requestDtoSkill.Generate(contract),
            _profileSkill.Generate(contract)
        };

        if (contract.Mode == GenerationMode.CreateNewController)
        {
            artifacts.Add(_controllerShellSkill.Generate(contract));
        }

        artifacts.Add(_controllerMethodSkill.Generate(contract));

        return artifacts;
    }
}

internal sealed class GenerateRequestDtoSkill
{
    public GeneratedArtifact Generate(ApiEndpointGenerationContract contract)
    {
        var code = RequestDtoRenderer.Render(contract);

        return new GeneratedArtifact
        {
            ArtifactKind = "RequestDto",
            RelativePath = ArtifactPaths.RequestDto(contract),
            Code = code,
            WriteMode = ArtifactWriteMode.OverwriteGeneratedOnly
        };
    }
}

internal sealed class GenerateAutoMapperProfileSkill
{
    public GeneratedArtifact Generate(ApiEndpointGenerationContract contract)
    {
        var code = AutoMapperProfileRenderer.Render(contract);

        return new GeneratedArtifact
        {
            ArtifactKind = "AutoMapperProfile",
            RelativePath = ArtifactPaths.Profile(contract),
            Code = code,
            WriteMode = ArtifactWriteMode.OverwriteGeneratedOnly
        };
    }
}

internal sealed class GenerateControllerShellSkill
{
    public GeneratedArtifact Generate(ApiEndpointGenerationContract contract)
    {
        var code = ControllerShellRenderer.Render(contract);

        return new GeneratedArtifact
        {
            ArtifactKind = "ControllerShell",
            RelativePath = ArtifactPaths.ControllerShell(contract),
            Code = code,
            WriteMode = ArtifactWriteMode.OverwriteGeneratedOnly
        };
    }
}

internal sealed class GenerateControllerMethodSkill
{
    public GeneratedArtifact Generate(ApiEndpointGenerationContract contract)
    {
        var code = ControllerMethodRenderer.Render(contract);

        return new GeneratedArtifact
        {
            ArtifactKind = "ControllerMethod",
            RelativePath = ArtifactPaths.ControllerMethod(contract),
            Code = code,
            WriteMode = ArtifactWriteMode.OverwriteGeneratedOnly
        };
    }
}

internal static class RequestDtoRenderer
{
    public static string Render(ApiEndpointGenerationContract contract)
    {
        var sb = new StringBuilder();

        sb.AppendLine("// <auto-generated />");
        sb.AppendLine("// Generated by Benchmark.AiGen. Do not edit manually.");
        sb.AppendLine();
        sb.AppendLine("using System.ComponentModel.DataAnnotations;");
        sb.AppendLine();
        sb.AppendLine($"namespace {contract.RequestDto.Namespace};");
        sb.AppendLine();
        sb.AppendLine($"public sealed class {contract.RequestDto.Name}");
        sb.AppendLine("{");

        foreach (var property in contract.RequestDto.Properties)
        {
            foreach (var attribute in property.Attributes)
            {
                sb.AppendLine($"    [{attribute}]");
            }

            var defaultValue = GetDefaultValue(property.Type);

            sb.AppendLine($"    public {property.Type} {property.Name} {{ get; init; }}{defaultValue}");
            sb.AppendLine();
        }

        sb.AppendLine("}");

        return sb.ToString();
    }

    private static string GetDefaultValue(string type)
    {
        return type == "string"
            ? " = string.Empty;"
            : string.Empty;
    }
}

internal static class AutoMapperProfileRenderer
{
    public static string Render(ApiEndpointGenerationContract contract)
    {
        return $$"""
// <auto-generated />
// Generated by Benchmark.AiGen. Do not edit manually.

using AutoMapper;
using {{contract.RequestDto.Namespace}};
using {{contract.MediatorRequest.Namespace}};

namespace Benchmark.Api.Profiles;

public sealed class {{contract.FeatureName}}Profile : Profile
{
    public {{contract.FeatureName}}Profile()
    {
        CreateMap<{{contract.RequestDto.Name}}, {{contract.MediatorRequest.OptionsTypeName}}>();
    }
}
""";
    }
}

internal static class ControllerShellRenderer
{
    public static string Render(ApiEndpointGenerationContract contract)
    {
        return $$"""
// <auto-generated />
// Generated by Benchmark.AiGen. Do not edit manually.

using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace {{contract.Controller.Namespace}};

[ApiController]
[Route("{{contract.Controller.RoutePrefix}}")]
public partial class {{contract.Controller.Name}} : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;

    public {{contract.Controller.Name}}(
        IMediator mediator,
        IMapper mapper)
    {
        _mediator = mediator;
        _mapper = mapper;
    }
}
""";
    }
}

internal static class ControllerMethodRenderer
{
    public static string Render(ApiEndpointGenerationContract contract)
    {
        var httpAttribute = RenderHttpAttribute(contract.Endpoint);
        var responseStatusConstant = ToStatusCodesConstant(contract.Response.HttpStatus);
        var bindingAttribute = contract.Endpoint.BindingSource;

        return $$"""
// <auto-generated />
// Generated by Benchmark.AiGen. Do not edit manually.

using {{contract.RequestDto.Namespace}};
using {{contract.MediatorRequest.Namespace}};
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace {{contract.Controller.Namespace}};

public partial class {{contract.Controller.Name}}
{
    {{httpAttribute}}
    [ProducesResponseType(typeof({{contract.Response.TypeName}}), StatusCodes.{{responseStatusConstant}})]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<{{contract.Response.TypeName}}>> {{contract.Endpoint.MethodName}}(
        [{{bindingAttribute}}] {{contract.RequestDto.Name}} request,
        CancellationToken cancellationToken)
    {
        var options = _mapper.Map<{{contract.MediatorRequest.OptionsTypeName}}>(request);
        var mediatorRequest = new {{contract.MediatorRequest.TypeName}}(options);

        var result = await _mediator.Send(mediatorRequest, cancellationToken);

        return Ok(result);
    }
}
""";
    }

    private static string RenderHttpAttribute(EndpointContract endpoint)
    {
        var verb = ToPascalCase(endpoint.HttpVerb);

        if (string.IsNullOrWhiteSpace(endpoint.Route))
        {
            return $"[Http{verb}]";
        }

        return $"[Http{verb}(\"{endpoint.Route}\")]";
    }

    private static string ToPascalCase(string value)
    {
        var lower = value.ToLowerInvariant();
        return char.ToUpperInvariant(lower[0]) + lower[1..];
    }

    private static string ToStatusCodesConstant(int statusCode)
    {
        return statusCode switch
        {
            200 => "Status200OK",
            201 => "Status201Created",
            202 => "Status202Accepted",
            204 => "Status204NoContent",
            _ => throw new InvalidOperationException($"Unsupported response status code: {statusCode}")
        };
    }
}

internal sealed class GeneratedCodeGuard
{
    private static readonly string[] ForbiddenTokens =
    {
        "DbContext",
        "IRepository",
        "Repository",
        "Benchmark.Infrastructure",
        "Microsoft.EntityFrameworkCore"
    };

    public void Validate(GeneratedArtifact artifact)
    {
        var syntaxTree = CSharpSyntaxTree.ParseText(artifact.Code);
        var diagnostics = syntaxTree
            .GetDiagnostics()
            .Where(diagnostic => diagnostic.Severity == DiagnosticSeverity.Error)
            .ToList();

        if (diagnostics.Count > 0)
        {
            throw new InvalidOperationException(
                $"Generated artifact {artifact.RelativePath} contains syntax errors:{Environment.NewLine}" +
                string.Join(Environment.NewLine, diagnostics));
        }

        foreach (var forbiddenToken in ForbiddenTokens)
        {
            if (artifact.Code.Contains(forbiddenToken, StringComparison.Ordinal))
            {
                throw new InvalidOperationException(
                    $"Generated artifact {artifact.RelativePath} contains forbidden token: {forbiddenToken}");
            }
        }

        if (artifact.ArtifactKind == "ControllerShell")
        {
            ValidateControllerShell(artifact);
        }

        if (artifact.ArtifactKind == "ControllerMethod")
        {
            ValidateControllerMethod(artifact);
        }
    }

    private static void ValidateControllerShell(GeneratedArtifact artifact)
    {
        var root = CSharpSyntaxTree.ParseText(artifact.Code).GetRoot();

        var controllerClass = root
            .DescendantNodes()
            .OfType<ClassDeclarationSyntax>()
            .FirstOrDefault();

        if (controllerClass is null)
        {
            throw new InvalidOperationException("Generated controller shell does not contain a class.");
        }

        if (!controllerClass.Modifiers.Any(modifier => modifier.Text == "partial"))
        {
            throw new InvalidOperationException("Generated controller shell must be partial.");
        }
    }

    private static void ValidateControllerMethod(GeneratedArtifact artifact)
    {
        if (!artifact.Code.Contains("_mediator.Send", StringComparison.Ordinal))
        {
            throw new InvalidOperationException("Controller method must use _mediator.Send.");
        }

        if (!artifact.Code.Contains("_mapper.Map", StringComparison.Ordinal))
        {
            throw new InvalidOperationException("Controller method must use _mapper.Map.");
        }
    }
}

internal sealed class ArtifactWriter
{
    private readonly string _root;

    public ArtifactWriter(string root)
    {
        _root = root;
    }

    public void Write(IReadOnlyList<GeneratedArtifact> artifacts, bool preview)
    {
        var basePath = preview
            ? Path.Combine(_root, "artifacts", "ai-preview")
            : _root;

        foreach (var artifact in artifacts)
        {
            var targetPath = Path.Combine(basePath, artifact.RelativePath);

            Directory.CreateDirectory(Path.GetDirectoryName(targetPath)!);

            if (File.Exists(targetPath))
            {
                var existing = File.ReadAllText(targetPath);

                if (!existing.Contains("<auto-generated />", StringComparison.Ordinal))
                {
                    throw new InvalidOperationException(
                        $"Refusing to overwrite human-written file: {targetPath}");
                }

                if (artifact.WriteMode != ArtifactWriteMode.OverwriteGeneratedOnly)
                {
                    throw new InvalidOperationException(
                        $"Refusing to overwrite existing file: {targetPath}");
                }
            }

            File.WriteAllText(targetPath, artifact.Code);
        }
    }
}

internal static class ArtifactPaths
{
    public static string RequestDto(ApiEndpointGenerationContract contract)
    {
        return Path.Combine(
            "src",
            "Benchmark.Api",
            "DTOs",
            contract.Area,
            $"{contract.RequestDto.Name}.cs");
    }

    public static string Profile(ApiEndpointGenerationContract contract)
    {
        return Path.Combine(
            "src",
            "Benchmark.Api",
            "Profiles",
            $"{contract.FeatureName}Profile.cs");
    }

    public static string ControllerShell(ApiEndpointGenerationContract contract)
    {
        return Path.Combine(
            "src",
            "Benchmark.Api",
            "Controllers",
            $"{contract.Controller.Name}.cs");
    }

    public static string ControllerMethod(ApiEndpointGenerationContract contract)
    {
        return Path.Combine(
            "src",
            "Benchmark.Api",
            "Controllers",
            $"{contract.Controller.Name}.{contract.FeatureName}.ai.cs");
    }
}

internal sealed record ApiEndpointGenerationContract
{
    public required string Service { get; init; }

    public required string FeatureName { get; init; }

    public required string Area { get; init; }

    public required GenerationMode Mode { get; init; }

    public required ControllerContract Controller { get; init; }

    public required EndpointContract Endpoint { get; init; }

    public required DtoContract RequestDto { get; init; }

    public required MediatorRequestContract MediatorRequest { get; init; }

    public required ResponseContract Response { get; init; }

    public required MappingContract Mapping { get; init; }
}

internal enum GenerationMode
{
    CreateNewController,
    AddMethodToExistingController
}

internal sealed record ControllerContract
{
    public required string Name { get; init; }

    public required string Namespace { get; init; }

    public required string RoutePrefix { get; init; }
}

internal sealed record EndpointContract
{
    public required string MethodName { get; init; }

    public required string HttpVerb { get; init; }

    public string Route { get; init; } = string.Empty;

    public required string BindingSource { get; init; }
}

internal sealed record DtoContract
{
    public required string Namespace { get; init; }

    public required string Name { get; init; }

    public required IReadOnlyList<DtoPropertyContract> Properties { get; init; }
}

internal sealed record DtoPropertyContract
{
    public required string Name { get; init; }

    public required string Type { get; init; }

    public IReadOnlyList<string> Attributes { get; init; } = Array.Empty<string>();
}

internal sealed record MediatorRequestContract
{
    public required string Namespace { get; init; }

    public required string TypeName { get; init; }

    public required string OptionsTypeName { get; init; }
}

internal sealed record ResponseContract
{
    public required string Namespace { get; init; }

    public required string TypeName { get; init; }

    public required int HttpStatus { get; init; }
}

internal sealed record MappingContract
{
    public required string Source { get; init; }

    public required string Destination { get; init; }

    public required string Mode { get; init; }
}

internal sealed record GeneratedArtifact
{
    public required string ArtifactKind { get; init; }

    public required string RelativePath { get; init; }

    public required string Code { get; init; }

    public required ArtifactWriteMode WriteMode { get; init; }
}

internal enum ArtifactWriteMode
{
    CreateOnly,
    OverwriteGeneratedOnly
}

internal static class RepositoryRootFinder
{
    public static string FindFrom(string startDirectory)
    {
        var directory = new DirectoryInfo(Path.GetFullPath(startDirectory));

        while (directory is not null)
        {
            if (directory.GetFiles("*.sln").Any())
            {
                return directory.FullName;
            }

            if (Directory.Exists(Path.Combine(directory.FullName, ".git")))
            {
                return directory.FullName;
            }

            directory = directory.Parent;
        }

        throw new DirectoryNotFoundException(
            $"Could not find repository root starting from: {startDirectory}");
    }
}