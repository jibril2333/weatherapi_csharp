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
public class Weather2Controller(ILogger<Weather2Controller> _logger, IWeather2Service _weather2Service) : ControllerBase
{
    // 你现在可以在类的方法中直接使用 _logger 和 _weather2Service
    [HttpGet("{cityName}")]
    public async Task<IActionResult> Get(string cityName)
    {
        try
        {
            var (latitude, longitude) = await _weather2Service.GetCityCoordinatesAsync(cityName);
            var forecast = await _weather2Service.GetForecastAsync(latitude, longitude);
            return Ok(forecast);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "获取天气数据失败");
            return StatusCode(500, "获取天气数据失败");
        }
    }
}
