using RCS.Core.Modules.Wms.Entities;

namespace RCS.Core.Modules.Wms.Repositories
{
    /// <summary>
    /// 库位仓储接口 (只定义规范，绝对不写实现)
    /// </summary>
    public interface ILocationRepository
    {
        // 根据库位编码查询 (有可能查不到，所以允许返回 null)
        Task<Location?> GetByCodeAsync(string code);

        // 更新库位状态
        Task UpdateAsync(Location location);
    }
}