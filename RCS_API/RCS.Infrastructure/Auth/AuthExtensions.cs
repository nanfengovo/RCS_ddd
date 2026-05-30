using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using RCS.Core.Common;

namespace RCS.Infrastructure.Auth;

internal static class AuthExtensions
{
    public static IServiceCollection AddRcsAuth(this IServiceCollection services, IConfiguration configuration)
    {
        // 1. 注册全局用户上下文
        services.AddHttpContextAccessor();
        services.AddScoped<ICurrentUser, CurrentUser>();

        // 2. 配置 JWT 鉴权
        var jwtSecret = configuration["JwtSettings:Secret"] ?? "YourSuperSecretKeyForRcsSystem_2026!";
        var key = Encoding.ASCII.GetBytes(jwtSecret);

        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false, // 生产环境建议开启
                    ValidateAudience = false,
                    ClockSkew = TimeSpan.Zero
                };
            });

        // 3. 配置 RBAC 策略 (示例：定义一个要求必须是 WmsAdmin 角色的策略)
        services.AddAuthorizationBuilder()
            .AddPolicy("RequireWmsAdmin", policy => policy.RequireRole("WmsAdmin", "SystemAdmin"));

        return services;
    }
}