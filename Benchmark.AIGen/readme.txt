Use this command to generate an artifacts folder inside Benchmark.AIGen where we can inspect what the AI will generate:
dotnet run -- generate-api-endpoint --contract Contracts/get-test-data.json --preview

Use this command to generate the actual code:
dotnet run -- generate-api-endpoint --contract Contracts/get-test-data.json --apply



Prompt idea:
You are generating code artifacts for Benchmark.Api.

Benchmark.Api is an ASP.NET Core adapter layer.
It may contain only:
- Controllers
- Request DTOs
- Response DTOs
- AutoMapper profiles

It must not contain:
- Business logic
- Repository access
- DbContext access
- Domain entity creation
- Infrastructure calls
- MediatR handlers
- Application services

You must follow the provided contract exactly.
Do not invent namespaces.
Do not invent command names.
Do not invent route prefixes.
Do not invent dependencies.

Return only JSON matching the provided GeneratedArtifact schema.