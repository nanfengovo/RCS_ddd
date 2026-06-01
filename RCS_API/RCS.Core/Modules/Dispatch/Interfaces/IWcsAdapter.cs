using System.Linq;
using System.Threading.Tasks;

namespace RCS.Core.Modules.Dispatch.Interfaces
{
    /// <summary>
    /// 标准 WCS 调度中心适配器 (防腐层业务接口)
    /// </summary>
    public interface IWcsAdapter
    {
        /// <summary>
        /// 呼叫 AGV 前往指定地点执行任务
        /// </summary>
        /// <param name="locationCode">目标地图点</param>
        /// <param name="taskId">WMS 内部任务流水号</param>
        /// <param name="taskType">抽象任务类型 (如: GET_CARGO, PUT_CARGO)</param>
        Task DispatchAgvTaskAsync(string locationCode, string taskId, string taskType);

        /// <summary>
        /// 取消/删除已下发的调度任务
        /// </summary>
        Task DeleteTaskAsync(string taskId);
    }
}