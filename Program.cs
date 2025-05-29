using Microsoft.OpenApi.Models;
using restapi_c.Interfaces;
using restapi_c.Services;
using restapi_c.Models;

var builder = WebApplication.CreateBuilder(args);

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

// 配置 API 设置
builder.Services.Configure<ApiSettings>(builder.Configuration.GetSection("ApiSettings"));



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