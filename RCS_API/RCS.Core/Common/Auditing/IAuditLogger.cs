namespace RCS.Core.Common.Auditing;

public interface IAuditLogger
{
    // 异步写入日志
    Task LogAsync(AuditLog log);
}