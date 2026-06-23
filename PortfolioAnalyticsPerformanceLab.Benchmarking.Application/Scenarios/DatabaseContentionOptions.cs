namespace PortfolioAnalyticsPerformanceLab.Benchmarking.Application.Scenarios;

public sealed class DatabaseContentionOptions: IScenarioOptions
{
    public int WorkerCount { get; set; }

    public int UpdatesPerWorker { get; set; }

    public DbUpdateStrategy UpdateStrategy { get; set; }

    public DbIsolationLevel IsolationLevel { get; set; }

    public bool UseOptimisticConcurrency { get; set; }

    public int BatchSize { get; set; }
}

public enum DbUpdateStrategy
{
    SingleSharedRow = 1,
    AppendOnlyEvents = 2,
    BatchedUpdates = 3
}

public enum DbIsolationLevel
{
    ReadCommitted = 1,
    RepeatableRead = 2,
    Serializable = 3,
    Snapshot = 4
}