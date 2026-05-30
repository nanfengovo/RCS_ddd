
using RCS.Core.Modules.Wms.Enums;

namespace RCS.Core.Modules.Wms.Entities
{   
    /// <summary>
    /// 库位聚合根
    /// </summary>
    public class Location
    {
        public Guid Id {get ; private set;}

        /// <summary>
        /// 库位编码，唯一标识一个库位，格式可以是类似于 "A-01-01" 的字符串，表示区-排-位
        /// </summary>
        public string Code {get; private set;} = default!;

        /// <summary>
        /// 库位状态，表示当前库位的使用情况，如空闲、被锁定、占用或禁用等
        /// </summary>
        public LocationStatus Status {get; private set;}

        /// <summary>
        /// 当前任务ID，表示当前库位上关联的任务
        /// </summary>
        public Guid? CurrentTaskId {get; private set;}

        //EF Core 需要一个无参构造函数，访问修饰符可以是 protected 或 private，防止外部直接创建实例
        protected Location(){}

        //业务创建入口，强制要求提供必要的参数，并设置默认状态
        public Location(Guid id, string code)
        {
            Id = id;
            Code = code;
            Status = LocationStatus.Idle; // 默认状态为 Idle
        }

        //业务方法：锁定库位
        public void LockForTask(Guid taskId)
        {
            if(Status != LocationStatus.Idle)
            {
                throw new InvalidOperationException($"严重业务异常：库位 {Code} 当前状态为 {Status}，无法被任务 {taskId} 锁定！");  
            }
            Status = LocationStatus.Locked;
            CurrentTaskId = taskId;
        }
    }
}