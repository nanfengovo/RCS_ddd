using System.Threading.Channels;
using RCS.Core.Common.Auditing;

namespace RCS.Infrastructure.Logging;

/// <summary>
/// 极客架构：审计日志内存无锁队列 (In-Memory MQ)
/// </summary>
public class AuditLogQueue
{
    // Bounded 意味着有界队列，防止极端情况下日志堆积撑爆内存 (比如 Mongo 宕机了一整天)
    // 设个 10 万的容量，满了就丢弃旧的，保全系统主业务不挂。
    private readonly Channel<AuditLog> _queue = Channel.CreateBounded<AuditLog>(
        new BoundedChannelOptions(100_000)
        {
            FullMode = BoundedChannelFullMode.DropOldest 
        });

    // 生产者：API 线程调用这个把日志扔进去
    public async ValueTask WriteAsync(AuditLog log)
    {
        await _queue.Writer.WriteAsync(log);
    }

    // 消费者：后台 Worker 读日志
    public IAsyncEnumerable<AuditLog> ReadAllAsync(CancellationToken cancellationToken)
    {
        return _queue.Reader.ReadAllAsync(cancellationToken);
    }
}