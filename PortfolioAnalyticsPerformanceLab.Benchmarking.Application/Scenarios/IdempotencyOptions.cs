namespace PortfolioAnalyticsPerformanceLab.Benchmarking.Application.Scenarios;

public sealed class IdempotencyOptions: IScenarioOptions
{
    public int UniqueMessageCount { get; set; }

    public int DuplicateCountPerMessage { get; set; }

    public bool EnableInbox { get; set; }

    public int ConsumerCount { get; set; }
}