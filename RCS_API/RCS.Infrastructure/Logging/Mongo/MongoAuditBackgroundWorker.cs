using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using RCS.Core.Common.Auditing;

namespace RCS.Infrastructure.Logging.Mongo;

/// <summary>
/// 极客架构：后台异步日志写入服务 (独立于请求线程)
/// </summary>
internal class MongoAuditBackgroundWorker : BackgroundService
{
    private readonly AuditLogQueue _queue;
    private readonly ILogger<MongoAuditBackgroundWorker> _logger;
    private readonly IMongoCollection<AuditLog> _auditLogs;

    public MongoAuditBackgroundWorker(AuditLogQueue queue, IConfiguration configuration, ILogger<MongoAuditBackgroundWorker> logger)
    {
        _queue = queue;
        _logger = logger;

        // 🚀 核心修复：双重 fallback 机制
        var connectionString = configuration.GetConnectionString("RcsLogDb") 
                            ?? configuration.GetConnectionString("mongodb-server");

        if (string.IsNullOrEmpty(connectionString))
        {
            throw new InvalidOperationException("🚨 致命错误：未找到 MongoDB 连接字符串！请检查 Aspire 引用或 appsettings.json 配置。");
        }
        
        // ==========================================
        // 🚀 终极破案核心：修复 MongoDB 驱动的“认证库漂移”问题
        // ==========================================
        var urlBuilder = new MongoUrlBuilder(connectionString);
        
        // 强制告诉驱动：不管你要操作哪个业务库，验密码时必须去 admin 库！
        urlBuilder.AuthenticationSource = "admin"; 
        
        var client = new MongoClient(urlBuilder.ToMongoUrl());
        // ==========================================
        
        var database = client.GetDatabase(urlBuilder.DatabaseName ?? "RcsLogDb"); 
        _auditLogs = database.GetCollection<AuditLog>("audit_logs");
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("🛡️ 审计日志后台队列写入器已启动...");
        // 💡 探照灯 2：确认工人正常打卡上班
        _logger.LogWarning("\n 🛡️🛡️🛡️ [探照灯 2] 审计日志后台工人已成功打卡上班，正在死守队列...\n");
        
        try
        {
            // 只要系统不关闭，这个循环就会一直等在队列门口
            await foreach (var log in _queue.ReadAllAsync(stoppingToken))
            {
                // 💡 探照灯 3A：确认工人拿到货了
                _logger.LogWarning("📥 [探照灯 3A] 工人从队列里拿到了一条日志！准备写入 Mongo，操作: {ActionName}", log.ActionName);
                
                try
                {
                    // 真正的写入动作发生在这里，绝对不阻塞 API 线程！
                    await _auditLogs.InsertOneAsync(log, cancellationToken: stoppingToken);
                    
                    // 💡 探照灯 3B：确认写入成功
                    _logger.LogWarning("✅✅✅ [探照灯 3B] 成功写入 MongoDB！Id: {Id}", log.Id);
                }
                catch (Exception ex)
                {
                    // 🚨 探照灯 3C：极其精确的 Mongo 写入报错
                    _logger.LogError(ex, "❌ [探照灯 3C] 写入 MongoDB 发生血腥崩溃！原因: {Message}", ex.Message);
                }
            }
        }
        catch (OperationCanceledException)
        {
            _logger.LogInformation("后台工人准备下班。");
        }
    }
}