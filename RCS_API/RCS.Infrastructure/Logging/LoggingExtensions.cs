using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace RCS.Infrastructure.Logging;

internal static class LoggingExtensions
{
    public static IServiceCollection AddRcsLogging(this IServiceCollection services)
    {
        // 注册 MediatR 管道行为
        services.AddScoped(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));
        return services;
    }
}