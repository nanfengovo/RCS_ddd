using MediatR;
using Microsoft.Extensions.DependencyInjection;
using RCS.Core.Common.Auditing;
using RCS.Infrastructure.Logging.Mongo;

namespace RCS.Infrastructure.Logging;

internal static class LoggingExtensions
{
    public static IServiceCollection AddRcsLogging(this IServiceCollection services)
    {
        // 注册 MediatR 管道行为
        services.AddScoped(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));

        // 🚀 1. 注册内存队列 (必须是单例，全局唯一)
        services.AddSingleton<AuditLogQueue>();
        
        // 🚀 2. 注册业务层使用的 Logger 接口
        services.AddScoped<IAuditLogger, QueuedAuditLogger>();
        
        // 🚀 3. 注册后台持续运行的 Worker (消费者)
        services.AddHostedService<MongoAuditBackgroundWorker>();
        return services;
    }
}