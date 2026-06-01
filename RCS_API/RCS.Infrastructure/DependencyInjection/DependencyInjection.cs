using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RCS.Infrastructure.Auth;
using RCS.Infrastructure.Exceptions;
using RCS.Infrastructure.Logging;
using RCS.Infrastructure.Observability; // 👈 引入可观测性
using RCS.Infrastructure.Persistence;   // 👈 引入持久化

namespace RCS.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddRcsOpenTelemetry()   // 🚀 1. 挂载雷达 (OTel)
                .AddRcsLogging()         // 📝 2. 挂载日志拦截器
                .AddRcsExceptionHandling() // 🛡️ 3. 挂载异常护城河
                .AddRcsPersistence(configuration) // 💾 4. 挂载数据库与仓储
                .AddRcsAuth(configuration)           // 🔐 5. 挂载认证授权
                .AddRcsExternalSystems(configuration); // 🛰️ 6. 挂载第三方外部系统防腐层

        return services;
    }
}