using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PortfolioAnalyticsPerformanceLab.Benchmarking.Infrastructure.BackgroundJobs;

namespace PortfolioAnalyticsPerformanceLab.Benchmarking.Infrastructure.Extensions;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration config)
    {
        services.AddHostedService<OutboxMessagePublisherBackgroundService>();
        
        return services;
    }
}