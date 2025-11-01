using System.Collections.Generic;
using System.Threading.Tasks;
using WeatherApi.Models;

namespace WeatherApi.Services
{
    public interface IWeatherService
    {
        Task<WeatherResponse> GetWeatherByCityAsync(string city);
        Task<WeatherResponse> GetWeatherByCoordinatesAsync(double latitude, double longitude);
    }
}
