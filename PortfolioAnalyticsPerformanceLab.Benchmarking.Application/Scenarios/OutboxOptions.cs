namespace PortfolioAnalyticsPerformanceLab.Benchmarking.Application.Scenarios;

public sealed class OutboxOptions: IScenarioOptions
{
    public int EventCount { get; set; }

    public int PublisherBatchSize { get; set; }

    public int PublisherIntervalMs { get; set; }

    public bool UsePublishConfirmations { get; set; }

    public double BrokerFailureRate { get; set; }
}