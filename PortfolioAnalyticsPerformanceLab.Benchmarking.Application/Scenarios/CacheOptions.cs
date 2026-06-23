namespace PortfolioAnalyticsPerformanceLab.Benchmarking.Application.Scenarios;

public sealed class CacheOptions: IScenarioOptions
{
    public int RequestCount { get; set; }

    public int UniqueInstrumentCount { get; set; }

    public CacheStrategy CacheStrategy { get; set; }

    public int CacheTtlSeconds { get; set; }

    public double CacheMissPenaltyMs { get; set; }
}

public enum CacheStrategy
{
    NoCache = 1,
    MemoryCache = 2,
    RedisCache = 3
}