using Microsoft.AspNetCore.Mvc;
using restapi_c.Interfaces;
using Microsoft.Extensions.Logging;

namespace restapi_c.Controllers;

// ApiController特性：标记这是一个API控制器，启用API特定的行为
// 包括：自动模型验证、自动HTTP 400响应、绑定源推理等
[ApiController]
// Route特性：定义控制器的路由模板
// [controller]会被替换为控制器名称（去掉"Controller"后缀），即"Weather"
[Route("[controller]")]
public class WeatherController(IWeatherService _weatherService, ILogger<WeatherController> _logger) : ControllerBase
{
    // HttpGet特性：标记这是一个处理GET请求的动作方法
    // Name参数：为这个动作指定一个名称，用于生成URL
    [HttpGet(Name = "GetWeatherForecast")]
    public IActionResult Get()
    {
        // 记录请求时间
        _logger.LogInformation("Weather forecast requested at {time}", DateTime.UtcNow);
        
        // 调用天气服务获取预报数据
        var forecast = _weatherService.GetForecast();
        
        // 记录返回的预报数量
        _logger.LogInformation($"Returning {forecast.Length} weather forecasts");
        
        // 返回200 OK响应，包含预报数据
        return Ok(forecast);
    }
} 
