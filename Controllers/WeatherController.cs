using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using restapi_c.Interfaces;

namespace restapi_c.Controllers;

[ApiController]
[Route("[controller]")]
public class WeatherController(ILogger<WeatherController> _logger, IWeatherService _weatherService) : ControllerBase
{
    [HttpGet("{cityName}")]
    public async Task<IActionResult> Get(string cityName)
    {
        _logger.LogInformation("收到获取城市 {CityName} 天气信息的请求", cityName);
        
        try
        {
            var (latitude, longitude) = await _weatherService.GetCityCoordinatesAsync(cityName);
            var forecast = await _weatherService.GetForecastAsync(latitude, longitude);
            _logger.LogInformation("成功返回城市 {CityName} 的天气信息", cityName);
            return Ok(forecast);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "获取城市 {CityName} 的天气数据失败: {ErrorMessage}", cityName, ex.Message);
            return StatusCode(500, $"获取城市 {cityName} 的天气数据失败: {ex.Message}");
        }
    }
}
