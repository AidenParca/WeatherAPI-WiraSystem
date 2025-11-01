using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Moq;
using Moq.Protected;
using WeatherApi.Models;
using WeatherApi.Models.OpenWeatherMap;
using WeatherApi.Services;
using Xunit;

namespace WeatherApi.Tests
{
    public class WeatherServiceTests
    {
        private readonly Mock<IConfiguration> _mockConfiguration;
        private readonly Mock<HttpMessageHandler> _mockHttpMessageHandler;
        private readonly HttpClient _httpClient;
        private readonly WeatherService _weatherService;

        public WeatherServiceTests()
        {
            _mockConfiguration = new Mock<IConfiguration>();
            _mockConfiguration.Setup(c => c["OpenWeatherMap:ApiKey"]).Returns("test-api-key");

            _mockHttpMessageHandler = new Mock<HttpMessageHandler>();
            _httpClient = new HttpClient(_mockHttpMessageHandler.Object)
            {
                BaseAddress = new Uri("https://api.openweathermap.org/")
            };

            _weatherService = new WeatherService(_httpClient, _mockConfiguration.Object);
        }

        [Fact]
        public async Task GetWeatherByCityAsync_WithValidCity_ReturnsWeatherResponse()
        {
            // Arrange
            var city = "Tehran";
            var latitude = 35.6892;
            var longitude = 51.3890;

            // Mock weather API response
            var weatherData = new WeatherData
            {
                Coord = new Coord { Lat = latitude, Lon = longitude },
                Main = new Main { Temp = 25.5, Humidity = 60 },
                Wind = new Wind { Speed = 3.5 }
            };

            var weatherJson = JsonSerializer.Serialize(weatherData, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });

            // Mock air pollution API response
            var pollutionData = new AirPollutionData
            {
                List = new List<AirPollutionInfo>
                {
                    new AirPollutionInfo
                    {
                        Main = new MainAqi { Aqi = 2 },
                        Components = new Components
                        {
                            Pm2_5 = 15.5,
                            Co = 200.0,
                            No = 10.0,
                            No2 = 25.0,
                            O3 = 80.0,
                            So2 = 5.0
                        }
                    }
                }
            };

            var pollutionJson = JsonSerializer.Serialize(pollutionData, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });

            // Setup HTTP handler responses
            _mockHttpMessageHandler
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.Is<HttpRequestMessage>(req =>
                        req.RequestUri.ToString().Contains("weather") && req.RequestUri.ToString().Contains(city)),
                    ItExpr.IsAny<System.Threading.CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(weatherJson, Encoding.UTF8, "application/json")
                });

            _mockHttpMessageHandler
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.Is<HttpRequestMessage>(req =>
                        req.RequestUri.ToString().Contains("air_pollution")),
                    ItExpr.IsAny<System.Threading.CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(pollutionJson, Encoding.UTF8, "application/json")
                });

            // Act
            var result = await _weatherService.GetWeatherByCityAsync(city);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(25.5, result.Temperature);
            Assert.Equal(60, result.Humidity);
            Assert.Equal(3.5, result.WindSpeed);
            Assert.Equal(2, result.Aqi);
            Assert.NotNull(result.Coordinates);
            Assert.Equal(latitude, result.Coordinates.Latitude);
            Assert.Equal(longitude, result.Coordinates.Longitude);
            Assert.NotNull(result.Pollutants);
            Assert.Equal(15.5, result.Pollutants.Pm2_5);
            Assert.Equal(200.0, result.Pollutants.Co);
        }

        [Fact]
        public async Task GetWeatherByCityAsync_WithInvalidApiKey_ThrowsException()
        {
            // Arrange
            var mockConfig = new Mock<IConfiguration>();
            mockConfig.Setup(c => c["OpenWeatherMap:ApiKey"]).Returns((string?)null);

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(() =>
            {
                var service = new WeatherService(_httpClient, mockConfig.Object);
                return Task.CompletedTask;
            });
        }

        [Fact]
        public async Task GetWeatherByCityAsync_WithInvalidCity_ThrowsException()
        {
            // Arrange
            var city = "InvalidCity12345";

            _mockHttpMessageHandler
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<System.Threading.CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.NotFound
                });

            // Act & Assert
            await Assert.ThrowsAsync<HttpRequestException>(() =>
                _weatherService.GetWeatherByCityAsync(city));
        }
    }
}
