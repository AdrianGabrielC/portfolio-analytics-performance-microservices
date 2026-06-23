using Microsoft.EntityFrameworkCore;
using PortfolioAnalyticsPerformanceLab.Benchmarking.Application.Interfaces.Repositories;
using PortfolioAnalyticsPerformanceLab.Benchmarking.Domain.Entities;
using PortfolioAnalyticsPerformanceLab.Benchmarking.Infrastructure.Persistence;
using PortfolioAnalyticsPerformanceLab.Benchmarking.Infrastructure.Repositories.BaseRepository;

namespace PortfolioAnalyticsPerformanceLab.Benchmarking.Infrastructure.Repositories;

public sealed class BenchmarkRunEventsRepository : RepositoryBase<BenchmarkRunEvent>, IBenchmarkRunEventsRepository
{
    public BenchmarkRunEventsRepository(BenchmarkingDbContext context) : base(context)
    {
    }

    public async Task<List<BenchmarkRunEvent>> GetAllBenchmarkRunEventsAsync(bool trackChanges = false) => await
        GetAll(trackChanges)
            .OrderBy(e => e.CreatedAtUtc)
            .ToListAsync();

    public async Task<BenchmarkRunEvent> GetBenchmarkRunEventByIdAsync(Guid benchmarkRunEventId, bool trackChanges = false) => await
        FindByCondition(e => e.Id.Equals(benchmarkRunEventId), trackChanges)
            .FirstOrDefaultAsync();

    public void CreateBenchmarkRunEvent(BenchmarkRunEvent benchmarkRunEvent) => Create(benchmarkRunEvent);
    public void UpdateBenchmarkRunEvent(BenchmarkRunEvent benchmarkRunEvent) => Update(benchmarkRunEvent);
    public void DeleteBenchmarkRunEvent(BenchmarkRunEvent benchmarkRunEvent) => Delete(benchmarkRunEvent);
}