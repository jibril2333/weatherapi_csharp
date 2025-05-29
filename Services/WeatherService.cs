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
        using var client = _httpClientFactory.CreateClient();
        string url = $"https://geocoding-api.open-meteo.com/v1/search?name={cityName}&count=1&language=en&format=json";
        var response = await client.GetAsync(url);
        response.EnsureSuccessStatusCode(); // 确保请求成功
        var content = await response.Content.ReadAsStringAsync(); // 读取响应内容

        using var json = JsonDocument.Parse(content); // 解析JSON
        var root = json.RootElement; // 获取根元素
        var results = root.GetProperty("results").EnumerateArray(); // 获取results元素
        var firstResult = results.First(); // 获取第一个结果
        var latitude = firstResult.GetProperty("latitude").GetDouble(); // 获取latitude元素
        var longitude = firstResult.GetProperty("longitude").GetDouble(); // 获取longitude元素
        return (latitude, longitude);
    }


    /// <summary>
    /// 获取天气预报
    /// </summary>
    /// <param name="latitude">纬度</param>
    /// <param name="longitude">经度</param>
    /// <returns>天气预报数组</returns>
    public async Task<DailyWeather[]> GetForecastAsync(double latitude = 35.6895, double longitude = 139.6917)
    {
        _logger.LogInformation("Weather2Service: Generating weather forecast");

        using var client = _httpClientFactory.CreateClient();
        string url = $"https://api.open-meteo.com/v1/forecast" +
                     $"?latitude={latitude}&longitude={longitude}" +
                     $"&daily=temperature_2m_max,temperature_2m_min" +
                     $"&timezone=Asia%2FTokyo";
        var response = await client.GetAsync(url);
        response.EnsureSuccessStatusCode(); // 确保请求成功
        var content = await response.Content.ReadAsStringAsync(); // 读取响应内容

        using var json = JsonDocument.Parse(content); // 解析JSON
        var root = json.RootElement; // 获取根元素
        var daily = root.GetProperty("daily"); // 获取daily元素

        var dates = daily.GetProperty("time").EnumerateArray(); // 获取time元素
        var maxTemps = daily.GetProperty("temperature_2m_max").EnumerateArray(); // 获取temperature_2m_max元素
        var minTemps = daily.GetProperty("temperature_2m_min").EnumerateArray(); // 获取temperature_2m_min元素

        var result = new List<DailyWeather>();

        using var dateEnum = dates.GetEnumerator(); // 获取date元素
        using var maxEnum = maxTemps.GetEnumerator(); // 获取maxTemps元素
        using var minEnum = minTemps.GetEnumerator(); // 获取minTemps元素

        while (dateEnum.MoveNext() && maxEnum.MoveNext() && minEnum.MoveNext())
        {
            result.Add(new DailyWeather(
                DateOnly.Parse(dateEnum.Current.ToString()),  // 将 JsonElement 转换为 DateOnly
                maxEnum.Current.GetDouble(),                   // 将 JsonElement 转换为 double
                minEnum.Current.GetDouble()                    // 将 JsonElement 转换为 double
            ));
        }
        return result.ToArray();
    }
}