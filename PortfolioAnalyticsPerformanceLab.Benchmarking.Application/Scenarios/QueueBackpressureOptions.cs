namespace PortfolioAnalyticsPerformanceLab.Benchmarking.Application.Scenarios;

public sealed class QueueBackpressureOptions: IScenarioOptions
{
    public int JobCount { get; set; }

    public int ProducerRatePerSecond { get; set; }

    public int WorkerCount { get; set; }

    public int ProcessingTimeMs { get; set; }

    public int MaxQueueLength { get; set; }

    public BackpressureStrategy BackpressureStrategy { get; set; }
}

public enum BackpressureStrategy
{
    None = 1,
    RejectNewJobs = 2,
    SlowDownProducer = 3,
    QueueAndWait = 4
}