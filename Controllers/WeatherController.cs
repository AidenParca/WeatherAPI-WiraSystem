using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WeatherApi.Services;

namespace WeatherApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherController : ControllerBase
    {
        private readonly IWeatherService _weatherService;

        public WeatherController(IWeatherService weatherService)
        {
            _weatherService = weatherService;
        }

        [HttpGet("{city}")]
        public async Task<IActionResult> Get(string city)
        {
            try
            {
                var weather = await _weatherService.GetWeatherByCityAsync(city);
                return Ok(weather);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
                return StatusCode(500, "An error occurred while fetching weather data.");
            }
        }

        [HttpGet("coordinates")]
        public async Task<IActionResult> GetByCoordinates([FromQuery] double latitude, [FromQuery] double longitude)
        {
            try
            {
                var weather = await _weatherService.GetWeatherByCoordinatesAsync(latitude, longitude);
                return Ok(weather);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
                return StatusCode(500, "An error occurred while fetching weather data by coordinates.");
            }
        }
    }
}
