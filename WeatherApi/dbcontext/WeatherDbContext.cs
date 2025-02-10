
using Microsoft.EntityFrameworkCore;
using WeatherApi.models;

namespace WeatherApi.dbcontext
{
    public class WeatherDbContext : DbContext
    {
         public DbSet<WeatherData> Weather { get; set; }

    public WeatherDbContext(DbContextOptions<WeatherDbContext> options) : base(options) { }
    }
}