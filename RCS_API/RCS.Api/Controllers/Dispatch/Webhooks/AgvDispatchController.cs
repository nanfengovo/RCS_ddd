using MediatR;
using Microsoft.AspNetCore.Mvc;
using RCS.Core.Modules.Dispatch.Commands;

namespace RCS.Api.Controllers;

/// <summary>
/// AGV 调度控制台 API
/// </summary>
[ApiController]
[Route("api/v1/wms/dispatch")]
public class AgvDispatchController : ControllerBase
{
    private readonly IMediator _mediator;

    public AgvDispatchController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// 派发 AGV 搬运任务
    /// </summary>
    [HttpPost("tasks")]
    public async Task<IActionResult> DispatchTask([FromBody] DispatchAgvCommand command)
    {
        // 🚀 把前端发来的命令直接扔进中枢神经 (MediatR) 处理
        await _mediator.Send(command);

        // 如果走到这里没有抛出异常，说明业务校验通过，且成功发给了第三方系统
        return Ok(new { message = "AGV 调度指令已成功发出！" });
    }

    /// <summary>
    /// 删除/取消指定的 AGV 任务
    /// </summary>
    [HttpDelete("tasks/{taskId}")]
    public async Task<IActionResult> DeleteTask([FromRoute] string taskId)
    {
        var command = new DeleteAgvTaskCommand(taskId);
        await _mediator.Send(command);

        return Ok(new { message = $"任务 {taskId} 已成功取消！" });
    }
}