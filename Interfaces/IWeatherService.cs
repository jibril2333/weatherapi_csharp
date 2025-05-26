using restapi_c.Models;

namespace restapi_c.Interfaces;

public interface IWeatherService
{
    WeatherForecast[] GetForecast();
} 