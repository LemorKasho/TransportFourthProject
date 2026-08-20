using Microsoft.EntityFrameworkCore;
using TransportFourthProject.Api.Models;

namespace TransportFourthProject.Api.Data.Seed
{
    public class BusTypeSeed
    {
        public static void Seed(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<BusType>().HasData(
                new BusType { Id = 1, Type = "Standard", Capacity = 30 },
                new BusType { Id = 2, Type = "VIP", Capacity = 15 }
            );
        }
    }
}