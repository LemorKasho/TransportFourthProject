using Microsoft.EntityFrameworkCore;
using TransportFourthProject.Api.Models;

namespace TransportFourthProject.Api.Data.Seed
{
    public class UserDiscountSeed
    {
        public static void Seed(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserDiscount>().HasData(
                new UserDiscount
                {
                    Id = 1,
                    Name = "Student discount",
                    Percentage = 20
                },
                new UserDiscount
                {
                    Id = 2,
                    Name = "Engineer discount",
                    Percentage = 40
                },
                new UserDiscount
                {
                    Id = 3,
                    Name = "Patient discount",
                    Percentage = 50
                }
            );
        }
    }
}
