
using RCS.Core.Modules.Sys.Entities;

namespace RCS.Core.Modules.Auth.Repositories;

/// <summary>
/// 用户聚合根仓储接口
/// </summary>
public interface IUserRepository
{
    /// <summary>
    /// 根据用户名查询用户
    /// </summary>
    Task<User?> GetByUsernameAsync(string username);

    /// <summary>
    /// 获取用户拥有的所有细粒度权限码 (连表查询的脏活交给基础设施层去干)
    /// </summary>
    Task<string[]> GetUserPermissionsAsync(Guid userId);
}