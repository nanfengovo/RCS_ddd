using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RCS.Core.Modules.Wms.Entities;

namespace RCS.Infrastructure.Persistence.EntityFramework.Configurations.Wms
{
    /// <summary>
    /// 库位实体的数据库映射规则
    /// </summary>
    public class LocationConfiguration : IEntityTypeConfiguration<Location>
    {
        public void Configure(EntityTypeBuilder<Location> builder)
        {
            // 1. 指定真实的数据库表名
            builder.ToTable("wms_locations");

            // 2. 设定主键
            builder.HasKey(x => x.Id);

            // 3. 配置字段细节：必填、最大长度 50
            builder.Property(x => x.Code)
                .IsRequired()
                .HasMaxLength(50);

            // 4. 业务规则映射到数据库约束：库位编码必须全局唯一
            builder.HasIndex(x => x.Code).IsUnique(); 
        }
    }
}