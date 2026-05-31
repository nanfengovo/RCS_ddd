using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using RCS.Core.Common;

namespace RCS.Infrastructure.Exceptions;

/// <summary>
/// 全局异常护城河
/// </summary>
internal class GlobalExceptionHandler : IExceptionHandler
{
    private readonly ILogger<GlobalExceptionHandler> _logger;

    public GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger)
    {
        _logger = logger;
    }

    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        // 强制把所有异常转为 ApiResponse，确保返回 200 或 400
        httpContext.Response.StatusCode = StatusCodes.Status200OK;
        httpContext.Response.ContentType = "application/json";

        ApiResponse response;

        // 关键点：这里要用真正的类型判断
        if (exception is RCS.Core.Exceptions.DomainException domainEx)
        {
            response = ApiResponse.Fail(domainEx.Message, 400); // 返回业务错误码 400
        }
        else
        {
            // 如果是其他未知异常，记录日志
            _logger.LogError(exception, "系统发生未捕获异常");
            response = ApiResponse.Fail("服务器内部开小差了，请稍后再试。", 500);
        }

        await httpContext.Response.WriteAsJsonAsync(response, cancellationToken);
        return true; // 告诉中间件：异常已被处理
    }
}