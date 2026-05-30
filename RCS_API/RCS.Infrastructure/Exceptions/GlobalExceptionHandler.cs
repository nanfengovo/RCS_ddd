using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using RCS.Core.Common;
using RCS.Core.Exceptions;

namespace RCS.Infrastructure.Exceptions;

public class GlobalExceptionHandler : IExceptionHandler
{
    private readonly ILogger<GlobalExceptionHandler> _logger;

    public GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger)
    {
        _logger = logger;
    }

    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        _logger.LogError(exception, "系统发生未捕获异常: {Message}", exception.Message);

        var statusCode = exception switch
        {
            DomainException => StatusCodes.Status400BadRequest,
            UnauthorizedAccessException => StatusCodes.Status401Unauthorized,
            _ => StatusCodes.Status500InternalServerError
        };

        var message = exception is DomainException ? exception.Message : "服务器内部开小差了，请稍后再试。";

        // 统一包装为我们自定义的 ApiResponse
        var apiResponse = ApiResponse.Fail(message, statusCode);

        httpContext.Response.StatusCode = StatusCodes.Status200OK; // 业务上全返回 200，前端看 Code
        await httpContext.Response.WriteAsJsonAsync(apiResponse, cancellationToken);

        return true;
    }
}