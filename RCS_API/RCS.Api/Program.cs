using RCS.Infrastructure;
using RCS.Application.Modules.Wms.Commands;
using Scalar.AspNetCore;
using RCS.Infrastructure.Persistence.EntityFramework;
using Microsoft.EntityFrameworkCore;

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