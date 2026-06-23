namespace PortfolioAnalyticsPerformanceLab.Benchmarking.Domain.Enums;

public enum ScenarioType
{
    CpuValuation = 1,
    AsyncIoRisk = 2,
    Saga = 3,
    Outbox = 4,
    Idempotency = 5,
    DatabaseContention = 6,
    Cache = 7,
    QueueBackpressure = 8
}