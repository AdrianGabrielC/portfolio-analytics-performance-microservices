namespace PortfolioAnalyticsPerformanceLab.Benchmarking.Application.Interfaces.Repositories;

public interface IRepositoryManager
{
    IBenchmarkRunsRepository Benchmarking { get; }
    IOutboxMessagesRepository OutboxMessages { get; }
    IBenchmarkRunEventsRepository BenchmarkRunEvents { get; }
    Task SaveAsync();
}