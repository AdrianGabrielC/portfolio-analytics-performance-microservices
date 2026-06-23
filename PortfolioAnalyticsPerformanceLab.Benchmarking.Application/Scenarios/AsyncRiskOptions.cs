namespace PortfolioAnalyticsPerformanceLab.Benchmarking.Application.Scenarios;

public sealed class AsyncIoRiskOptions: IScenarioOptions
{
    public int PortfolioCount { get; set; }

    public int PositionsPerPortfolio { get; set; }

    public int ConcurrentRequests { get; set; }

    public int ExternalServiceDelayMs { get; set; }

    public double ExternalServiceFailureRate { get; set; }

    public int TimeoutMs { get; set; }

    public int RetryCount { get; set; }

    public bool UseBulkhead { get; set; }

    public int BulkheadMaxConcurrency { get; set; }
}