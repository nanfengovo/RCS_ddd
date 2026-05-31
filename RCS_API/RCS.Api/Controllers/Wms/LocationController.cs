using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RCS.Application.Modules.Wms.Commands;
using RCS.Core.Common;
using RCS.Core.Common.Security;
using RCS.Core.Exceptions;
using RCS.Infrastructure.Persistence.EntityFramework;

namespace RCS.Api.Controllers.Wms;

[ApiController]
[Route("api/wms/locations")]
public class LocationController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly RcsDbContext _dbContext;
    private readonly ILogger<LocationController> _logger;

    public LocationController(IMediator mediator, RcsDbContext dbContext, ILogger<LocationController> logger)
    {
        _mediator = mediator;
        _dbContext = dbContext;
        _logger = logger;
    }

    // =================================================================
    // 🧪 探针 1：验证全局异常拦截器 (GlobalExceptionHandler)
    // 预期：返回标准的 ApiResponse JSON，且 HTTP 状态码为 200，内部 Code 为 400
    // =================================================================
    [HttpGet("test-exception")]
    public IActionResult TestException()
    {
        // 故意抛出一个领域异常
        throw new DomainException("这是一个用于测试的领域异常，护城河是否能拦截我？");
    }

    // =================================================================
    // 🧪 探针 2：验证 MediatR 管道日志 (LoggingBehavior)
    // 预期：控制台和 OpenTelemetry 中打印出结构化的入参和耗时日志
    // =================================================================
    [HttpPost("{locationCode}/lock")]
    [RequirePermission("wms:location:lock")] // 👈 就是这一行！极致优雅的 PBAC 鉴权！
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)] 
    public async Task<IActionResult> LockLocation(string locationCode, [FromBody] LockLocationRequest request)
    {
        // 如果没有权限，连这个方法的门都进不来！AOP 已经在外面把他踢回去了！
        var command = new LockLocationCommand(locationCode, request.TaskId);
        await _mediator.Send(command);

        return Ok(ApiResponse.Success("库位锁定命令已发送！"));
    }

    // =================================================================
    // 🧪 探针 3：验证 EF Core 数据库连接与建表 (Persistence)
    // 预期：成功往 Postgres 中插入一条数据，并能查出来
    // =================================================================
    [HttpGet("test-db")]
    public async Task<IActionResult> TestDatabase()
    {
        // 检查数据库连接
        var canConnect = await _dbContext.Database.CanConnectAsync();
        if (!canConnect) return Ok(ApiResponse.Fail("数据库连接失败！请检查连接字符串和密码。"));

        // 获取当前表里有多少条数据
        var count = await _dbContext.Locations.CountAsync();
        
        return Ok(ApiResponse.Success($"数据库连接极其完美！当前库位表中有 {count} 条数据。"));
    }

    // =================================================================
    // 🧪 探针 4：验证 OpenTelemetry 链路追踪
    // 预期：在 Aspire Dashboard 的 Traces 页面，能看到这个请求完整的调用链
    // =================================================================
    [HttpGet("test-telemetry")]
    public async Task<IActionResult> TestTelemetry()
    {
        // 手动打个日志，看看会不会被收集
        _logger.LogInformation("正在执行一个耗时的复杂计算，准备被 OpenTelemetry 抓取...");
        
        // 模拟一个数据库慢查询
        await Task.Delay(500); 
        var locations = await _dbContext.Locations.Take(10).ToListAsync();

        return Ok(ApiResponse.Success(locations, "链路追踪测试完成！快去 Aspire Dashboard 看看！"));
    }

        // 接收前端 JSON 报文的极简 DTO
        public class LockLocationRequest
        {
            public Guid TaskId { get; set; }
        }
}