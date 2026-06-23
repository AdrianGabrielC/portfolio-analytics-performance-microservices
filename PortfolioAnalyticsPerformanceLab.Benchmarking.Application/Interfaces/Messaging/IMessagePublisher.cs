namespace PortfolioAnalyticsPerformanceLab.Benchmarking.Application.Interfaces.Messaging;

public interface IMessagePublisher
{
    Task PublishAsync<TMessage>(
        string queueName,
        TMessage message,
        Guid messageId,
        CancellationToken cancellationToken = default);

    Task PublishRawAsync(
        string queueName,
        string payloadJson,
        Guid messageId,
        CancellationToken cancellationToken = default);
}