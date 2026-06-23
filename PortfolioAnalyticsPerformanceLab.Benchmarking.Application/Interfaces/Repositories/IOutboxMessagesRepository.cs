using PortfolioAnalyticsPerformanceLab.Benchmarking.Domain.Entities;

namespace PortfolioAnalyticsPerformanceLab.Benchmarking.Application.Interfaces.Repositories;

public interface IOutboxMessagesRepository
{
    Task<List<OutboxMessage>> GetAllOutboxMessagesAsync(bool trackChanges = false);
    Task<OutboxMessage> GetOutboxMessageByIdAsync(Guid outboxMessageId, bool trackChanges = false);
    Task<List<OutboxMessage>> GetPendingMessagesAsync(int take, CancellationToken cancellationToken);
    void CreateOutboxMessage(OutboxMessage outboxMessage);
    void UpdateOutboxMessage(OutboxMessage outboxMessage);
    void DeleteOutboxMessage(OutboxMessage outboxMessage);
}