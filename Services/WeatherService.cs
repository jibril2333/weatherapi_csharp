using restapi_c.Interfaces;
using restapi_c.Models;
using System.Net.Http;
using System.Threading.Tasks;
using System.Text.Json;
using System.Collections.Generic;
using System;


namespace restapi_c.Services;


/// <summary>
/// 天气服务实现 - 第二个版本
/// </summary>
public class WeatherService(ILogger<WeatherService> _logger, IHttpClientFactory _httpClientFactory) : IWeatherService
{
    // private static readonly string[] Summaries = new[]
    // {
    //     "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    // };

    public async Task<(double latitude, double longitude)> GetCityCoordinatesAsync(string cityName = "Tokyo")
    {
        try
        {
            using var client = _httpClientFactory.CreateClient();
            string url = $"https://geocoding-api.open-meteo.com/v1/search?name={cityName}&count=1&language=en&format=json";
            _logger.LogDebug("调用地理编码 API: {Url}", url);
            
            var response = await client.GetAsync(url);
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();

            using var json = JsonDocument.Parse(content);
            var root = json.RootElement;
            var results = root.GetProperty("results").EnumerateArray();
            var firstResult = results.First();
            var latitude = firstResult.GetProperty("latitude").GetDouble();
            var longitude = firstResult.GetProperty("longitude").GetDouble();
            
            _logger.LogInformation("获取城市 {CityName} 的经纬度: 纬度={Latitude}, 经度={Longitude}", 
                cityName, latitude, longitude);
            return (latitude, longitude);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "获取城市 {CityName} 的经纬度失败: {ErrorMessage}", 
                cityName, ex.Message);
            throw;
        }
    }


    /// <summary>
    /// 获取天气预报
    /// </summary>
    /// <param name="latitude">纬度</param>
    /// <param name="longitude">经度</param>
    /// <returns>天气预报数组</returns>
    public async Task<DailyWeather[]> GetForecastAsync(double latitude = 35.6895, double longitude = 139.6917)
    {
        try
        {
            using var client = _httpClientFactory.CreateClient();
            string url = $"https://api.open-meteo.com/v1/forecast" +
                         $"?latitude={latitude}&longitude={longitude}" +
                         $"&daily=temperature_2m_max,temperature_2m_min" +
                         $"&timezone=Asia%2FTokyo";
            _logger.LogDebug("调用天气预报 API: {Url}", url);
            
            var response = await client.GetAsync(url);
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();

            using var json = JsonDocument.Parse(content);
            var root = json.RootElement;
            var daily = root.GetProperty("daily");

            var dates = daily.GetProperty("time").EnumerateArray();
            var maxTemps = daily.GetProperty("temperature_2m_max").EnumerateArray();
            var minTemps = daily.GetProperty("temperature_2m_min").EnumerateArray();

            var result = new List<DailyWeather>();

            using var dateEnum = dates.GetEnumerator();
            using var maxEnum = maxTemps.GetEnumerator();
            using var minEnum = minTemps.GetEnumerator();

            while (dateEnum.MoveNext() && maxEnum.MoveNext() && minEnum.MoveNext())
            {
                result.Add(new DailyWeather(
                    DateOnly.Parse(dateEnum.Current.ToString()),
                    maxEnum.Current.GetDouble(),
                    minEnum.Current.GetDouble()
                ));
            }

            _logger.LogInformation("获取天气预报数据: 纬度={Latitude}, 经度={Longitude}, 天数={Count}", 
                latitude, longitude, result.Count);
            return result.ToArray();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "获取天气预报失败: 纬度={Latitude}, 经度={Longitude}, 错误: {ErrorMessage}", 
                latitude, longitude, ex.Message);
            throw;
        }
    }
}