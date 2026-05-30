using Microsoft.Extensions.DependencyInjection;

namespace RCS.Infrastructure.Exceptions;

internal static class ExceptionHandlingExtensions
{
    public static IServiceCollection AddRcsExceptionHandling(this IServiceCollection services)
    {
        services.AddExceptionHandler<GlobalExceptionHandler>();
        services.AddProblemDetails();
        return services;
    }
}