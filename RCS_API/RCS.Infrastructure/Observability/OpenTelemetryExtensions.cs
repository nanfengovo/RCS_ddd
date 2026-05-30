using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Npgsql;
using OpenTelemetry;
using OpenTelemetry.Metrics;
using OpenTelemetry.Trace;

namespace RCS.Infrastructure.Observability;

internal static class OpenTelemetryExtensions
{
    public static IServiceCollection AddRcsOpenTelemetry(this IServiceCollection services)
    {
        // 1. 配置日志
        services.AddLogging(logging =>
        {
            logging.AddOpenTelemetry(options =>
            {
                options.IncludeFormattedMessage = true;
                options.IncludeScopes = true;
            });
        });

        // 2. 配置链路追踪与指标
        services.AddOpenTelemetry()
            .WithMetrics(metrics =>
            {
                metrics.AddAspNetCoreInstrumentation(); // API 吞吐量
            })
            .WithTracing(tracing =>
            {
                tracing.AddAspNetCoreInstrumentation(); // HTTP 追踪
                tracing.AddNpgsql();                    // 数据库调用追踪
            })
            .UseOtlpExporter(); // 发射给 Aspire

        return services;
    }
}