namespace PortfolioAnalyticsPerformanceLab.Benchmarking.Application.Scenarios;

public sealed class SagaOptions: IScenarioOptions
{
    public int WorkflowCount { get; set; }

    public bool FailPortfolioStep { get; set; }

    public bool FailMarketDataStep { get; set; }

    public bool FailPricingStep { get; set; }

    public bool FailRiskStep { get; set; }

    public double FailureRate { get; set; }

    public int RetryCount { get; set; }

    public int RetryDelayMs { get; set; }

    public bool EnableCompensation { get; set; }

    public bool EnableIdempotency { get; set; }
}