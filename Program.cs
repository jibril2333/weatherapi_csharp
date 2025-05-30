using Microsoft.OpenApi.Models;
using restapi_c.Interfaces;
using restapi_c.Services;
using restapi_c.Models;
using DotNetEnv;

// 加载 .env 文件
var envPath = Path.Combine(Directory.GetCurrentDirectory(), ".env");
if (File.Exists(envPath))
{
    Env.Load(envPath);
    Console.WriteLine($"已加载 .env 文件: {envPath}");
}
else
{
    Console.WriteLine($"未找到 .env 文件: {envPath}");
}

var builder = WebApplication.CreateBuilder(args);

// 配置 API 设置
var apiKey = Environment.GetEnvironmentVariable("OPENAI_API_KEY");

if (string.IsNullOrEmpty(apiKey))
{
    Console.WriteLine("警告: OPENAI_API_KEY 环境变量为空");
}

var apiSettings = new ApiSettings
{
    ApiKey = apiKey ?? string.Empty,
    Model = "gpt-3.5-turbo"
};
builder.Services.Configure<ApiSettings>(options =>
{
    options.ApiKey = apiSettings.ApiKey;
    options.Model = apiSettings.Model;
});

// Add services to the container.
builder.Services.AddControllers(); //注册控制器，用于处理HTTP请求。
builder.Services.AddEndpointsApiExplorer(); //注册API探索器，用于生成API文档。可能不需要
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Weather API", Version = "v1" });
});

// 注册服务
builder.Services.AddHttpClient();  // 注册 IHttpClientFactory
builder.Services.AddScoped<IWeatherService, WeatherService>();  // 注册 Weather2Service
builder.Services.AddScoped<IChatService, ChatService>();  // 注册 ChatService

var app = builder.Build(); // 构建应用程序

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Weather API V1");
    });
}

// app.UseHttpsRedirection();
// 重定向到HTTPS，开发时暂时不用
// app.UseAuthorization();
// 授权，开发时暂时不用
app.MapControllers();
// 映射控制器
app.Run(); 