using System.Net.Http.Json;
using restapi_c.Interfaces;
using restapi_c.Models;

namespace restapi_c.Services;

public class WeatherService2 : IWeatherService
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<WeatherService2> _logger;

    public WeatherService2(HttpClient httpClient, ILogger<WeatherService2> logger)
    {
        _httpClient = httpClient;
        _logger = logger;
    }

    // 实现接口方法
    public WeatherForecast[] GetForecast()
    {
        return GetForecast("beijing").Result; // 默认返回北京的天气
    }

    // 新增的带城市参数的方法
    public async Task<WeatherForecast[]> GetForecast(string city)
    {
        try
        {
            // 1. 获取城市坐标
            var geocodingUrl = $"https://geocoding-api.open-meteo.com/v1/search?name={city}&count=1";
            var geocodingResponse = await _httpClient.GetFromJsonAsync<GeocodingResponse>(geocodingUrl);
            
            if (geocodingResponse?.Results == null || !geocodingResponse.Results.Any())
            {
                _logger.LogWarning("City not found: {City}", city);
                return Array.Empty<WeatherForecast>();
            }

            var location = geocodingResponse.Results[0];

            // 2. 获取天气数据
            var weatherUrl = $"https://api.open-meteo.com/v1/forecast?latitude={location.Latitude}&longitude={location.Longitude}&daily=temperature_2m_max,temperature_2m_min,precipitation_probability_max&timezone=auto";
            var weatherResponse = await _httpClient.GetFromJsonAsync<WeatherResponse>(weatherUrl);

            if (weatherResponse?.Daily == null)
            {
                _logger.LogWarning("Weather data not found for city: {City}", city);
                return Array.Empty<WeatherForecast>();
            }

            // 3. 转换数据格式
            var forecasts = new List<WeatherForecast>();
            for (int i = 0; i < weatherResponse.Daily.Time.Length; i++)
            {
                forecasts.Add(new WeatherForecast
                {
                    Date = DateTime.Parse(weatherResponse.Daily.Time[i]),
                    TemperatureC = (int)((weatherResponse.Daily.Temperature2mMax[i] + weatherResponse.Daily.Temperature2mMin[i]) / 2),
                    Summary = GetWeatherSummary(weatherResponse.Daily.PrecipitationProbabilityMax[i])
                });
            }

            return forecasts.ToArray();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting weather forecast for city: {City}", city);
            return Array.Empty<WeatherForecast>();
        }
    }

    private string GetWeatherSummary(int precipitationProbability)
    {
        return precipitationProbability switch
        {
            < 20 => "Sunny",
            < 40 => "Partly Cloudy",
            < 60 => "Cloudy",
            < 80 => "Light Rain",
            _ => "Heavy Rain"
        };
    }
}

// API响应模型
public class GeocodingResponse
{
    public List<LocationResult> Results { get; set; }
}

public class LocationResult
{
    public float Latitude { get; set; }
    public float Longitude { get; set; }
}

public class WeatherResponse
{
    public DailyData Daily { get; set; }
}

public class DailyData
{
    public string[] Time { get; set; }
    public float[] Temperature2mMax { get; set; }
    public float[] Temperature2mMin { get; set; }
    public int[] PrecipitationProbabilityMax { get; set; }
} 