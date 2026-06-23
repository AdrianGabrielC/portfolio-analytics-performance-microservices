using MediatR;

namespace PortfolioAnalyticsPerformanceLab.Benchmarking.Application.Queries.GetBenchmarkRuns;

public class GetBenchmarkRunsQuery : IRequest<GetBenchmarkRunsQueryResult>
{
    public GetBenchmarkRunsQueryOptions Options { get; set; }

    public GetBenchmarkRunsQuery(GetBenchmarkRunsQueryOptions options) => Options = options;
}

public sealed class GetBenchmarkRunsQueryHandler : IRequestHandler<GetBenchmarkRunsQuery, GetBenchmarkRunsQueryResult>
{
    public GetBenchmarkRunsQueryHandler()
    {
    }

    public async Task<GetBenchmarkRunsQueryResult> Handle(GetBenchmarkRunsQuery request, CancellationToken cancellationToken)
    {
        // TODO: fetch BenchmarkRuns from persistence layer
        return new GetBenchmarkRunsQueryResult();
    }
}
