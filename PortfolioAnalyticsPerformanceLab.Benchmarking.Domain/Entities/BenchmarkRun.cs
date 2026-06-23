using PortfolioAnalyticsPerformanceLab.Benchmarking.Domain.Enums;

namespace PortfolioAnalyticsPerformanceLab.Benchmarking.Domain.Entities;

public class BenchmarkRun
{
    public Guid Id { get; set; }

    public ScenarioType ScenarioType { get; set; }

    public BenchmarkStatus Status { get; set; }

    public DateTime CreatedAtUtc { get; set; }

    public DateTime? StartedAtUtc { get; set; }

    public DateTime? FinishedAtUtc { get; set; }

    public string ConfigurationJson { get; set; } = null!;

    public string? ResultJson { get; set; }

    public string? ErrorMessage { get; set; }

    public ICollection<BenchmarkRunEvent> Events { get; set; }
        = new List<BenchmarkRunEvent>();
    
}