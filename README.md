# Weather API
![alt text](https://github.com/AidenParca/WeatherAPI-WiraSystem/blob/main/Screenshot%202025-11-01%20172041.png)
A .NET Core Web API application that provides weather and air quality information for cities using the OpenWeatherMap API.

## Features

- **Weather Information**: Get current temperature, humidity, and wind speed for any city
- **Air Quality Data**: Retrieve Air Quality Index (AQI) and major pollutants (PM2.5, CO, NO, NO₂, O₃, SO₂)
- **Geographic Coordinates**: Returns latitude and longitude for the queried location
- **Web Interface**: User-friendly web UI to interact with the API
- **Multiple Input Methods**: Search by city name or use browser geolocation

## Requirements

- .NET 9.0 SDK
- OpenWeatherMap API key (free tier available at https://openweathermap.org/api)

## Setup Instructions

1. **Clone the repository**
   ```bash
   git clone <repository-url>
   cd WatherApi
   ```

2. **Configure API Key**
   
   Open `appsettings.json` and add your OpenWeatherMap API key:
   ```json
   {
     "OpenWeatherMap": {
       "ApiKey": "your-api-key-here"
     }
   }
   ```

3. **Run the Application**
   ```bash
   dotnet run
   ```

4. **Access the Application**
   
   - Web UI: Navigate to `http://localhost:5148` or `https://localhost:7271`
   - Swagger UI: `https://localhost:7271/swagger` (in development mode)
   - API Endpoints:
     - `GET /weather/{city}` - Get weather by city name
     - `GET /weather/coordinates?latitude={lat}&longitude={lon}` - Get weather by coordinates

## Running Tests

To run the unit tests:

```bash
dotnet test
```

## API Endpoints

### Get Weather by City

**Request:**
```
GET /weather/{city}
```

**Example:**
```
GET /weather/Tehran
```

**Response:**
```json
{
  "temperature": 25.5,
  "humidity": 60,
  "windSpeed": 3.5,
  "aqi": 2,
  "coordinates": {
    "latitude": 35.6892,
    "longitude": 51.3890
  },
  "pollutants": {
    "pm2_5": 15.5,
    "co": 200.0,
    "no": 10.0,
    "no2": 25.0,
    "o3": 80.0,
    "so2": 5.0
  }
}
```

### Get Weather by Coordinates

**Request:**
```
GET /weather/coordinates?latitude={lat}&longitude={lon}
```

**Example:**
```
GET /weather/coordinates?latitude=35.6892&longitude=51.3890
```

**Response:** Same format as above.

## Project Structure

```
WatherApi/
├── Controllers/
│   └── WeatherController.cs      # API endpoints
├── Models/
│   ├── WeatherResponse.cs        # Response DTOs
│   └── OpenWeatherMapModels.cs   # External API models
├── Services/
│   ├── IWeatherService.cs        # Service interface
│   └── WeatherService.cs         # Business logic
├── WeatherApi.Tests/
│   └── WeatherServiceTests.cs     # Unit tests
├── wwwroot/
│   ├── index.html                # Web UI
│   └── js/
│       └── app.js                # Frontend logic
└── Program.cs                     # Application entry point
```

## Technologies Used

- **.NET 9.0** - Framework
- **ASP.NET Core Web API** - API framework
- **HttpClient** - HTTP client for external API calls
- **Swagger/OpenAPI** - API documentation
- **Bootstrap 5** - Frontend styling
- **Leaflet** - Interactive maps
- **xUnit** - Testing framework
- **Moq** - Mocking framework for tests

## Acceptance Criteria

✅ Returns weather information for known cities (e.g., "Tehran")  
✅ Includes all required data:
- Temperature (in Celsius)
- Humidity (in %)
- Wind Speed (in meters per second)
- Air Quality Index (AQI)
- Major pollutants (PM2.5, CO, NO, NO₂, O₃, SO₂)
- Geographical coordinates (Latitude/Longitude)

✅ Code compiles and runs in one step  
✅ Clean code following Clean Code principles  
✅ At least one unit test included

## Notes

- The application uses metric units (Celsius, meters per second)
- Air Quality Index (AQI) values range from 1-5:
  - 1 = Good
  - 2 = Fair
  - 3 = Moderate
  - 4 = Poor
  - 5 = Very Poor
- The web UI includes an interactive map showing the location

## License

This project is part of a technical assessment.

