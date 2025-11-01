using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using WeatherApi.Models;
using WeatherApi.Models.OpenWeatherMap;

namespace WeatherApi.Services
{
    public class WeatherService : IWeatherService
    {
        private readonly HttpClient _httpClient;
        private readonly string? _apiKey;
        private readonly JsonSerializerOptions _jsonSerializerOptions;

        public WeatherService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _apiKey = configuration["OpenWeatherMap:ApiKey"];
            if (string.IsNullOrEmpty(_apiKey))
            {
                throw new InvalidOperationException("OpenWeatherMap API key is not configured.");
            }
            _jsonSerializerOptions = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
        }

        public async Task<WeatherResponse> GetWeatherByCityAsync(string city)
        {
            // 1. Get weather data and coordinates
            var weatherResponse = await _httpClient.GetAsync($"https://api.openweathermap.org/data/2.5/weather?q={city}&appid={_apiKey}&units=metric");
            weatherResponse.EnsureSuccessStatusCode();

            var weatherContent = await weatherResponse.Content.ReadAsStringAsync();
            var weatherData = JsonSerializer.Deserialize<WeatherData>(weatherContent, _jsonSerializerOptions);

            if (weatherData?.Coord == null)
            {
                throw new InvalidOperationException("Could not retrieve coordinates for the specified city.");
            }

            var lat = weatherData.Coord.Lat;
            var lon = weatherData.Coord.Lon;

            // 2. Get air pollution data
            var pollutionResponse = await _httpClient.GetAsync($"http://api.openweathermap.org/data/2.5/air_pollution?lat={lat}&lon={lon}&appid={_apiKey}");
            pollutionResponse.EnsureSuccessStatusCode();

            var pollutionContent = await pollutionResponse.Content.ReadAsStringAsync();
            var pollutionData = JsonSerializer.Deserialize<AirPollutionData>(pollutionContent, _jsonSerializerOptions);

            if (pollutionData?.List == null || pollutionData.List.Count == 0)
            {
                throw new InvalidOperationException("Could not retrieve air pollution data.");
            }

            var pollutionInfo = pollutionData.List[0];

            // 3. Map to WeatherResponse
            var result = new WeatherResponse
            {
                Temperature = weatherData.Main?.Temp ?? 0,
                Humidity = weatherData.Main?.Humidity ?? 0,
                WindSpeed = weatherData.Wind?.Speed ?? 0,
                Coordinates = new Models.Coordinates
                {
                    Latitude = lat,
                    Longitude = lon
                },
                Aqi = pollutionInfo.Main?.Aqi ?? 0,
                Pollutants = new Models.Pollutants
                {
                    Co = pollutionInfo.Components?.Co ?? 0,
                    No = pollutionInfo.Components?.No ?? 0,
                    No2 = pollutionInfo.Components?.No2 ?? 0,
                    O3 = pollutionInfo.Components?.O3 ?? 0,
                    So2 = pollutionInfo.Components?.So2 ?? 0,
                    Pm2_5 = pollutionInfo.Components?.Pm2_5 ?? 0
                }
            };

            return result;
        }

        public async Task<WeatherResponse> GetWeatherByCoordinatesAsync(double latitude, double longitude)
        {
            // 1. Get weather data and coordinates
            var weatherResponse = await _httpClient.GetAsync($"https://api.openweathermap.org/data/2.5/weather?lat={latitude}&lon={longitude}&appid={_apiKey}&units=metric");
            weatherResponse.EnsureSuccessStatusCode();

            var weatherContent = await weatherResponse.Content.ReadAsStringAsync();
            var weatherData = JsonSerializer.Deserialize<WeatherData>(weatherContent, _jsonSerializerOptions);

            if (weatherData?.Coord == null)
            {
                throw new InvalidOperationException("Could not retrieve coordinates for the specified location.");
            }

            var lat = weatherData.Coord.Lat;
            var lon = weatherData.Coord.Lon;

            // 2. Get air pollution data
            var pollutionResponse = await _httpClient.GetAsync($"http://api.openweathermap.org/data/2.5/air_pollution?lat={lat}&lon={lon}&appid={_apiKey}");
            pollutionResponse.EnsureSuccessStatusCode();

            var pollutionContent = await pollutionResponse.Content.ReadAsStringAsync();
            var pollutionData = JsonSerializer.Deserialize<AirPollutionData>(pollutionContent, _jsonSerializerOptions);

            if (pollutionData?.List == null || pollutionData.List.Count == 0)
            {
                throw new InvalidOperationException("Could not retrieve air pollution data.");
            }

            var pollutionInfo = pollutionData.List[0];

            // 3. Map to WeatherResponse
            var result = new WeatherResponse
            {
                Temperature = weatherData.Main?.Temp ?? 0,
                Humidity = weatherData.Main?.Humidity ?? 0,
                WindSpeed = weatherData.Wind?.Speed ?? 0,
                Coordinates = new Models.Coordinates
                {
                    Latitude = lat,
                    Longitude = lon
                },
                Aqi = pollutionInfo.Main?.Aqi ?? 0,
                Pollutants = new Models.Pollutants
                {
                    Co = pollutionInfo.Components?.Co ?? 0,
                    No = pollutionInfo.Components?.No ?? 0,
                    No2 = pollutionInfo.Components?.No2 ?? 0,
                    O3 = pollutionInfo.Components?.O3 ?? 0,
                    So2 = pollutionInfo.Components?.So2 ?? 0,
                    Pm2_5 = pollutionInfo.Components?.Pm2_5 ?? 0
                }
            };

            return result;
        }
    }
}
