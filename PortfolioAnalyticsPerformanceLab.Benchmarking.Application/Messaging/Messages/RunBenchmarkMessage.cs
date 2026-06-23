using PortfolioAnalyticsPerformanceLab.Benchmarking.Domain.Enums;

namespace PortfolioAnalyticsPerformanceLab.Benchmarking.Application.Messaging.Messages;

public sealed record RunBenchmarkMessage(
    Guid MessageId,
    Guid BenchmarkRunId,
    ScenarioType ScenarioType,
    string ConfigurationJson,
    DateTime RequestedAtUtc);