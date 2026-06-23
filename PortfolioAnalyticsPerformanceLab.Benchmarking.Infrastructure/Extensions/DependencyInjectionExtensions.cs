using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PortfolioAnalyticsPerformanceLab.Benchmarking.Application.Interfaces.Repositories;
using PortfolioAnalyticsPerformanceLab.Benchmarking.Infrastructure.BackgroundJobs;
using PortfolioAnalyticsPerformanceLab.Benchmarking.Infrastructure.Persistence;
using PortfolioAnalyticsPerformanceLab.Benchmarking.Infrastructure.Repositories;

namespace PortfolioAnalyticsPerformanceLab.Benchmarking.Infrastructure.Extensions;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration config)
    {
        services.AddDbContext<BenchmarkingDbContext>(options =>
        {
            options.UseSqlServer(config.GetConnectionString("BenchmarkingDb"));
        });
        
        services.AddScoped<IRepositoryManager, RepositoryManager>();
        
        services.AddHostedService<OutboxMessagePublisherBackgroundService>();
        
        return services;
    }
}