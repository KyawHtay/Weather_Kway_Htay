using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WeatherApi.models.interfaces
{
    public interface IWeatherRepository
    {
        Task<IEnumerable<WeatherData>> GetAllAsync();
        Task<WeatherData> GetByCityAsync(string city);
        Task AddAsync(WeatherData weatherData);
        Task SaveChangesAsync();
    }
}