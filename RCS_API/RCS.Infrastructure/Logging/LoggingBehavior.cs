using System.Diagnostics;
using System.Text.Json;
using MediatR;
using Microsoft.Extensions.Logging;

namespace RCS.Infrastructure.Logging;

public class LoggingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : notnull
{
    private readonly ILogger<LoggingBehavior<TRequest, TResponse>> _logger;

    public LoggingBehavior(ILogger<LoggingBehavior<TRequest, TResponse>> logger)
    {
        _logger = logger;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        var requestName = typeof(TRequest).Name;
        _logger.LogInformation("➡️ [开始执行] {RequestName} | Payload: {Data}", requestName, JsonSerializer.Serialize(request));

        var timer = Stopwatch.StartNew();
        try
        {
            var response = await next();
            timer.Stop();
            
            if(timer.ElapsedMilliseconds > 500)
                _logger.LogWarning("⚠️ [执行缓慢] {RequestName} 耗时: {Time} ms", requestName, timer.ElapsedMilliseconds);
            else
                _logger.LogInformation("✅ [执行成功] {RequestName} 耗时: {Time} ms", requestName, timer.ElapsedMilliseconds);

            return response;
        }
        catch (Exception ex)
        {
            timer.Stop();
            _logger.LogError("❌ [执行失败] {RequestName} 耗时: {Time} ms | Error: {Msg}", requestName, timer.ElapsedMilliseconds, ex.Message);
            throw;
        }
    }
}