using System.Diagnostics;
using Microsoft.Extensions.Logging;
using RCS.Core.Common.Auditing;

namespace RCS.Infrastructure.Logging.Http;

/// <summary>
/// 极客基建：全局第三方出站流量拦截器
/// </summary>
public class OutboundLoggingHandler : DelegatingHandler
{
    private readonly ILogger<OutboundLoggingHandler> _logger;
    private readonly AuditLogQueue _queue; // 🚀 复用我们之前的 Mongo 内存队列！

    public OutboundLoggingHandler(ILogger<OutboundLoggingHandler> logger, AuditLogQueue queue)
    {
        _logger = logger;
        _queue = queue;
    }

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var stopwatch = Stopwatch.StartNew();
        var targetUrl = request.RequestUri?.ToString();
        var method = request.Method.Method;
        
        // 1. 偷窥请求体 (注意：HttpContent 的读取不会破坏原请求)
        string requestBody = request.Content != null 
            ? await request.Content.ReadAsStringAsync(cancellationToken) 
            : "";

        _logger.LogInformation("🚀 [发起外部调用] {Method} {Url}", method, targetUrl);

        try
        {
            // 👉 2. 放行请求，真正发往第三方服务器
            var response = await base.SendAsync(request, cancellationToken);
            stopwatch.Stop();

            // 3. 偷窥响应体
            string responseBody = await response.Content.ReadAsStringAsync(cancellationToken);

            // 🚀 4. 组装出站日志，异步扔进 Mongo 队列！(复用我们之前的基建)
            var outLog = new AuditLog 
            {
                ActionName = $"[出站] {method} {request.RequestUri?.AbsolutePath}",
                UserId = "System", // 出站一般记为系统级行为
                Parameters = $"【Req】{requestBody} \n【Res】{responseBody}", // 暴力拼接，或存为结构化 JSON
                ExecutionDuration = stopwatch.ElapsedMilliseconds,
                IsSuccess = response.IsSuccessStatusCode
            };
            
            _ = _queue.WriteAsync(outLog).AsTask(); // Fire and Forget！

            return response;
        }
        catch (Exception ex)
        {
            // 记录超时、DNS 解析失败等网络级血腥崩溃
            stopwatch.Stop();
             _ = _queue.WriteAsync(new AuditLog 
             { 
                 ActionName = $"[出站崩溃] {targetUrl}",
                 IsSuccess = false,
                 ErrorMessage = ex.Message
             }).AsTask();
             
            throw;
        }
    }
}