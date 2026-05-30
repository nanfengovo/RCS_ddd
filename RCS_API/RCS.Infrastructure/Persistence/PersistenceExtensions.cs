using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RCS.Core.Modules.Wms.Repositories; // 你的核心层接口
using RCS.Infrastructure.Modules.Wms.Repositories;
using RCS.Infrastructure.Persistence.EntityFramework; // 你的基础设施层实现类

namespace RCS.Infrastructure.Persistence;

internal static class PersistenceExtensions
{
    public static IServiceCollection AddRcsPersistence(this IServiceCollection services, IConfiguration configuration)
    {
        // 1. 注册 DbContext
        services.AddDbContext<RcsDbContext>(options =>
            options.UseNpgsql(
                configuration.GetConnectionString("RcsCoreDb"),
                npgsqlOptions => npgsqlOptions.EnableRetryOnFailure()));

        // 2. 注册仓储 (Repository)
        services.AddScoped<ILocationRepository, LocationRepository>();

        return services;
    }
}