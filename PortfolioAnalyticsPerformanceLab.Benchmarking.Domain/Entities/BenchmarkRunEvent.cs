namespace PortfolioAnalyticsPerformanceLab.Benchmarking.Domain.Entities;

public class BenchmarkRunEvent
{
    public Guid Id { get; set; }

    public Guid BenchmarkRunId { get; set; }

    public BenchmarkRun BenchmarkRun { get; set; } = null!;

    public string EventType { get; set; } = null!;

    public string PayloadJson { get; set; } = null!;

    public DateTime CreatedAtUtc { get; set; }
}