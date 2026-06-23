using Microsoft.EntityFrameworkCore;
using PortfolioAnalyticsPerformanceLab.Benchmarking.Application.Interfaces.Repositories;
using PortfolioAnalyticsPerformanceLab.Benchmarking.Domain.Entities;
using PortfolioAnalyticsPerformanceLab.Benchmarking.Infrastructure.Persistence;
using PortfolioAnalyticsPerformanceLab.Benchmarking.Infrastructure.Repositories.BaseRepository;

namespace PortfolioAnalyticsPerformanceLab.Benchmarking.Infrastructure.Repositories;

public class OutboxMessagesRepository: RepositoryBase<OutboxMessage>, IOutboxMessagesRepository
{
    public OutboxMessagesRepository(BenchmarkingDbContext context) : base(context)
    {
    }
    
    public async Task<List<OutboxMessage>> GetAllOutboxMessagesAsync(bool trackChanges = false) => await 
        GetAll(trackChanges)
            .OrderBy(br => br.OccurredAtUtc)
            .ToListAsync();
    
    public async Task<OutboxMessage> GetOutboxMessageByIdAsync(Guid outboxMessageId, bool trackChanges = false) => await
        FindByCondition(br => br.Id.Equals(outboxMessageId), trackChanges)
            .FirstOrDefaultAsync();

    public async Task<List<OutboxMessage>> GetPendingMessagesAsync(int take, CancellationToken cancellationToken) => await
        GetAll(true)
            .Where(x => x.Status == OutboxMessageStatus.Pending)
            .OrderBy(x => x.OccurredAtUtc)
            .Take(take)
            .ToListAsync(cancellationToken);
    
    public void CreateOutboxMessage(OutboxMessage outboxMessage) => Create(outboxMessage);
    public void UpdateOutboxMessage(OutboxMessage outboxMessage) => Update(outboxMessage);
    public void DeleteOutboxMessage(OutboxMessage outboxMessage) => Delete(outboxMessage);
}

