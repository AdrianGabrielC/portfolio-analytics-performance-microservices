using System.Text.Json;
using MediatR;
using PortfolioAnalyticsPerformanceLab.Benchmarking.Application.Interfaces.Repositories;
using PortfolioAnalyticsPerformanceLab.Benchmarking.Application.Messaging.Messages;
using PortfolioAnalyticsPerformanceLab.Benchmarking.Domain.Entities;
using PortfolioAnalyticsPerformanceLab.Benchmarking.Domain.Enums;

namespace PortfolioAnalyticsPerformanceLab.Benchmarking.Application.Commands.CreateBenchmarkRun;

public class CreateBenchmarkRunCommand: IRequest<CreateBenchmarkRunCommandResult>
{
    public CreateBenchmarkRunCommandOptions Options { get; set; }
    public CreateBenchmarkRunCommand(CreateBenchmarkRunCommandOptions options) => Options = options;
}

public sealed class CreateBenchmarkRunCommandHandler : IRequestHandler<CreateBenchmarkRunCommand, CreateBenchmarkRunCommandResult>
{
    private readonly IRepositoryManager _repositoryManager;
    
    public CreateBenchmarkRunCommandHandler(IRepositoryManager repositoryManager)
    {
        _repositoryManager  = repositoryManager; 
    }

    public async Task<CreateBenchmarkRunCommandResult> Handle(CreateBenchmarkRunCommand request,
        CancellationToken cancellationToken)
    {
        var now = DateTime.UtcNow;
        var benchmarkRun = new BenchmarkRun
        {
            Id = Guid.NewGuid(),
            ScenarioType = request.Options.ScenarioType,
            Status = BenchmarkStatus.Pending,
            CreatedAtUtc = now,
            ConfigurationJson = JsonSerializer.Serialize(request.Options.Configuration)
        };
        
        var runCreatedEvent = new BenchmarkRunEvent
        {
            Id = Guid.NewGuid(),
            BenchmarkRunId = benchmarkRun.Id,
            EventType = BenchmarkRunEventTypes.BenchmarkRunCreated,
            PayloadJson = JsonSerializer.Serialize(new
            {
                benchmarkRun.Id,
                benchmarkRun.ScenarioType,
                benchmarkRun.CreatedAtUtc
            }),
            CreatedAtUtc = now
        };

        var message = new RunBenchmarkMessage(
            MessageId: Guid.NewGuid(),
            BenchmarkRunId: benchmarkRun.Id,
            ScenarioType: benchmarkRun.ScenarioType,
            ConfigurationJson: benchmarkRun.ConfigurationJson,
            RequestedAtUtc: now);

        var outboxMessage = new OutboxMessage
        {
            Id = message.MessageId,
            OccurredAtUtc = now,
            Type = nameof(RunBenchmarkMessage),
            QueueName = "benchmarking.run-benchmark",
            PayloadJson = JsonSerializer.Serialize(message),
            Status = OutboxMessageStatus.Pending,
            RetryCount = 0
        };
        
        _repositoryManager.Benchmarking.CreateBenchmarkRun(benchmarkRun);
        _repositoryManager.BenchmarkRunEvents.CreateBenchmarkRunEvent(runCreatedEvent);
        _repositoryManager.OutboxMessages.CreateOutboxMessage(outboxMessage);
        await _repositoryManager.SaveAsync();
        
        return new CreateBenchmarkRunCommandResult { Id = benchmarkRun.Id };
    }
}