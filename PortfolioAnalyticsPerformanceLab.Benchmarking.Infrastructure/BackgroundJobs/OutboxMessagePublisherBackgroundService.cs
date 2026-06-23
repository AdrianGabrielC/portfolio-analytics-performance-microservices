using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using PortfolioAnalyticsPerformanceLab.Benchmarking.Application.Services;

namespace PortfolioAnalyticsPerformanceLab.Benchmarking.Infrastructure.BackgroundJobs;

public class OutboxMessagePublisherBackgroundService : BackgroundService
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly ILogger<OutboxMessagePublisherBackgroundService> _logger;
    
    public OutboxMessagePublisherBackgroundService(IServiceScopeFactory scopeFactory, ILogger<OutboxMessagePublisherBackgroundService> logger)
    {
        _scopeFactory = scopeFactory;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        using var timer = new PeriodicTimer(TimeSpan.FromSeconds(2));

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                using var scope = _scopeFactory.CreateScope();
                
                var processor = scope.ServiceProvider.GetRequiredService<ProcessOutboxMessagesService>();
                
                await processor.ProcessAsync(stoppingToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }

            await timer.WaitForNextTickAsync(stoppingToken);
        }
    }
}