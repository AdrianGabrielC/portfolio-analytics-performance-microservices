using MediatR;

namespace PortfolioAnalyticsPerformanceLab.Benchmarking.Application.Commands.CancelBenchmarkRun;

public class CancelBenchmarkRunCommand : IRequest<CancelBenchmarkRunCommandResult>
{
    public CancelBenchmarkRunCommandOptions Options { get; set; }

    public CancelBenchmarkRunCommand(CancelBenchmarkRunCommandOptions options) => Options = options;
}

public sealed class CancelBenchmarkRunCommandHandler : IRequestHandler<CancelBenchmarkRunCommand, CancelBenchmarkRunCommandResult>
{
    public CancelBenchmarkRunCommandHandler()
    {
    }

    public async Task<CancelBenchmarkRunCommandResult> Handle(CancelBenchmarkRunCommand request, CancellationToken cancellationToken)
    {
        // TODO: cancel benchmark run
        return new CancelBenchmarkRunCommandResult();
    }
}

