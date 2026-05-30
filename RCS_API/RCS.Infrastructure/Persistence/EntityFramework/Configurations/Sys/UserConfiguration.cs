using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RCS.Core.Modules.Sys.Entities;

namespace RCS.Infrastructure.Persistence.EntityFramework.Configurations.Sys
{
    // 1. 用户表配置
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("sys_users"); // 极客规范：系统表加 sys_ 前缀
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Username).IsRequired().HasMaxLength(50);
            builder.HasIndex(x => x.Username).IsUnique(); // 账号必须唯一
        }
    }

    // 2. 角色表配置
    public class RoleConfiguration : IEntityTypeConfiguration<Role>
    {
        public void Configure(EntityTypeBuilder<Role> builder)
        {
            builder.ToTable("sys_roles");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Name).IsRequired().HasMaxLength(50);
            builder.HasIndex(x => x.Name).IsUnique();
        }
    }

    // 3. 权限表配置
    public class PermissionConfiguration : IEntityTypeConfiguration<Permission>
    {
        public void Configure(EntityTypeBuilder<Permission> builder)
        {
            builder.ToTable("sys_permissions");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Code).IsRequired().HasMaxLength(100);
            builder.HasIndex(x => x.Code).IsUnique(); // 权限码绝对不能重复
            builder.Property(x => x.Type).HasMaxLength(20);
        }
    }

    // 4. 用户-角色关联配置 (复合主键)
    public class UserRoleConfiguration : IEntityTypeConfiguration<UserRole>
    {
        public void Configure(EntityTypeBuilder<UserRole> builder)
        {
            builder.ToTable("sys_user_roles");
            builder.HasKey(ur => new { ur.UserId, ur.RoleId }); // 联合主键

            builder.HasOne(ur => ur.User).WithMany(u => u.UserRoles).HasForeignKey(ur => ur.UserId);
            builder.HasOne(ur => ur.Role).WithMany(r => r.UserRoles).HasForeignKey(ur => ur.RoleId);
        }
    }

    // 5. 角色-权限关联配置 (复合主键)
    public class RolePermissionConfiguration : IEntityTypeConfiguration<RolePermission>
    {
        public void Configure(EntityTypeBuilder<RolePermission> builder)
        {
            builder.ToTable("sys_role_permissions");
            builder.HasKey(rp => new { rp.RoleId, rp.PermissionId }); // 联合主键

            builder.HasOne(rp => rp.Role).WithMany(r => r.RolePermissions).HasForeignKey(rp => rp.RoleId);
            builder.HasOne(rp => rp.Permission).WithMany(p => p.RolePermissions).HasForeignKey(rp => rp.PermissionId);
        }
    }
}