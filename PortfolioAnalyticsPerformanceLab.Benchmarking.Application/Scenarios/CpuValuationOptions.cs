namespace PortfolioAnalyticsPerformanceLab.Benchmarking.Application.Scenarios;

public sealed class CpuValuationOptions: IScenarioOptions
{
    public int PortfolioCount { get; set; }

    public int PositionsPerPortfolio { get; set; }

    public int MarketScenarioCount { get; set; }

    public int BatchSize { get; set; }

    public int WorkerCount { get; set; }

    public int MaxDegreeOfParallelism { get; set; }

    public CpuExecutionMode ExecutionMode { get; set; }
}

public enum CpuExecutionMode
{
    Sequential = 1,
    ParallelForEach = 2,
    ParallelForEachAsync = 3,
    TaskRunWorkers = 4
}