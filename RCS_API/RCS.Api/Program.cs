using RCS.Infrastructure;
using RCS.Application.Modules.Wms.Commands;
using Scalar.AspNetCore;
using RCS.Infrastructure.Persistence.EntityFramework;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.HttpLogging;

var builder = WebApplication.CreateBuilder(args);

// ==========================================
// 1. 极客架构：一键拉起所有底层重型武器
// ==========================================
builder.Services.AddInfrastructure(builder.Configuration);

// 2. 注册 MediatR (注意：极客做法是给 Application 层也写个 AddApplication() 扩展，先放这也没关系)
builder.Services.AddMediatR(cfg => 
    cfg.RegisterServicesFromAssembly(typeof(LockLocationCommand).Assembly));

builder.Services.AddControllers();
builder.Services.AddOpenApi(); // .NET 10 原生 OpenAPI

// ==========================================
// 🚀 极客基建：全局入站 HTTP 流量雷达
// ==========================================
builder.Services.AddHttpLogging(options =>
{
    // 🔥 杀手锏特性：把 Request 和 Response 融合成一条极其干净的日志！
    // 告别以前那种“一条进、一条出”刷屏的痛苦
    options.CombineLogs = true; 

    // 🎯 精准打击：我们只抓取最有运维价值的字段，绝不拖慢系统性能
    options.LoggingFields = HttpLoggingFields.RequestMethod |
                            HttpLoggingFields.RequestPath |
                            HttpLoggingFields.ResponseStatusCode |
                            HttpLoggingFields.Duration |
                            HttpLoggingFields.RequestHeaders;

    // 🕵️‍♂️ 重点监控：抓取客户端类型和真实 IP
    options.RequestHeaders.Add("User-Agent");
    options.RequestHeaders.Add("X-Forwarded-For"); 
});

// 1. 定义一个允许跨域的策略名称
var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

// 2. 注入 CORS 服务
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins,
                      policy =>
                      {
                          policy.WithOrigins("http://localhost:5173") // 👈 这里填你 React 前端的准确地址
                                .AllowAnyHeader()   // 允许携带任何请求头 (如 Authorization 里的 Token)
                                .AllowAnyMethod()   // 允许任何 HTTP 方法 (GET, POST, DELETE 等)
                                .AllowCredentials(); // 允许携带凭证 (如果将来用到 Cookie)
                      });
});

var app = builder.Build();

// ==========================================
// HTTP 管道配置
// ==========================================
app.UseExceptionHandler(); // 开启全局异常拦截

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi(); 
    app.MapScalarApiReference(); // 挂载绝美的 Scalar API UI
}

// 🚀 开启入站雷达拦截！(所有路过的 HTTP 请求都会被它拍下快照)
app.UseHttpLogging();
app.UseCors(MyAllowSpecificOrigins); // 开启 CORS
app.UseAuthorization();
app.MapControllers();

// 自动应用 EF Core 数据迁移 (开发环境专属便利)
if (app.Environment.IsDevelopment())
{
    using var scope = app.Services.CreateScope();
    var dbContext = scope.ServiceProvider.GetRequiredService<RcsDbContext>();
    dbContext.Database.Migrate(); 
}

app.Run();