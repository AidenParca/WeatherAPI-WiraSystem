
namespace WeatherApi.Models
{
    public class WeatherResponse
    {
        public double Temperature { get; set; }
        public int Humidity { get; set; }
        public double WindSpeed { get; set; }
        public int Aqi { get; set; }
        public Pollutants? Pollutants { get; set; }
        public Coordinates? Coordinates { get; set; }
    }

    public class Pollutants
    {
        public double Pm2_5 { get; set; }
        public double Co { get; set; }
        public double No { get; set; }
        public double No2 { get; set; }
        public double O3 { get; set; }
        public double So2 { get; set; }
    }

    public class Coordinates
    {
        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }
}
