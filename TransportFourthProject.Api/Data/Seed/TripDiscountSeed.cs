using Microsoft.EntityFrameworkCore;
using TransportFourthProject.Api.Models;

namespace TransportFourthProject.Api.Data.Seed
{
    public class TripDiscountSeed
    {
        public static void Seed(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TripDiscount>().HasData(
                new TripDiscount
                {
                    Id = 1,
                    Name = "Adha-Aid discount",
                    Percentage = 10
                },
                new TripDiscount
                {
                    Id = 2,
                    Name = "Christmas discount",
                    Percentage = 20
                },
                new TripDiscount
                {
                    Id = 3,
                    Name = "Labor day discount",
                    Percentage = 30
                }
            );
        }
    }
}
