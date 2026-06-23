namespace PortfolioAnalyticsPerformanceLab.Benchmarking.Domain.Exceptions.Core;

public abstract class NotFoundException: Exception
{
    protected NotFoundException(string message): base(message) { }
}