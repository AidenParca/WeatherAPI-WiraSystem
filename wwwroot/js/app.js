// Initialize map
let map = null;
let marker = null;

function initMap(latitude, longitude) {
    if (map) {
        map.setView([latitude, longitude], 13);
        if (marker) {
            marker.setLatLng([latitude, longitude]);
        } else {
            marker = L.marker([latitude, longitude]).addTo(map);
        }
    } else {
        map = L.map('map').setView([latitude, longitude], 13);
        L.tileLayer('https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png', {
            attribution: '© OpenStreetMap contributors'
        }).addTo(map);
        marker = L.marker([latitude, longitude]).addTo(map);
    }
}

function displayWeather(data, locationName = '') {
    if (!data) {
        console.error('No data received');
        handleError(null, 'No weather data received from server.');
        return;
    }
    
    console.log('Displaying weather data:', data);
    
    const location = locationName || (data.coordinates && data.coordinates.latitude && data.coordinates.longitude 
        ? `Lat: ${data.coordinates.latitude.toFixed(4)}, Lon: ${data.coordinates.longitude.toFixed(4)}`
        : 'Unknown location');
    
    // Update map
    if (data.coordinates && data.coordinates.latitude && data.coordinates.longitude) {
        initMap(data.coordinates.latitude, data.coordinates.longitude);
    }
    
    // Get AQI description and badge color
    const aqiDescriptions = {
        1: { text: 'Good', color: 'success' },
        2: { text: 'Fair', color: 'info' },
        3: { text: 'Moderate', color: 'warning' },
        4: { text: 'Poor', color: 'danger' },
        5: { text: 'Very Poor', color: 'dark' }
    };
    const aqiInfo = aqiDescriptions[data.aqi] || { text: 'Unknown', color: 'secondary' };
    
    document.getElementById('weatherInfo').innerHTML = `
        <div class="card">
            <div class="card-header">
                <h2 class="mb-0">Weather ${locationName ? 'in ' + locationName : 'at your location'}</h2>
            </div>
            <div class="card-body">
                <div class="row">
                    <div class="col-md-6">
                        <h3 class="h5 mb-3">Basic Information</h3>
                        <div class="mb-3">
                            <p class="mb-1"><strong>Temperature:</strong> <span class="text-primary">${data.temperature}°C</span></p>
                            <p class="mb-1"><strong>Humidity:</strong> <span class="text-info">${data.humidity}%</span></p>
                            <p class="mb-1"><strong>Wind Speed:</strong> <span class="text-secondary">${data.windSpeed} m/s</span></p>
                            <p class="mb-0"><strong>Location:</strong> <small class="text-muted">${location}</small></p>
                        </div>
                    </div>
                    <div class="col-md-6">
                        <h3 class="h5 mb-3">Air Quality</h3>
                        <div class="mb-2">
                            <p class="mb-0"><strong>AQI:</strong> <span class="badge bg-${aqiInfo.color}">${data.aqi} (${aqiInfo.text})</span></p>
                        </div>
                        ${data.pollutants ? `
                            <div class="small">
                                <p class="mb-1"><strong>PM2.5:</strong> ${data.pollutants.pm2_5} μg/m³</p>
                                <p class="mb-1"><strong>CO:</strong> ${data.pollutants.co} μg/m³</p>
                                <p class="mb-1"><strong>NO:</strong> ${data.pollutants.no} μg/m³</p>
                                <p class="mb-1"><strong>NO₂:</strong> ${data.pollutants.no2} μg/m³</p>
                                <p class="mb-1"><strong>O₃:</strong> ${data.pollutants.o3} μg/m³</p>
                                <p class="mb-0"><strong>SO₂:</strong> ${data.pollutants.so2} μg/m³</p>
                            </div>
                        ` : ''}
                    </div>
                </div>
            </div>
        </div>
    `;
}

function handleError(error, message) {
    console.error('Error:', error);
    document.getElementById('weatherInfo').innerHTML = `
        <div class="alert alert-danger" role="alert">
            <strong>Error:</strong> ${message}
        </div>
    `;
}

// Wait for DOM to be ready
document.addEventListener('DOMContentLoaded', () => {
    const cityButton = document.getElementById('getWeatherByCity');
    const locationButton = document.getElementById('getWeatherByLocation');
    
    if (!cityButton || !locationButton) {
        console.error('Could not find required buttons in the DOM');
        return;
    }
    
    console.log('Event listeners attaching...');

cityButton.addEventListener('click', () => {
    const city = document.getElementById('cityInput').value.trim();
    if (!city) {
        alert('Please enter a city name');
        return;
    }
    
    console.log('Fetching weather for city:', city);
    
    // Show loading state
    document.getElementById('weatherInfo').innerHTML = `
        <div class="alert alert-info" role="alert">
            <strong>Loading...</strong> Fetching weather data for ${city}...
        </div>
    `;
    
    fetch(`/weather/${encodeURIComponent(city)}`)
        .then(response => {
            console.log('Response status:', response.status, response.statusText);
            if (!response.ok) {
                return response.text().then(text => {
                    console.error('Error response:', text);
                    throw new Error(text || `HTTP error! status: ${response.status}`);
                });
            }
            return response.json();
        })
        .then(data => {
            console.log('Weather data received:', data);
            displayWeather(data, city);
        })
        .catch(error => {
            console.error('Fetch error:', error);
            handleError(error, `Could not fetch weather data. ${error.message || 'Please check if the city name is correct and the server is running.'}`);
        });
});

locationButton.addEventListener('click', () => {
    if (navigator.geolocation) {
        // Show loading state
        document.getElementById('weatherInfo').innerHTML = `
            <div class="alert alert-info" role="alert">
                <strong>Loading...</strong> Getting your location...
            </div>
        `;
        
        navigator.geolocation.getCurrentPosition(
            position => {
                const latitude = position.coords.latitude;
                const longitude = position.coords.longitude;
                
                console.log('Fetching weather for coordinates:', latitude, longitude);
                
                document.getElementById('weatherInfo').innerHTML = `
                    <div class="alert alert-info" role="alert">
                        <strong>Loading...</strong> Fetching weather data for your location...
                    </div>
                `;

                fetch(`/weather/coordinates?latitude=${latitude}&longitude=${longitude}`)
                    .then(response => {
                        console.log('Response status:', response.status, response.statusText);
                        if (!response.ok) {
                            return response.text().then(text => {
                                console.error('Error response:', text);
                                throw new Error(text || `HTTP error! status: ${response.status}`);
                            });
                        }
                        return response.json();
                    })
                    .then(data => {
                        console.log('Weather data received:', data);
                        displayWeather(data);
                    })
                    .catch(error => {
                        console.error('Fetch error:', error);
                        handleError(error, `Could not fetch weather data for your location. ${error.message || ''}`);
                    });
            },
            error => {
                console.error('Geolocation error:', error);
                handleError(error, 'Could not get your location. Please enable geolocation permissions.');
            }
        );
    } else {
        alert('Geolocation is not supported by this browser.');
    }
});

}); // End of DOMContentLoaded
