var builder = DistributedApplication.CreateBuilder(args);

//1. 引入业务关系型数据库（PostgreSQL) + PgAdmin可视化工具
var postgres = builder.AddPostgres("postgres-server")
                                                                .WithPgAdmin()
                                                                .AddDatabase("RcsCoreDb");

// 2. 高频状态缓存 (Redis) + RedisInsight 可视化工具
var redis = builder.AddRedis("redis-cache")
                   .WithRedisInsight();

// 3. 非结构化/日志/AI Prompt 存储 (MongoDB) + MongoExpress 可视化工具
var mongo = builder.AddMongoDB("mongodb-server")
                   .WithMongoExpress()
                   .AddDatabase("RcsLogDb");

// 4. 事件总线与死信队列 (RabbitMQ) + 自带的 Management UI
var rabbitmq = builder.AddRabbitMQ("event-bus")
                      .WithManagementPlugin();

// 5. 注册你的 Web API，并把四大金刚的连接字符串自动注入进去！
var apiService = builder.AddProject<Projects.RCS_Api>("rcs-api")
                        .WithReference(postgres)
                        .WithReference(redis)
                        .WithReference(mongo)
                        .WithReference(rabbitmq)
                        .WaitFor(postgres); // 确保 API 在 PostgreSQL 准备好后才启动

builder.Build().Run();
