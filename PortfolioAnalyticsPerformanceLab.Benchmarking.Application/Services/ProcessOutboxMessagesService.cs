using PortfolioAnalyticsPerformanceLab.Benchmarking.Application.Interfaces.Messaging;
using PortfolioAnalyticsPerformanceLab.Benchmarking.Application.Interfaces.Repositories;
using PortfolioAnalyticsPerformanceLab.Benchmarking.Domain.Entities;

namespace PortfolioAnalyticsPerformanceLab.Benchmarking.Application.Services;

public sealed class ProcessOutboxMessagesService
{
    private readonly IRepositoryManager _repositoryManager;
    private readonly IMessagePublisher _, _messagePublisher;
    
    public ProcessOutboxMessagesService(IRepositoryManager repositoryManager, IMessagePublisher messagePublisher)
    {
        _repositoryManager = repositoryManager;
        _messagePublisher = messagePublisher;
    }

    public async Task ProcessAsync(CancellationToken cancellationToken)
    {
        var messages = await _repositoryManager.OutboxMessages.GetPendingMessagesAsync(take: 20, cancellationToken);
        foreach (var outboxMessage in messages)
        {
            try
            {
                await _messagePublisher.PublishRawAsync(
                    queueName: outboxMessage.QueueName,
                    payloadJson: outboxMessage.PayloadJson,
                    messageId: outboxMessage.Id,
                    cancellationToken: cancellationToken);

                outboxMessage.Status = OutboxMessageStatus.Published;
                outboxMessage.PublishedAtUtc = DateTime.UtcNow;
                outboxMessage.ErrorMessage = null;
            }
            catch (Exception ex)
            {
                outboxMessage.RetryCount++;
                outboxMessage.ErrorMessage = ex.Message;
                if (outboxMessage.RetryCount >= 5)
                {
                    outboxMessage.Status = OutboxMessageStatus.Failed;
                }
            }
        }

        await _repositoryManager.SaveAsync();
    }
}