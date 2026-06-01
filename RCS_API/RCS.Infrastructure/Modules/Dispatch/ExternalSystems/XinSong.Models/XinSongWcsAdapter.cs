using RCS.Core.Exceptions;
using RCS.Core.Modules.Dispatch.Interfaces;
using RCS.Infrastructure.ExternalSystems.XinSong;

namespace RCS.Infrastructure.Modules.Dispatch.ExternalSystems.XinSong.Models
{
    public class XinSongWcsAdapter : IWcsAdapter
    {
        private readonly IXinSongWcsApi _api;

        public XinSongWcsAdapter(IXinSongWcsApi api)
        {
            _api = api;
        }

        public async Task DispatchAgvTaskAsync(string locationCode, string taskId, string taskType)
        {
            // 核心：翻译内部业务领域为新松特定的 API 模型
            var request = new XinSongTaskAddRequest
            {
                BulkTaskCount = 1, 
                BulkTaskType = "task", 
                SubTasks = new List<XinSongSubTask>
                {
                    new XinSongSubTask
                    {
                        AgvSerial = 0, // 0为不指定 
                        TaskSerial = taskId,
                        Target = int.Parse(locationCode), // 假设库位Code可以转为地图点ID
                        TaskType = taskType == "GET_CARGO" ? "PA_14_get" : "PA_14_put", // 任务类型映射 
                        GoalAction = taskType == "GET_CARGO" ? 1 : 2, // 1-取, 2-放
                        Priority = 50, // 默认中等优先级 
                        CompleteTime = DateTime.Now.AddHours(1).ToString("yyyy-MM-dd HH:mm:ss") 
                    }
                }
            };

            // 发起 HTTP 请求 (出站日志和序列化全部由 Refit 和拦截器接管)
            var response = await _api.TaskAddAsync(request);

            // 防腐验证：如果返回 false，直接向上抛出标准领域异常 
            if (!response.Result)
            {
                throw new DomainException($"呼叫新松 AGV 失败: {response.ErrMsg}");
            }
        }

        public async Task DeleteTaskAsync(string taskId)
        {
            var request = new XinSongTaskDeleteRequest
            {
                TaskSerial = taskId, 
                DeleteTaskCount = 1, 
                AgvSerial = 0 
            };

            var response = await _api.TaskDeleteAsync(request);

            if (!response.Result)
            {
                throw new DomainException($"取消新松任务失败: {response.ErrMsg}");
            }
        }
    }
}