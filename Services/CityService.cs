using restapi_c.Interfaces;
using restapi_c.Models;

namespace restapi_c.Services;

public class CityService : ICityService
{
    // 城市数据
    private static readonly City[] Cities = new[]
    {
        new City("Beijing", "China"),
        new City("Shanghai", "China"),
        new City("Tokyo", "Japan"),
        new City("New York", "USA"),
        new City("London", "UK")
    };

    // 天气描述数据
    private static readonly string[] Summaries = new[]
    {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

    // 获取所有城市
    public City[] GetCities()
    {
        return Cities;
    }

    // 获取特定城市天气
    public WeatherForecast[] GetCityWeather(string cityName)
    {
        if (!IsCityExists(cityName))
        {
            return Array.Empty<WeatherForecast>();
        }

        return Enumerable.Range(1, 5).Select(index =>
            new WeatherForecast
            (
                DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                Random.Shared.Next(-20, 55),
                Summaries[Random.Shared.Next(Summaries.Length)]
            ))
            .ToArray();
    }

    // 检查城市是否存在
    public bool IsCityExists(string cityName)
    {
        return Cities.Any(c => c.Name.Equals(cityName, StringComparison.OrdinalIgnoreCase));
    }
} 