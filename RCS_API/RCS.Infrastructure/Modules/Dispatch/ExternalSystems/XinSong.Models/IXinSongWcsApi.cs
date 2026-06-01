
using RCS.Infrastructure.Modules.Dispatch.ExternalSystems.XinSong.Models;
using Refit;

namespace RCS.Infrastructure.ExternalSystems.XinSong;

/// <summary>
/// 新松调度系统 HTTP API (Refit 自动生成底层代码)
/// </summary>
public interface IXinSongWcsApi
{
    // 1. 心跳
    [Get("/api/xinsong/heart_beat")]
    Task<XinSongBaseResponse> HeartBeatAsync([Query] string timestamp); 

    // 2. 下发任务 
    [Post("/api/v1/xinsong/task_add")]
    Task<XinSongBaseResponse> TaskAddAsync([Body] XinSongTaskAddRequest request); 

    // 8. RCS 端操作删除任务 
    [Post("/api/v1/xinsong/task_delete")]
    Task<XinSongBaseResponse> TaskDeleteAsync([Body] XinSongTaskDeleteRequest request); 
}