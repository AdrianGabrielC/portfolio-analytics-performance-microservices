using PortfolioAnalyticsPerformanceLab.Benchmarking.Application.Scenarios;

namespace PortfolioAnalyticsPerformanceLab.Benchmarking.Application.Queries.GetBenchmarkScenarios;

public class GetBenchmarkScenariosQueryResult
{
    public IReadOnlyList<ScenarioDefinition> Scenarios { get; init; } = [];
}

