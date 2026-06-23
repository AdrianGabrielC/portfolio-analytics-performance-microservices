using System.Text.Json;
using Microsoft.AspNetCore.Diagnostics;
using PortfolioAnalyticsPerformanceLab.Benchmarking.Domain.Exceptions.Core;

namespace PortfolioAnalyticsPerformanceLab.Benchmarking.Api.Extensions;

public static class ExceptionMiddlewareExtensions
{
    //public static void ConfigureExceptionHandler(this IApplicationBuilder app)
    public static void ConfigureExceptionHandler(this WebApplication app)
    {
        app.UseExceptionHandler(appError =>
        {
            appError.Run(async context =>
            {
                context.Response.ContentType = "application/json";
                var contextFeature = context.Features.Get<IExceptionHandlerFeature>();
                if (contextFeature != null)
                {
                    context.Response.StatusCode = contextFeature.Error switch
                    {
                        NotFoundException => StatusCodes.Status404NotFound,
                        BadRequestException => StatusCodes.Status400BadRequest,
                        UnauthorizedAccessException => StatusCodes.Status401Unauthorized,
                        _ => StatusCodes.Status500InternalServerError
                    };

                    //logger.LogError($"Something went wrong: {contextFeature.Error}");

                    var errorResponse = new ErrorDetails
                    {
                        StatusCode = context.Response.StatusCode,
                        Message = context.Response.StatusCode == StatusCodes.Status500InternalServerError
                            ? "Internal Server Error."
                            : contextFeature.Error.Message
                    };

                    await context.Response.WriteAsync(JsonSerializer.Serialize(errorResponse));
                }
            });
        });
    }

}