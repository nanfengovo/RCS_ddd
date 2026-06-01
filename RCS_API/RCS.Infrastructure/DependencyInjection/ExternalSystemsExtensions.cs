using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RCS.Core.Modules.Dispatch.Interfaces;
using RCS.Infrastructure.ExternalSystems.XinSong;
using RCS.Infrastructure.Logging.Http;
using RCS.Infrastructure.Modules.Dispatch.ExternalSystems.XinSong.Models;
using Refit;

namespace RCS.Infrastructure; // 保持同一个命名空间，方便点出方法

/// <summary>
/// 极客基建：外部第三方系统注册扩展
/// </summary>
public static class ExternalSystemsExtensions
{
    public static IServiceCollection AddRcsExternalSystems(this IServiceCollection services, IConfiguration configuration)
    {
        // 1. 注册核心出站日志拦截器 (写入 MongoDB)
        services.AddTransient<OutboundLoggingHandler>();

        // 2. 注册新松 WCS 外部服务 (Refit 客户端)
        // 🚀 修复了 var string? 的语法错误
        string? xinSongUrl = configuration["WcsConfig:XinSongUrl"] ?? "http://127.0.0.1:9999";
        
        services.AddRefitClient<IXinSongWcsApi>()
            .ConfigureHttpClient(c => c.BaseAddress = new Uri(xinSongUrl))
            .AddHttpMessageHandler<OutboundLoggingHandler>(); // 挂载全局出站日志拦截器

        // 3. 注册防腐层适配器供领域层调用
        services.AddScoped<IWcsAdapter, XinSongWcsAdapter>();

        // 💡 如果未来接入海康、极智嘉等，统统在这里排队注册...

        return services;
    }
}