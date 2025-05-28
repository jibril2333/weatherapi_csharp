using Microsoft.AspNetCore.Mvc;
using restapi_c.Interfaces;
using Microsoft.Extensions.Logging;

namespace restapi_c.Controllers;

[ApiController]
[Route("[controller]")]
public class CityController : ControllerBase
{
    private readonly ICityService _cityService;
    private readonly ILogger<CityController> _logger;

    public CityController(ICityService cityService, ILogger<CityController> logger)
    {
        _cityService = cityService;
        _logger = logger;
    }

    [HttpGet]
    public IActionResult Get()
    {
        _logger.LogInformation("Getting all cities");
        var cities = _cityService.GetCities();
        return Ok(cities);
        
    }

    [HttpGet("{cityName}/weather")]
    public IActionResult GetCityWeather(string cityName)
    {
        // 1. 先检查城市是否存在
        if (!_cityService.IsCityExists(cityName))
        {
            _logger.LogWarning("City not found: {CityName}", cityName);
            return NotFound($"City '{cityName}' not found");
        }

        // 2. 记录请求日志
        _logger.LogInformation("Getting weather for city: {CityName}", cityName);
        
        // 3. 获取天气数据
        var weather = _cityService.GetCityWeather(cityName);
        
        // 4. 返回天气数据
        return Ok(weather);
    }
} 