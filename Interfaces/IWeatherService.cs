using restapi_c.Models;

namespace restapi_c.Interfaces;

/// <summary>
/// 天气服务接口 - 第二个版本
/// </summary>
public interface IWeather2Service
{

    // 获取城市经纬度,用tuple返回
    Task<(double latitude, double longitude)> GetCityCoordinatesAsync(string cityName);

    /// <summary>
    /// 获取天气预报
    /// </summary>
    /// <returns>天气预报数组</returns>
    Task<DailyWeather[]> GetForecastAsync(double latitude = 35.6895, double longitude = 139.6917);
} 