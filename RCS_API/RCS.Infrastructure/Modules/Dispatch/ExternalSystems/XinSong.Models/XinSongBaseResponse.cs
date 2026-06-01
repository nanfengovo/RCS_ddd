using System.Text.Json.Serialization;

namespace RCS.Infrastructure.Modules.Dispatch.ExternalSystems.XinSong.Models
{
    /// <summary>
    /// 新松基础响应实体
    /// </summary>
    public class XinSongBaseResponse
    {
        [JsonPropertyName("Result")]
        public bool Result { get; set; } // 请求的结果，成功为true [cite: 73]

        [JsonPropertyName("ErrMsg")]
        public string ErrMsg { get; set; } = string.Empty; // 错误的信息提示 [cite: 73]
    }

    /// <summary>
    /// 2. 下发任务请求体 [cite: 20]
    /// </summary>
    public class XinSongTaskAddRequest
    {
        [JsonPropertyName("bulk_task_count")]
        public int BulkTaskCount { get; set; } // 组合任务子任务数量 [cite: 65]

        [JsonPropertyName("bulk_task_type")]
        public string BulkTaskType { get; set; } = string.Empty; // 组合任务类型名 [cite: 65]

        [JsonPropertyName("sub_task")]
        public List<XinSongSubTask> SubTasks { get; set; } = new(); // 子任务数组 [cite: 65]
    }

    public class XinSongSubTask
    {
        [JsonPropertyName("AGV_serial")]
        public int AgvSerial { get; set; } // 0为不指定 [cite: 65]
        
        [JsonPropertyName("robot_type")]
        public string RobotType { get; set; } = "3"; // 车辆类型 [cite: 65]

        [JsonPropertyName("area_property")]
        public string[] AreaProperty { get; set; } = Array.Empty<string>(); // 区域属性 [cite: 65]

        [JsonPropertyName("cargo_id")]
        public string CargoId { get; set; } = string.Empty; // 货物 RFID [cite: 65]

        [JsonPropertyName("complete_time")]
        public string CompleteTime { get; set; } = string.Empty; // 最晚执行时间 YYYY-MM-DD HH:mm:SS [cite: 65]

        [JsonPropertyName("succession")]
        public int Succession { get; set; } // 接续号 [cite: 65]

        [JsonPropertyName("pre_report")]
        public string PreReport { get; set; } = "0"; // 集中上报标记 [cite: 65]

        [JsonPropertyName("priority")]
        public int Priority { get; set; } = 1; // 优先级 0-99 [cite: 65]

        [JsonPropertyName("goal_action")]
        public int GoalAction { get; set; } // 1-取物料, 2-放物料, 0-普通 [cite: 65]

        [JsonPropertyName("mark")]
        public string Mark { get; set; } = "0"; // 备用 [cite: 65]

        [JsonPropertyName("option_code")]
        public string OptionCode { get; set; } = "0,0"; // 设备操作码 [cite: 65]

        [JsonPropertyName("target")]
        public int Target { get; set; } // 地图点 [cite: 66]

        [JsonPropertyName("storage")]
        public string Storage { get; set; } = string.Empty; // 备用 [cite: 66]

        [JsonPropertyName("task_serial")]
        public string TaskSerial { get; set; } = string.Empty; // 任务名称 [cite: 66]

        [JsonPropertyName("task_type")]
        public string TaskType { get; set; } = string.Empty; // 任务类型 [cite: 66]
    }

    /// <summary>
    /// 8. 删除任务请求体 [cite: 176]
    /// </summary>
    public class XinSongTaskDeleteRequest
    {
        [JsonPropertyName("delete_task_count")]
        public int DeleteTaskCount { get; set; } = 1; // 任务数量 [cite: 182]

        [JsonPropertyName("task_serial")]
        public string TaskSerial { get; set; } = string.Empty; // 任务名称 [cite: 182]
        
        [JsonPropertyName("AGV_serial")]
        public int AgvSerial { get; set; } = 0; // 执行任务车号 [cite: 182]

        [JsonPropertyName("sub_tasks")]
        public List<string> SubTasks { get; set; } = new(); // 子任务数组 [cite: 182]
    }
}