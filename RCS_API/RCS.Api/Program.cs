using Microsoft.EntityFrameworkCore;
using Npgsql;
using OpenTelemetry;
using OpenTelemetry.Metrics;
using OpenTelemetry.Trace;
using RCS.Application.Modules.Wms.Commands;
using RCS.Core.Modules.Wms.Repositories;
using RCS.Infrastructure.Modules.Wms.Repositories;
using RCS.Infrastructure.Persistence.EntityFramework;
using Scalar.AspNetCore; // 引入现代化的 API UI

var builder = WebApplication.CreateBuilder(args);

// ==========================================
// 📡 极客架构：配置 OpenTelemetry 可观测性
// ==========================================
builder.Logging.AddOpenTelemetry(logging =>
{
    logging.IncludeFormattedMessage = true;
    logging.IncludeScopes = true;
});

builder.Services.AddOpenTelemetry()
    .WithMetrics(metrics =>
    {
        metrics.AddAspNetCoreInstrumentation(); // 收集 API 吞吐量、耗时等指标
    })
    .WithTracing(tracing =>
    {
        tracing.AddAspNetCoreInstrumentation(); // 追踪 HTTP 请求
        tracing.AddNpgsql();                    // 追踪 PostgreSQL 数据库调用
    })
    .UseOtlpExporter(); // 将数据统一发射给 Aspire Dashboard
// ==========================================

// 1. 添加控制器支持 (你的默认模板里只有 Minimal API，我们需要 Controller)
builder.Services.AddControllers();

// 2. .NET 10 原生的 OpenAPI 生成规范
builder.Services.AddOpenApi();

// 3. 注册数据库上下文 (PostgreSQL)
builder.Services.AddDbContext<RcsDbContext>(options =>
    options.UseNpgsql(
        builder.Configuration.GetConnectionString("RcsCoreDb"),
        npgsqlOptions => npgsqlOptions.EnableRetryOnFailure()
        ));
// builder.Services.AddDbContext<RcsDbContext>(options =>
//     options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// 4. 注册 MediatR (CQRS 核心)
builder.Services.AddMediatR(cfg => 
    cfg.RegisterServicesFromAssembly(typeof(LockLocationCommand).Assembly));

// 5. 依赖注入：组装仓储
builder.Services.AddScoped<ILocationRepository, LocationRepository>();

var app = builder.Build();

// 6. 配置 HTTP 请求管道
if (app.Environment.IsDevelopment())
{
    // 开启原生 OpenAPI 数据节点
    app.MapOpenApi(); 
    // 挂载绝美的 Scalar API 测试页面！
    app.MapScalarApiReference(); 
}

app.UseAuthorization();
app.MapControllers(); // 映射我们的 LocationController

// 在开发环境下，自动应用 EF Core 数据迁移
if (app.Environment.IsDevelopment())
{
    using var scope = app.Services.CreateScope();
    var dbContext = scope.ServiceProvider.GetRequiredService<RcsDbContext>();
    // 这行代码等同于在控制台敲 dotnet ef database update
    dbContext.Database.Migrate(); 
}

app.Run();