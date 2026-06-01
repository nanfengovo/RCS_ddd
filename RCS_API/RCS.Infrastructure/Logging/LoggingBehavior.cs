using System.Diagnostics;
using System.Text.Json;
using MediatR;
using Microsoft.Extensions.Logging;
using RCS.Core.Common;
using RCS.Core.Common.Auditing; // 👈 引入审计日志模型和接口
using RCS.Core.Exceptions;      // 👈 引入领域异常基类

namespace RCS.Infrastructure.Logging;

/// <summary>
/// 极客架构：全局请求拦截器 (记录控制台日志 + 异步发送 MongoDB 审计日志)
/// </summary>
public class LoggingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : notnull
{
    private readonly ILogger<LoggingBehavior<TRequest, TResponse>> _logger;
    private readonly ICurrentUser _currentUser;
    private readonly IAuditLogger _auditLogger; // 🚀 注入异步发件人

    public LoggingBehavior(
        ILogger<LoggingBehavior<TRequest, TResponse>> logger, 
        ICurrentUser currentUser,
        IAuditLogger auditLogger)
    {
        _logger = logger;
        _currentUser = currentUser;
        _auditLogger = auditLogger;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        var requestName = typeof(TRequest).Name;
        var operatorId = _currentUser.UserId ?? "Anonymous"; 
        
        // ⏱️ 1. 启动极其精确的秒表 (用于监控接口性能)
        var stopwatch = Stopwatch.StartNew();
        
        // 📝 2. 预先组装审计日志的基础信息
        var auditLog = new AuditLog
        {
            UserId = operatorId,
            UserName = _currentUser.UserName ?? "Anonymous",
            ActionName = requestName,
            // 将入参序列化存入 Mongo (排查线上问题的究极神器)
            Parameters = JsonSerializer.Serialize(request) 
        };

        // 🚀 开启日志作用域：这里的控制台日志会自动带上 UserId
        using (_logger.BeginScope(new Dictionary<string, object> { ["UserId"] = operatorId }))
        {
            _logger.LogInformation("➡️ [开始执行] {RequestName} | 操作人: {UserId}", requestName, operatorId);
            
            try
            {
                // 👉 放行请求，去执行真正的业务 (Handler)
                var response = await next();
                
                _logger.LogInformation("✅ [执行成功] {RequestName}", requestName);
                auditLog.IsSuccess = true;
                
                return response;
            }
            catch (Exception ex)
            {
                // 业务执行失败，记录死因
                auditLog.IsSuccess = false;
                auditLog.ErrorMessage = ex.Message;

                // 🛡️ 优雅的类型判断：准确识别业务拦截 vs 系统崩溃
                if (ex is DomainException domainEx)
                {
                    _logger.LogWarning("⚠️ [业务拦截] {RequestName} | 原因: {Message}", requestName, domainEx.Message);
                }
                else
                {
                    _logger.LogError(ex, "❌ [执行失败] {RequestName} | 发生系统级异常！", requestName);
                }
                throw; // 必须继续向上抛出给 GlobalExceptionHandler，确保前端收到 400/500
            }
            finally
            {
                // 🛑 无论成功失败，都必须执行这里的收尾工作！
                stopwatch.Stop();
                auditLog.ExecutionDuration = stopwatch.ElapsedMilliseconds;

                // 🚀 发射！扔进内存队列 (Fire and Forget)
                // 极客细节：这里故意不用 await，使用弃元符 `_`，意味着主线程不等待它写完，直接把响应返回给前端！
                _ = _auditLogger.LogAsync(auditLog); 
            }
        }
    }
}