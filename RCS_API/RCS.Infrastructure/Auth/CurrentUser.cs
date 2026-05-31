using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using RCS.Core.Common;

namespace RCS.Infrastructure.Auth;

/// <summary>
/// 从 HTTP Token 中提取用户信息的极客实现
/// </summary>
internal class CurrentUser : ICurrentUser
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CurrentUser(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    private ClaimsPrincipal? User => _httpContextAccessor.HttpContext?.User;

    public bool IsAuthenticated => User?.Identity?.IsAuthenticated ?? false;

    // 假设你的 JWT 里存 UserId 用的 ClaimType 是 NameIdentifier
    public string? UserId => User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;

    public string? UserName => User?.FindFirst(ClaimTypes.Name)?.Value;

    public string[] Roles => User?.FindAll(ClaimTypes.Role).Select(c => c.Value).ToArray() ?? Array.Empty<string>();

    public Task<string[]> GetPermissionsAsync()
    {
        // 极客预留：未来这里换成 await _redis.GetAsync($"UserPermissions:{UserId}");
        return Task.FromResult(new[] { "wms:location:lock", "sys:user:view" });
    }

    public async Task<bool> HasPermissionAsync(string permissionCode)
    {
        // 1. 获取当前用户的所有权限 (走缓存)
        var permissions = await GetPermissionsAsync();
        
        // 2. 判断是否包含需要的权限 (如果是超管，可以直接 return true)
        return permissions.Contains(permissionCode);
    }
}