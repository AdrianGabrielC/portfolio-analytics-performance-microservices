using PortfolioAnalyticsPerformanceLab.Benchmarking.Domain.Entities;

namespace PortfolioAnalyticsPerformanceLab.Benchmarking.Application.Interfaces.Repositories;

public interface IBenchmarkRunEventsRepository
{
    Task<List<BenchmarkRunEvent>> GetAllBenchmarkRunEventsAsync(bool trackChanges = false);
    Task<BenchmarkRunEvent> GetBenchmarkRunEventByIdAsync(Guid benchmarkRunEventId, bool trackChanges = false);
    
    void CreateBenchmarkRunEvent(BenchmarkRunEvent benchmarkRunEvent);
    void UpdateBenchmarkRunEvent(BenchmarkRunEvent benchmarkRunEvent);
    void DeleteBenchmarkRunEvent(BenchmarkRunEvent benchmarkRunEvent);
}