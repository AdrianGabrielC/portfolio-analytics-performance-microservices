using PortfolioAnalyticsPerformanceLab.Benchmarking.Domain.Enums;

namespace PortfolioAnalyticsPerformanceLab.Benchmarking.Application.Scenarios;

public sealed record ScenarioDefinition(
    ScenarioType Type,
    string Name,
    string Description,
    object DefaultConfiguration);