using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WeatherApi.dbcontext;
using WeatherApi.models;
using WeatherApi.models.interfaces;

namespace WeatherApi.data.interfaces
{
    public class WeatherRepository : IWeatherRepository
    {
        private readonly WeatherDbContext _context;

        public WeatherRepository(WeatherDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<WeatherData>> GetAllAsync()
        {
            return await _context.Weather.ToListAsync();
        }

        public async Task<WeatherData> GetByCityAsync(string city)
        {
            var weatherData = await _context.Weather.FirstOrDefaultAsync(w => w.City == city);
            if (weatherData == null)
            {
                throw new InvalidOperationException($"No weather data found for city: {city}");
            }
            return weatherData;
        }

        public async Task AddAsync(WeatherData weatherData)
        {
            await _context.Weather.AddAsync(weatherData);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}