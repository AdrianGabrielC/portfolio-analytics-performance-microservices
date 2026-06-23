using MediatR;
using PortfolioAnalyticsPerformanceLab.Benchmarking.Application.Scenarios;

namespace PortfolioAnalyticsPerformanceLab.Benchmarking.Application.Queries.GetBenchmarkScenarios;

public class GetBenchmarkScenariosQuery : IRequest<GetBenchmarkScenariosQueryResult>
{
    public GetBenchmarkScenariosQueryOptions Options { get; set; }

    public GetBenchmarkScenariosQuery(GetBenchmarkScenariosQueryOptions options) => Options = options;
}

public sealed class GetBenchmarkScenariosQueryHandler : IRequestHandler<GetBenchmarkScenariosQuery, GetBenchmarkScenariosQueryResult>
{
    public GetBenchmarkScenariosQueryHandler()
    {
    }

    public async Task<GetBenchmarkScenariosQueryResult> Handle(GetBenchmarkScenariosQuery request, CancellationToken cancellationToken)
    {
        return new()
        {
            Scenarios = ScenarioCatalog.All
        };
    }
}

