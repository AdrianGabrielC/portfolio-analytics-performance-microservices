using MediatR;

namespace PortfolioAnalyticsPerformanceLab.Benchmarking.Application.Queries.GetBenchmarkRunEventById;

public class GetBenchmarkRunEventByIdQuery : IRequest<GetBenchmarkRunEventByIdQueryResult>
{
    public GetBenchmarkRunEventByIdQueryOptions Options { get; set; }

    public GetBenchmarkRunEventByIdQuery(GetBenchmarkRunEventByIdQueryOptions options) => Options = options;
}

public sealed class GetBenchmarkRunEventByIdQueryHandler : IRequestHandler<GetBenchmarkRunEventByIdQuery, GetBenchmarkRunEventByIdQueryResult>
{
    public GetBenchmarkRunEventByIdQueryHandler()
    {
    }

    public async Task<GetBenchmarkRunEventByIdQueryResult> Handle(GetBenchmarkRunEventByIdQuery request, CancellationToken cancellationToken)
    {
        // TODO: fetch BenchmarkRun event by id from persistence layer
        return new GetBenchmarkRunEventByIdQueryResult();
    }
}

