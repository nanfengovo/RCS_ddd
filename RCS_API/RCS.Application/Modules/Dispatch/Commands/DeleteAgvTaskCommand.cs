using MediatR;
using RCS.Core.Modules.Dispatch.Interfaces;

namespace RCS.Core.Modules.Dispatch.Commands;

public record DeleteAgvTaskCommand(string TaskId) : IRequest<bool>;

public class DeleteAgvTaskCommandHandler : IRequestHandler<DeleteAgvTaskCommand, bool>
{
    private readonly IWcsAdapter _wcsAdapter;

    public DeleteAgvTaskCommandHandler(IWcsAdapter wcsAdapter)
    {
        _wcsAdapter = wcsAdapter;
    }

    public async Task<bool> Handle(DeleteAgvTaskCommand request, CancellationToken cancellationToken)
    {
        // 1. 业务校验：检查数据库里这个任务当前的状态，如果已经“执行中”，可能不允许删除。
        
        // 2. 通知下层设备取消任务
        await _wcsAdapter.DeleteTaskAsync(request.TaskId);
        
        // 3. 将本地数据库中的任务状态更新为 "已取消"
        
        return true;
    }
}