using PortfolioAnalyticsPerformanceLab.Benchmarking.Api.Extensions;
using PortfolioAnalyticsPerformanceLab.Benchmarking.Application.Util;
using PortfolioAnalyticsPerformanceLab.Benchmarking.Infrastructure.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.ConfigureCors();
builder.Services.AddControllers();
builder.Services.AddSwaggerGen();
builder.Services.ConfigureAutomapper();
builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssemblyContaining<ApplicationAssemblyMarker>();
});
builder.Services.AddInfrastructure(builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.ConfigureExceptionHandler();
app.UseHttpsRedirection();
app.MapControllers();
app.UseCors("CorsPolicy");

app.Run();