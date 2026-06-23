namespace PortfolioAnalyticsPerformanceLab.Benchmarking.Domain.Entities;

public sealed class OutboxMessage
{
    public Guid Id { get; set; }

    public DateTime OccurredAtUtc { get; set; }

    public string Type { get; set; } = null!;

    public string QueueName { get; set; } = null!;

    public string PayloadJson { get; set; } = null!;

    public OutboxMessageStatus Status { get; set; }

    public int RetryCount { get; set; }

    public DateTime? PublishedAtUtc { get; set; }

    public string? ErrorMessage { get; set; }
}

public enum OutboxMessageStatus
{
    Pending = 1,
    Published = 2,
    Failed = 3
}