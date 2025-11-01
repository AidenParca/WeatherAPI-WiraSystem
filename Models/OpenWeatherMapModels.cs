
namespace WeatherApi.Models.OpenWeatherMap
{
    // Models for Current Weather API response
    public class WeatherData
    {
        public Coord? Coord { get; set; }
        public Main? Main { get; set; }
        public Wind? Wind { get; set; }
    }

    public class Coord
    {
        public double Lon { get; set; }
        public double Lat { get; set; }
    }

    public class Main
    {
        public double Temp { get; set; }
        public int Humidity { get; set; }
    }

    public class Wind
    {
        public double Speed { get; set; }
    }

    // Models for Air Pollution API response
    public class AirPollutionData
    {
        public System.Collections.Generic.List<AirPollutionInfo>? List { get; set; }
    }

    public class AirPollutionInfo
    {
        public MainAqi? Main { get; set; }
        public Components? Components { get; set; }
    }

    public class MainAqi
    {
        public int Aqi { get; set; }
    }

    public class Components
    {
        public double Co { get; set; }
        public double No { get; set; }
        public double No2 { get; set; }
        public double O3 { get; set; }
        public double So2 { get; set; }
        public double Pm2_5 { get; set; }
    }
}
