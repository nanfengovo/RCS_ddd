using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using RCS.Core.Common;
using RCS.Core.Common.Security; // 引入刚才写的标签

namespace RCS.Infrastructure.Auth;

/// <summary>
/// 极客架构：全局权限拦截器 (AOP)
/// </summary>
internal class PermissionAuthorizationFilter : IAsyncAuthorizationFilter
{
    private readonly ICurrentUser _currentUser;

    public PermissionAuthorizationFilter(ICurrentUser currentUser)
    {
        _currentUser = currentUser;
    }

    public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
    {
        // 1. 如果接口允许匿名访问 [AllowAnonymous]，直接放行
        if (context.ActionDescriptor.EndpointMetadata.Any(em => em is IAllowAnonymous))
            return;

        // 2. 扫描接口上是否贴了我们自定义的 [RequirePermission] 标签
        var requiredPermissions = context.ActionDescriptor.EndpointMetadata
            .OfType<RequirePermissionAttribute>()
            .ToList();

        if (!requiredPermissions.Any())
            return; // 没贴标签，说明不需要细粒度权限，直接放行给下一步的 JWT 校验

        // 3. 校验登录状态
        if (!_currentUser.IsAuthenticated)
        {
            // 返回标准的 ApiResponse 格式
            var unAuthResponse = ApiResponse.Fail("登录已过期，请重新登录。", 401);
            context.Result = new ObjectResult(unAuthResponse) { StatusCode = 200 }; // 企业级规范：HTTP状态码200，业务码401
            return;
        }

        // 4. 核心逻辑：校验细粒度权限
        foreach (var attr in requiredPermissions)
        {
            var hasPerm = await _currentUser.HasPermissionAsync(attr.Code);
            if (!hasPerm)
            {
                // 一旦发现缺少某个权限，立刻拦截并返回 403
                var forbiddenResponse = ApiResponse.Fail($"操作越权！缺少必须的权限：{attr.Code}", 403);
                context.Result = new ObjectResult(forbiddenResponse) { StatusCode = 200 };
                return;
            }
        }
    }
}