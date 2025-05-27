using restapi_c.Models;

namespace restapi_c.Interfaces;

public interface ICityService
{
    // 获取所有城市
    City[] GetCities();

    // 获取特定城市天气
    // 参数：
    //   - cityName: 城市名称（不能为空）
    // 返回：
    //   - 成功：返回该城市的天气预报数组
    //   - 失败：返回空数组
    WeatherForecast[] GetCityWeather(string cityName);

    // 检查城市是否存在
    // 参数：
    //   - cityName: 城市名称
    // 返回：
    //   - true: 城市存在
    //   - false: 城市不存在
    bool IsCityExists(string cityName);
} 