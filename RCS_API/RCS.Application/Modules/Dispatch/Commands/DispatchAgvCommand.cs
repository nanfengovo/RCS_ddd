using MediatR;
using RCS.Core.Modules.Dispatch.Interfaces;

namespace RCS.Core.Modules.Dispatch.Commands;

// 1. 定义命令契约 (前端传过来的参数)
public record DispatchAgvCommand(string LocationCode, string TaskId, string TaskType) : IRequest<bool>;

// 2. 编写核心业务处理器
public class DispatchAgvCommandHandler : IRequestHandler<DispatchAgvCommand, bool>
{
    // 🚀 核心：我们只注入抽象的 IWcsAdapter，根本不关心底层是新松还是海康！
    private readonly IWcsAdapter _wcsAdapter;
    
    // 如果有数据库操作，可以在这里注入 ILocationRepository 等等
    // private readonly ILocationRepository _locationRepository;

    public DispatchAgvCommandHandler(IWcsAdapter wcsAdapter)
    {
        _wcsAdapter = wcsAdapter;
    }

    public async Task<bool> Handle(DispatchAgvCommand request, CancellationToken cancellationToken)
    {
        // -----------------------------------------------------------
        // 💡 极客提示：在这里写你的纯粹业务逻辑
        // 1. 查数据库，看看这个库位是不是已经被锁定了？
        // 2. 查数据库，看看库存够不够？
        // 3. 在数据库里生成一条 WMS 内部的 "任务执行记录" (状态：下发中)
        // -----------------------------------------------------------

        // 🚀 最终步骤：呼叫外部 AGV！
        // 如果底层网络断了或者新松报错，IWcsAdapter 会抛出 DomainException，
        // 我们配置的全局异常处理器 (GlobalExceptionHandler) 会自动拦截并返回给前端 400 错误。
        await _wcsAdapter.DispatchAgvTaskAsync(request.LocationCode, request.TaskId, request.TaskType);

        // 如果没有抛异常，说明成功发给新松了
        return true; 
    }
}