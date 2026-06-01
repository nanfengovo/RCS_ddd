namespace RCS.Core.Common.Auditing;

public class AuditLog
{
    public string Id { get; set; } = Guid.NewGuid().ToString("N"); // MongoDB 推荐 string 类型的 ID
    public string? UserId { get; set; }
    public string? UserName { get; set; }
    
    public string ActionName { get; set; } = null!; // 执行的动作 (比如 LockLocationCommand)
    public string? Parameters { get; set; }         // 请求参数 (JSON字符串)
    
    public long ExecutionDuration { get; set; }     // 执行耗时 (毫秒)
    public bool IsSuccess { get; set; }             // 是否成功
    public string? ErrorMessage { get; set; }       // 如果失败，记录错误信息
    
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}