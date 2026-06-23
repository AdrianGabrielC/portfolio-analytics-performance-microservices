using MediatR;

namespace PortfolioAnalyticsPerformanceLab.Benchmarking.Application.Queries.GetBenchmarkRunProgressById;

public class GetBenchmarkRunProgressByIdQuery : IRequest<GetBenchmarkRunProgressByIdQueryResult>
{
    public GetBenchmarkRunProgressByIdQueryOptions Options { get; set; }

    public GetBenchmarkRunProgressByIdQuery(GetBenchmarkRunProgressByIdQueryOptions options) => Options = options;
}

public sealed class GetBenchmarkRunProgressByIdQueryHandler : IRequestHandler<GetBenchmarkRunProgressByIdQuery, GetBenchmarkRunProgressByIdQueryResult>
{
    public GetBenchmarkRunProgressByIdQueryHandler()
    {
    }

    public async Task<GetBenchmarkRunProgressByIdQueryResult> Handle(GetBenchmarkRunProgressByIdQuery request, CancellationToken cancellationToken)
    {
        // TODO: fetch BenchmarkRun progress by id from persistence layer
        return new GetBenchmarkRunProgressByIdQueryResult();
    }
}

