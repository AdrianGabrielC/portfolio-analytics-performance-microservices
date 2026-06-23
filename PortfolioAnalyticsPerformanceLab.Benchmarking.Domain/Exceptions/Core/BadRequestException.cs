namespace PortfolioAnalyticsPerformanceLab.Benchmarking.Domain.Exceptions.Core;

public abstract class BadRequestException : Exception
{
    protected BadRequestException(string message) : base(message) { }
}