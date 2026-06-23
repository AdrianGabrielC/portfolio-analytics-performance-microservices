using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Options;
using PortfolioAnalyticsPerformanceLab.Benchmarking.Application.Interfaces.Messaging;
using RabbitMQ.Client;

namespace PortfolioAnalyticsPerformanceLab.Benchmarking.Infrastructure.Messaging;

public sealed class RabbitMqMessagePublisher : IMessagePublisher
{
    private readonly RabbitMqOptions _options;

    public RabbitMqMessagePublisher(IOptions<RabbitMqOptions> options)
    {
        _options = options.Value;
    }

    public async Task PublishAsync<TMessage>(
        string queueName,
        TMessage message,
        Guid messageId,
        CancellationToken cancellationToken = default)
    {
        var json = JsonSerializer.Serialize(message);

        await PublishRawAsync(
            queueName,
            json,
            messageId,
            cancellationToken);
    }

    public async Task PublishRawAsync(
        string queueName,
        string payloadJson,
        Guid messageId,
        CancellationToken cancellationToken = default)
    {
        var factory = new ConnectionFactory
        {
            HostName = _options.HostName,
            Port = _options.Port,
            UserName = _options.UserName,
            Password = _options.Password
        };

        await using var connection =
            await factory.CreateConnectionAsync(cancellationToken);

        await using var channel =
            await connection.CreateChannelAsync(
                cancellationToken: cancellationToken);

        await channel.QueueDeclareAsync(
            queue: queueName,
            durable: true,
            exclusive: false,
            autoDelete: false,
            arguments: null,
            cancellationToken: cancellationToken);

        var body = Encoding.UTF8.GetBytes(payloadJson);

        var properties = new BasicProperties
        {
            Persistent = true,
            ContentType = "application/json",
            MessageId = messageId.ToString(),
            Timestamp = new AmqpTimestamp(
                DateTimeOffset.UtcNow.ToUnixTimeSeconds())
        };

        await channel.BasicPublishAsync(
            exchange: string.Empty,
            routingKey: queueName,
            mandatory: false,
            basicProperties: properties,
            body: body,
            cancellationToken: cancellationToken);
    }
}