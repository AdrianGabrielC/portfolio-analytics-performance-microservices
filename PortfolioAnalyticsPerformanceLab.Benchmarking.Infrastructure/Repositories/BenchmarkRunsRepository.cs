using Microsoft.EntityFrameworkCore;
using PortfolioAnalyticsPerformanceLab.Benchmarking.Application.Interfaces.Repositories;
using PortfolioAnalyticsPerformanceLab.Benchmarking.Domain.Entities;
using PortfolioAnalyticsPerformanceLab.Benchmarking.Infrastructure.Persistence;
using PortfolioAnalyticsPerformanceLab.Benchmarking.Infrastructure.Repositories.BaseRepository;

namespace PortfolioAnalyticsPerformanceLab.Benchmarking.Infrastructure.Repositories.BenchmarkRunsRepository;

public sealed class BenchmarkRunsRepository : RepositoryBase<BenchmarkRun>, IBenchmarkRunsRepository
{
    public BenchmarkRunsRepository(BenchmarkingDbContext context) : base(context)
    {
    }
    
    public async Task<List<BenchmarkRun>> GetAllBenchmarkRunsAsync(bool trackChanges = false) => await 
        GetAll(trackChanges)
            .OrderBy(br => br.CreatedAtUtc)
            .ToListAsync();
    
    public async Task<BenchmarkRun> GetBenchmarkRunByIdAsync(Guid id, bool trackChanges) => await
        FindByCondition(br => br.Id.Equals(id), trackChanges)
            .FirstOrDefaultAsync();
    
    public void CreateBenchmarkRun(BenchmarkRun benchmarkRun) => Create(benchmarkRun);
    public void UpdateBenchmarkRun(BenchmarkRun benchmarkRun) => Update(benchmarkRun);
    public void DeleteBenchmarkRun(BenchmarkRun benchmarkRun) => Delete(benchmarkRun);
}