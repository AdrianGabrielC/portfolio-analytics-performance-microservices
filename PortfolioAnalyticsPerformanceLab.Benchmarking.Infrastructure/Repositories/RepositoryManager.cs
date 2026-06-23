using PortfolioAnalyticsPerformanceLab.Benchmarking.Application.Interfaces.Repositories;
using PortfolioAnalyticsPerformanceLab.Benchmarking.Infrastructure.Persistence;

namespace PortfolioAnalyticsPerformanceLab.Benchmarking.Infrastructure.Repositories;

public class RepositoryManager: IRepositoryManager
{
    private readonly BenchmarkingDbContext _repositoryContext;
    
    private Lazy<IBenchmarkRunsRepository> _benchmarkingRepository;
    private Lazy<IOutboxMessagesRepository> _outboxMessagesRepository;
    private Lazy<IBenchmarkRunEventsRepository> _benchmarkRunEventsRepository;

    public RepositoryManager(BenchmarkingDbContext repositoryContext)
    {
        _repositoryContext = repositoryContext;
        _benchmarkingRepository = new Lazy<IBenchmarkRunsRepository>(() => new PortfolioAnalyticsPerformanceLab.Benchmarking.Infrastructure.Repositories.BenchmarkRunsRepository.BenchmarkRunsRepository(repositoryContext));
        _outboxMessagesRepository = new Lazy<IOutboxMessagesRepository>(() => new OutboxMessagesRepository(repositoryContext));
        _benchmarkRunEventsRepository = new Lazy<IBenchmarkRunEventsRepository>(() => new BenchmarkRunEventsRepository(repositoryContext));
    }
    
    public IBenchmarkRunsRepository Benchmarking => _benchmarkingRepository.Value;
    public IOutboxMessagesRepository OutboxMessages => _outboxMessagesRepository.Value;
    public  IBenchmarkRunEventsRepository BenchmarkRunEvents => _benchmarkRunEventsRepository.Value;
    public async Task SaveAsync() => await _repositoryContext.SaveChangesAsync();
}
