namespace RCS.Core.Common;

public interface ICurrentUser
{
    bool IsAuthenticated { get; }
    string? UserId { get; }
    string? UserName { get; }
    
    /// <summary>
    /// 获取当前用户拥有的所有细粒度权限 (菜单、按钮、API)
    /// </summary>
    Task<string[]> GetPermissionsAsync();
    
    /// <summary>
    /// 判断当前用户是否拥有指定权限
    /// </summary>
    Task<bool> HasPermissionAsync(string permissionCode);
}