using Microsoft.EntityFrameworkCore;
using TransportFourthProject.Api.Models;

namespace TransportFourthProject.Api.Data.Seed
{
    public class RoutePriceSeed
    {
        public static void Seed(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<RoutePrice>().HasData(
                new RoutePrice
                {
                    Id = 1,
                    BusTypeId = 1,
                    StartCityId = 1,
                    EndCityId = 2,
                    Price = 1200,
                    DurationHours = 5
                },
                new RoutePrice
                {
                    Id = 2,
                    BusTypeId = 1,
                    StartCityId = 2,
                    EndCityId = 1,
                    Price = 1200,
                    DurationHours = 5
                },
                new RoutePrice
                {
                    Id = 3,
                    BusTypeId = 1,
                    StartCityId = 3,
                    EndCityId = 1,
                    Price = 800,
                    DurationHours = 3
                },
                new RoutePrice
                {
                    Id = 4,
                    BusTypeId = 1,
                    StartCityId = 1,
                    EndCityId = 3,
                    Price = 800,
                    DurationHours = 3
                },
                new RoutePrice
                {
                    Id = 5,
                    BusTypeId = 2,
                    StartCityId = 1,
                    EndCityId = 2,
                    Price = 1500,
                    DurationHours = 5
                 },
                new RoutePrice
                {
                    Id = 6,
                    BusTypeId = 2,
                    StartCityId = 2,
                    EndCityId = 1,
                    Price = 1500,
                    DurationHours = 5
                },
                new RoutePrice
                {
                    Id = 7,
                    BusTypeId = 2,
                    StartCityId = 3,
                    EndCityId = 1,
                    Price = 1000,
                    DurationHours = 3
                },
                new RoutePrice
                {
                    Id = 8,
                    BusTypeId = 2,
                    StartCityId = 1,
                    EndCityId = 3,
                    Price = 1000,
                    DurationHours = 3
                }
            );
        }
    }
}
