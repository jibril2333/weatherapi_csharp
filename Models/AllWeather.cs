namespace restapi_c.Models;

public class WeatherResponse
{
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public double Generationtime_ms { get; set; }
    public int Utc_offset_seconds { get; set; }
    public string Timezone { get; set; } = string.Empty;
    public string Timezone_abbreviation { get; set; } = string.Empty;
    public double Elevation { get; set; }
    public CurrentWeatherUnits? Current_weather_units { get; set; }
    public CurrentWeather? Current_weather { get; set; }
    public HourlyUnits? Hourly_units { get; set; }
    public Hourly? Hourly { get; set; }
    public DailyUnits? Daily_units { get; set; }
    public Daily? Daily { get; set; }
}

public class CurrentWeatherUnits
{
    public string Time { get; set; } = string.Empty;
    public string Interval { get; set; } = string.Empty;
    public string Temperature { get; set; } = string.Empty;
    public string Windspeed { get; set; } = string.Empty;
    public string Winddirection { get; set; } = string.Empty;
    public string Is_day { get; set; } = string.Empty;
    public string Weathercode { get; set; } = string.Empty;
}

public class CurrentWeather
{
    public string Time { get; set; } = string.Empty;
    public int Interval { get; set; }
    public double Temperature { get; set; }
    public double Windspeed { get; set; }
    public int Winddirection { get; set; }
    public int Is_day { get; set; }
    public int Weathercode { get; set; }
}

public class HourlyUnits
{
    public string Time { get; set; } = string.Empty;
    public string Temperature_2m { get; set; } = string.Empty;
    public string Precipitation { get; set; } = string.Empty;
    public string Precipitation_probability { get; set; } = string.Empty;
    public string Cloudcover { get; set; } = string.Empty;
    public string Windspeed_10m { get; set; } = string.Empty;
    public string Dewpoint_2m { get; set; } = string.Empty;
    public string Weathercode { get; set; } = string.Empty;
    public string Apparent_temperature { get; set; } = string.Empty;
}

public class Hourly
{
    public List<string> Time { get; set; } = new();
    public List<double> Temperature_2m { get; set; } = new();
    public List<double> Precipitation { get; set; } = new();
    public List<int> Precipitation_probability { get; set; } = new();
    public List<int> Cloudcover { get; set; } = new();
    public List<double> Windspeed_10m { get; set; } = new();
    public List<double> Dewpoint_2m { get; set; } = new();
    public List<int> Weathercode { get; set; } = new();
    public List<double> Apparent_temperature { get; set; } = new();
}

public class DailyUnits
{
    public string Time { get; set; } = string.Empty;
    public string Temperature_2m_max { get; set; } = string.Empty;
    public string Temperature_2m_min { get; set; } = string.Empty;
    public string Apparent_temperature_max { get; set; } = string.Empty;
    public string Apparent_temperature_min { get; set; } = string.Empty;
    public string Precipitation_sum { get; set; } = string.Empty;
    public string Precipitation_hours { get; set; } = string.Empty;
    public string Sunrise { get; set; } = string.Empty;
    public string Sunset { get; set; } = string.Empty;
    public string Uv_index_max { get; set; } = string.Empty;
    public string Windgusts_10m_max { get; set; } = string.Empty;
}

public class Daily
{
    public List<string> Time { get; set; } = new();
    public List<double> Temperature_2m_max { get; set; } = new();
    public List<double> Temperature_2m_min { get; set; } = new();
    public List<double> Apparent_temperature_max { get; set; } = new();
    public List<double> Apparent_temperature_min { get; set; } = new();
    public List<double> Precipitation_sum { get; set; } = new();
    public List<double> Precipitation_hours { get; set; } = new();
    public List<string> Sunrise { get; set; } = new();
    public List<string> Sunset { get; set; } = new();
    public List<double> Uv_index_max { get; set; } = new();
    public List<double> Windgusts_10m_max { get; set; } = new();
}
