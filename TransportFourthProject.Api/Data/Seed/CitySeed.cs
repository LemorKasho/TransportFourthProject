using Microsoft.EntityFrameworkCore;
using TransportFourthProject.Api.Models;

namespace TransportFourthProject.Api.Data.Seed
{
    public class CitySeed
    {
        public static void Seed(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<City>().HasData(
                new City { Id = 1, Name = "Allepo" },
                new City { Id = 2, Name = "Damascus" },
                new City { Id = 3, Name = "Homs" }
            );
        }
    }
}