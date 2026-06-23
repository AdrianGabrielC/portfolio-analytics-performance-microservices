namespace PortfolioAnalyticsPerformanceLab.Benchmarking.Api.Extensions;

public static class ServiceExtensions
{
    public static void ConfigureCors(this IServiceCollection services) => services.AddCors(options =>
    {
        options.AddPolicy("CorsPolicy", builder =>
            builder
                .AllowAnyOrigin()
                .AllowAnyHeader()
                .AllowAnyMethod());
    });

    public static void ConfigureAutomapper(this IServiceCollection services) => services.AddAutoMapper(cfg =>
    {
        cfg.LicenseKey = "<Your License Key>"; // required in automapper 15
    }, typeof(Program).Assembly);
    
}