using MediatR;

namespace PortfolioAnalyticsPerformanceLab.Benchmarking.Application.Queries.CompareBenchmarkRunsByIds;

public class CompareBenchmarkRunsByIdsQuery : IRequest<CompareBenchmarkRunsByIdsQueryResult>
{
    public CompareBenchmarkRunsByIdsQueryOptions Options { get; set; }

    public CompareBenchmarkRunsByIdsQuery(CompareBenchmarkRunsByIdsQueryOptions options) => Options = options;
}

public sealed class CompareBenchmarkRunsByIdsQueryHandler : IRequestHandler<CompareBenchmarkRunsByIdsQuery, CompareBenchmarkRunsByIdsQueryResult>
{
    public CompareBenchmarkRunsByIdsQueryHandler()
    {
    }

    public async Task<CompareBenchmarkRunsByIdsQueryResult> Handle(CompareBenchmarkRunsByIdsQuery request, CancellationToken cancellationToken)
    {
        // TODO: compare benchmark runs by ids
        return new CompareBenchmarkRunsByIdsQueryResult();
    }
}

