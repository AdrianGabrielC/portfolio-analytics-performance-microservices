using System.Text.Json;
using PortfolioAnalyticsPerformanceLab.Benchmarking.Application.Scenarios;
using PortfolioAnalyticsPerformanceLab.Benchmarking.Domain.Enums;

namespace PortfolioAnalyticsPerformanceLab.Benchmarking.Application.Services;

public static class ScenarioConfigurationDeserializer
{
    public static object Deserialize(
        ScenarioType scenarioType,
        string json)
    {
        return scenarioType switch
        {
            ScenarioType.CpuValuation =>
                JsonSerializer.Deserialize<CpuValuationOptions>(json)!,

            ScenarioType.AsyncIoRisk =>
                JsonSerializer.Deserialize<AsyncIoRiskOptions>(json)!,

            ScenarioType.Saga =>
                JsonSerializer.Deserialize<SagaOptions>(json)!,

            ScenarioType.Outbox =>
                JsonSerializer.Deserialize<OutboxOptions>(json)!,

            ScenarioType.Idempotency =>
                JsonSerializer.Deserialize<IdempotencyOptions>(json)!,

            ScenarioType.DatabaseContention =>
                JsonSerializer.Deserialize<DatabaseContentionOptions>(json)!,

            ScenarioType.Cache =>
                JsonSerializer.Deserialize<CacheOptions>(json)!,

            ScenarioType.QueueBackpressure =>
                JsonSerializer.Deserialize<QueueBackpressureOptions>(json)!,

            _ => throw new NotSupportedException()
        };
    }
}