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
        throw new NotImplementedException();
    }

    public Task<bool> HasPermissionAsync(string permissionCode)
    {
        throw new NotImplementedException();
    }
}