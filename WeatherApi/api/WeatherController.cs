using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WeatherApi.dbcontext;
using WeatherApi.models;
using WeatherApi.models.interfaces;

namespace WeatherApi.api
{
    [ApiController]
    [Route("api/weather")]
    public class WeatherController : Controller
    {
        private readonly IWeatherRepository _repository;
        private readonly IConfiguration _configuration;
        private readonly HttpClient _httpClient;

        public WeatherController(IWeatherRepository repository, HttpClient httpClient,IConfiguration configuration)
        {
            _repository = repository;
            _httpClient = httpClient;
            _configuration = configuration;;
        }

       [HttpGet("{city}")]
        public async Task<IActionResult> GetWeather(string city,string countrycode = "GB")
        {
            var weatherData = await FetchWeatherFromApi(city);
            if (weatherData == null)
                return NotFound();

            var weatherRecord = new WeatherData
            {
                City = city,
                Data = JsonSerializer.Serialize(weatherData)
            };

            await _repository.AddAsync(weatherRecord);
            await _repository.SaveChangesAsync();

            return Ok(weatherData);
        }

   private async Task<object> FetchWeatherFromApi(string city,string countrycode = "GB")
   {
     // Fetch API key from appsettings.json
    string apiKey = _configuration["OpenWeather:ApiKey"] ?? throw new InvalidOperationException("API key not found in configuration.");
    string Url = _configuration["OpenWeather:Url"] ?? throw new InvalidOperationException("API key not found in configuration.");
    // Fetch latitude and longitude from the OpenWeatherMap Geocoding API
    string geoApiUrl = $"{Url}/geo/1.0/direct?q={city},{countrycode}&appid={apiKey}";
    var geoResponse = await _httpClient.GetAsync(geoApiUrl);

    // Ensure successful response
    geoResponse.EnsureSuccessStatusCode();

    var geoJsonResponse = await geoResponse.Content.ReadAsStringAsync();
    var locationData = JsonSerializer.Deserialize<List<Location>>(geoJsonResponse);

    if (locationData == null || locationData.Count == 0)
    {
        throw new ArgumentException($"Could not find location data for city: {city}");
    }

    // Use the first result's latitude and longitude
    double latitude = locationData[0].Lat;
    double longitude = locationData[0].Lon;

    // Fetch weather data using latitude and longitude
    string weatherApiUrl = $"{Url}/data/3.0/onecall?lat={latitude}&lon={longitude}&exclude=hourly,minutely&appid={apiKey}";
    var weatherResponse = await _httpClient.GetAsync(weatherApiUrl);

    // Ensure successful response
    weatherResponse.EnsureSuccessStatusCode();

    var weatherJsonResponse = await weatherResponse.Content.ReadAsStringAsync();
    var weatherData = JsonSerializer.Deserialize<object>(weatherJsonResponse);

    if (weatherData == null)
    {
        throw new InvalidOperationException("Failed to deserialize weather data.");
    }

    return weatherData;
}


 }
}