using MediatR;

namespace PortfolioAnalyticsPerformanceLab.Benchmarking.Application.Queries.GetBenchmarkRunResultById;

public class GetBenchmarkRunResultByIdQuery : IRequest<GetBenchmarkRunResultByIdQueryResult>
{
    public GetBenchmarkRunResultByIdQueryOptions Options { get; set; }

    public GetBenchmarkRunResultByIdQuery(GetBenchmarkRunResultByIdQueryOptions options) => Options = options;
}

public sealed class GetBenchmarkRunResultByIdQueryHandler : IRequestHandler<GetBenchmarkRunResultByIdQuery, GetBenchmarkRunResultByIdQueryResult>
{
    public GetBenchmarkRunResultByIdQueryHandler()
    {
    }

    public async Task<GetBenchmarkRunResultByIdQueryResult> Handle(GetBenchmarkRunResultByIdQuery request, CancellationToken cancellationToken)
    {
        // TODO: fetch BenchmarkRun result by id from persistence layer
        return new GetBenchmarkRunResultByIdQueryResult();
    }
}

