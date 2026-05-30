using Microsoft.Extensions.Configuration;

var builder = DistributedApplication.CreateBuilder(args);

// ==========================================
// 🚀 核心架构开关：是否使用本地基础设施
// ==========================================
var useLocalInfrastructure = builder.Configuration.GetValue<bool>("UseLocalInfrastructure", false);

// 预先声明四大金刚的依赖引用
IResourceBuilder<IResourceWithConnectionString> postgresDb;
IResourceBuilder<IResourceWithConnectionString> redisCache;
IResourceBuilder<IResourceWithConnectionString> mongoDb;
IResourceBuilder<IResourceWithConnectionString> rabbitmqBus;

if (useLocalInfrastructure)
{
    // ---------------------------------------------------
    // 方案 A：物理机/内网裸机/云托管服务模式 (完全不碰 Docker)
    // ---------------------------------------------------
    postgresDb = builder.AddConnectionString("RcsCoreDb");
    redisCache = builder.AddConnectionString("redis-cache");
    mongoDb = builder.AddConnectionString("mongodb-server");
    rabbitmqBus = builder.AddConnectionString("event-bus");
}
else
{
   // ---------------------------------------------------
    // 方案 B：极客容器化一键开发模式 (强制全部开启密码验证，且零警告)
    // ---------------------------------------------------

    var pgPassword = builder.AddParameter("PgPassword", secret: true);
    var mongoUser = builder.AddParameter("MongoUser");
    var mongoPwd = builder.AddParameter("MongoPwd", secret: true);
    var rmqUser = builder.AddParameter("RmqUser");
    var rmqPwd = builder.AddParameter("RmqPwd", secret: true);
    var redisPwd = builder.AddParameter("RedisPwd", secret: true);

    // 1. Postgres (原生完美支持密码与健康检查)
    var postgres = builder.AddPostgres("postgres-server", password: pgPassword)
                          .WithPgAdmin()
                          .WithContainerName("RCSCoreDB")
                          .WithEndpoint(port: 5432,targetPort: 5432,name:"tcp")
                          .WithLifetime(ContainerLifetime.Persistent);
    postgresDb = postgres.AddDatabase("RcsCoreDb");

    // 2. Redis
    redisCache = builder.AddRedis("redis-cache")
                        .WithEndpoint(port: 6379,targetPort: 6379,name:"tcp")
                        .WithArgs("--requirepass", redisPwd)
                        .WithRedisInsight()
                        .WithContainerName("RCSCoreRedis")
                        .WithLifetime(ContainerLifetime.Persistent);
                        
    // 🎯 极客手术：切除 Redis 默认的无密码健康检查探针，消除 Unhealthy 警告
    var redisHc = redisCache.Resource.Annotations.FirstOrDefault(a => a.GetType().Name.Contains("HealthCheck"));
    if (redisHc != null) redisCache.Resource.Annotations.Remove(redisHc);

    // 3. Mongo
    var mongo = builder.AddMongoDB("mongodb-server")
                       .WithEnvironment("MONGO_INITDB_ROOT_USERNAME", mongoUser)
                       .WithEnvironment("MONGO_INITDB_ROOT_PASSWORD", mongoPwd)
                       .WithContainerName("RCSCoreLogMongo")
                       .WithLifetime(ContainerLifetime.Persistent);
                       
    // 修复伴生 UI 崩溃：把账号密码同步给 MongoExpress
    mongo.WithMongoExpress(c => 
    {
        c.WithEnvironment("ME_CONFIG_MONGODB_ADMINUSERNAME", mongoUser)
         .WithEnvironment("ME_CONFIG_MONGODB_ADMINPASSWORD", mongoPwd);
    });
    
    // 🎯 极客手术：切除 Mongo 默认的无密码健康检查探针，消除 Unhealthy 警告
    var mongoHc = mongo.Resource.Annotations.FirstOrDefault(a => a.GetType().Name.Contains("HealthCheck"));
    if (mongoHc != null) mongo.Resource.Annotations.Remove(mongoHc);

    mongoDb = mongo.AddDatabase("RcsLogDb");
    // 🎯 极客补刀：切除逻辑数据库 (RcsLogDb) 上的默认健康检查探针
    var mongoDbHc = mongoDb.Resource.Annotations.FirstOrDefault(a => a.GetType().Name.Contains("HealthCheck"));
    if (mongoDbHc != null) mongoDb.Resource.Annotations.Remove(mongoDbHc);

    // 4. RabbitMQ (原生完美支持密码与健康检查)
    rabbitmqBus = builder.AddRabbitMQ("event-bus", userName: rmqUser, password: rmqPwd)
                         .WithManagementPlugin()
                         .WithContainerName("RCSCoreRabbitMQ")
                         .WithLifetime(ContainerLifetime.Persistent);
}

// ==========================================
// 🎯 注入给 API
// ==========================================
var apiService = builder.AddProject<Projects.RCS_Api>("rcs-api")
                        .WithReference(postgresDb)
                        .WithReference(redisCache)
                        .WithReference(mongoDb)
                        .WithReference(rabbitmqBus);

// 🛡️ 仅在 Docker 模式下，才需要等待容器变为 Healthy 状态
if (!useLocalInfrastructure)
{
    apiService.WaitFor(postgresDb);
}

builder.Build().Run();