using RCS.Core.Common.Auditing;

namespace RCS.Infrastructure.Logging.Mongo;

internal class QueuedAuditLogger : IAuditLogger
{
private readonly AuditLogQueue _queue;

    public QueuedAuditLogger(AuditLogQueue queue)
    {
        _queue = queue;
    }

    public Task LogAsync(AuditLog log)
    {
        // 💡 探照灯 1：确认发件人工作正常
        Console.WriteLine($"\n 🚀🚀🚀 [探照灯 1] 拦截器已触发，准备将 {log.ActionName} 扔进队列！\n");
        // 瞬间写入内存，极其轻量！完全不影响主线程！
        _queue.WriteAsync(log).AsTask();
        return Task.CompletedTask;
    }
}