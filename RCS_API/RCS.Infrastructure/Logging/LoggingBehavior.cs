using MediatR;
using Microsoft.Extensions.Logging;
using RCS.Core.Common; // 引入 ICurrentUser

namespace RCS.Infrastructure.Logging;

public class LoggingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : notnull
{
    private readonly ILogger<LoggingBehavior<TRequest, TResponse>> _logger;
    private readonly ICurrentUser _currentUser; // 👈 注入全局用户上下文

    public LoggingBehavior(ILogger<LoggingBehavior<TRequest, TResponse>> logger, ICurrentUser currentUser)
    {
        _logger = logger;
        _currentUser = currentUser;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        var requestName = typeof(TRequest).Name;
        // 🚀 极客魔法：获取当前操作人，如果没有登录就是 Anonymous
        var operatorId = _currentUser.UserId ?? "Anonymous"; 

        // 🚀 开启日志作用域：在这个 using 块里面发生的所有日志，都会自动带上 UserId！
        using (_logger.BeginScope(new Dictionary<string, object> 
        { 
            ["UserId"] = operatorId 
        }))
        {
            _logger.LogInformation("➡️ [开始执行] {RequestName} | 操作人: {UserId}", requestName, operatorId);
            
            try
            {
                var response = await next();
                _logger.LogInformation("✅ [执行成功] {RequestName}", requestName);
                return response;
            }
            catch (Exception ex)
            {
                // 如果是业务异常，我们用 LogWarning（警告），因为它不是系统级崩溃
                if (ex.GetType().Name == "DomainException")
                {
                    _logger.LogWarning("⚠️ [业务拦截] {RequestName} | 原因: {Message}", requestName, ex.Message);
                }
                else
                {
                    _logger.LogError(ex, "❌ [执行失败] {RequestName} | 发生系统级异常！", requestName);
                }
                throw; // 必须继续抛出，让外层的 GlobalExceptionHandler 捕捉！
            }
        }
    }
}