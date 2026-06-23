using PortfolioAnalyticsPerformanceLab.Benchmarking.Domain.Enums;

namespace PortfolioAnalyticsPerformanceLab.Benchmarking.Application.Scenarios;

public static class ScenarioCatalog
{
    public static readonly IReadOnlyList<ScenarioDefinition> All =
    [
        new(
            ScenarioType.CpuValuation,
            "CPU-bound portfolio valuation",
            "Benchmark CPU parallelization strategies",
            new CpuValuationOptions
            {
                PortfolioCount = 100,
                PositionsPerPortfolio = 1000,
                MarketScenarioCount = 500,
                BatchSize = 100,
                WorkerCount = 4,
                MaxDegreeOfParallelism = 4,
                ExecutionMode = CpuExecutionMode.ParallelForEach
            }),

        new(
            ScenarioType.AsyncIoRisk,
            "Async I/O risk aggregation",
            "Benchmark concurrent external service calls",
            new AsyncIoRiskOptions
            {
                PortfolioCount = 100,
                PositionsPerPortfolio = 1000,
                ConcurrentRequests = 50,
                ExternalServiceDelayMs = 100,
                ExternalServiceFailureRate = 0.05,
                TimeoutMs = 2000,
                RetryCount = 2,
                UseBulkhead = true,
                BulkheadMaxConcurrency = 25
            }),

        new(
            ScenarioType.Saga,
            "Saga distributed transaction",
            "Benchmark saga orchestration, retries, compensation and idempotency",
            new SagaOptions
            {
                WorkflowCount = 100,

                FailPortfolioStep = false,
                FailMarketDataStep = false,
                FailPricingStep = true,
                FailRiskStep = false,

                FailureRate = 0.10,

                RetryCount = 2,
                RetryDelayMs = 500,

                EnableCompensation = true,
                EnableIdempotency = true
            }),

        new(
            ScenarioType.Outbox,
            "Outbox throughput",
            "Benchmark transactional outbox publishing performance",
            new OutboxOptions
            {
                EventCount = 10_000,
                PublisherBatchSize = 100,
                PublisherIntervalMs = 100,

                UsePublishConfirmations = true,
                BrokerFailureRate = 0.02
            }),

        new(
            ScenarioType.Idempotency,
            "Idempotent consumer",
            "Benchmark duplicate message handling and inbox pattern",
            new IdempotencyOptions
            {
                UniqueMessageCount = 10_000,
                DuplicateCountPerMessage = 3,

                EnableInbox = true,
                ConsumerCount = 4
            }),

        new(
            ScenarioType.DatabaseContention,
            "Database contention",
            "Benchmark concurrent database update strategies",
            new DatabaseContentionOptions
            {
                WorkerCount = 8,
                UpdatesPerWorker = 1_000,

                UpdateStrategy = DbUpdateStrategy.SingleSharedRow,
                IsolationLevel = DbIsolationLevel.ReadCommitted,

                UseOptimisticConcurrency = true,
                BatchSize = 100
            }),

        new(
            ScenarioType.Cache,
            "Cache vs database market data",
            "Benchmark market data retrieval with different cache strategies",
            new CacheOptions
            {
                RequestCount = 100_000,
                UniqueInstrumentCount = 1_000,

                CacheStrategy = CacheStrategy.MemoryCache,

                CacheTtlSeconds = 300,
                CacheMissPenaltyMs = 25
            }),

        new(
            ScenarioType.QueueBackpressure,
            "Queue backpressure processing",
            "Benchmark producer/consumer throughput under load",
            new QueueBackpressureOptions
            {
                JobCount = 50_000,
                ProducerRatePerSecond = 1_000,

                WorkerCount = 8,
                ProcessingTimeMs = 50,

                MaxQueueLength = 10_000,
                BackpressureStrategy = BackpressureStrategy.SlowDownProducer
            })
    ];
}