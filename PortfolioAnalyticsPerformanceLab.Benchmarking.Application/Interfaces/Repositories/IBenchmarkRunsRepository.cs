using PortfolioAnalyticsPerformanceLab.Benchmarking.Domain.Entities;

namespace PortfolioAnalyticsPerformanceLab.Benchmarking.Application.Interfaces.Repositories;

public interface IBenchmarkRunsRepository
{
    Task<List<BenchmarkRun>> GetAllBenchmarkRunsAsync(bool trackChanges = false);
    Task<BenchmarkRun> GetBenchmarkRunByIdAsync(Guid benchmarkRunId, bool trackChanges = false);
    
    void CreateBenchmarkRun(BenchmarkRun benchmarkRun);
    void UpdateBenchmarkRun(BenchmarkRun benchmarkRun);
    void DeleteBenchmarkRun(BenchmarkRun benchmarkRun);
}