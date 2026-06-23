using MediatR;

namespace PortfolioAnalyticsPerformanceLab.Benchmarking.Application.Queries.GetBenchmarkRunById;

public class GetBenchmarkRunByIdQuery : IRequest<GetBenchmarkRunByIdQueryResult>
{
    public GetBenchmarkRunByIdQueryOptions Options { get; set; }

    public GetBenchmarkRunByIdQuery(GetBenchmarkRunByIdQueryOptions options) => Options = options;
}

public sealed class GetBenchmarkRunByIdQueryHandler : IRequestHandler<GetBenchmarkRunByIdQuery, GetBenchmarkRunByIdQueryResult>
{
    public GetBenchmarkRunByIdQueryHandler()
    {
    }

    public async Task<GetBenchmarkRunByIdQueryResult> Handle(GetBenchmarkRunByIdQuery request, CancellationToken cancellationToken)
    {
        // TODO: fetch BenchmarkRun by id from persistence layer
        return new GetBenchmarkRunByIdQueryResult();
    }
}
