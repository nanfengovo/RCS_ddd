using Microsoft.EntityFrameworkCore;
using RCS.Core.Modules.Auth.Repositories;
using RCS.Core.Modules.Sys.Entities;
using RCS.Infrastructure.Persistence.EntityFramework;

namespace RCS.Infrastructure.Modules.Auth.Repositories;

internal class UserRepository : IUserRepository
{
    private readonly RcsDbContext _dbContext;

    public UserRepository(RcsDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<User?> GetByUsernameAsync(string username)
    {
        return await _dbContext.Users
            .FirstOrDefaultAsync(u => u.Username == username);
    }

    public async Task<string[]> GetUserPermissionsAsync(Guid userId)
    {
        // 复杂的连表 SQL 逻辑全部封锁在这里！
        return await _dbContext.UserRoles
            .Where(ur => ur.UserId == userId)
            .Join(_dbContext.RolePermissions, ur => ur.RoleId, rp => rp.RoleId, (ur, rp) => rp.PermissionId)
            .Join(_dbContext.Permissions, pid => pid, p => p.Id, (pid, p) => p.Code)
            .Distinct()
            .ToArrayAsync();
    }
}