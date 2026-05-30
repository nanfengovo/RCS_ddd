using Microsoft.EntityFrameworkCore;
using RCS.Core.Modules.Sys.Entities;
using RCS.Core.Modules.Wms.Entities;

namespace RCS.Infrastructure.Persistence.EntityFramework
{
    public class RcsDbContext : DbContext
    {
        //构造函数：接收由API层注入的连接字符串配置
        public RcsDbContext(DbContextOptions<RcsDbContext> options):base(options)
        {
        }

        //定义 DbSet 属性，表示数据库中的表，这里以 Location 表为例
        public DbSet<Location> Locations{get; set;}

            // 🚀 新增的鉴权底座五大名表
        public DbSet<User> Users => Set<User>();
        public DbSet<Role> Roles => Set<Role>();
        public DbSet<Permission> Permissions => Set<Permission>();
        public DbSet<UserRole> UserRoles => Set<UserRole>();
        public DbSet<RolePermission> RolePermissions => Set<RolePermission>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //配置 Location 实体的映射关系
            modelBuilder.Entity<Location>(entity =>
            {
                base.OnModelCreating(modelBuilder);
                // 这样你的 DbContext 就不会变成几千行的“垃圾场”
                modelBuilder.ApplyConfigurationsFromAssembly(typeof(RcsDbContext).Assembly);
            });
        }
    }
}