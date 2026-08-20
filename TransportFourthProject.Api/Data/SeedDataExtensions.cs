using Microsoft.EntityFrameworkCore;
namespace TransportFourthProject.Api.Data.Seed
{
    //static class because :extension method(this) لان عندي شي ثابت بيانات يعني ولا احتاج اوبجكت ونريد 
    //extension method : لاضافة دوال جديدة لاي كلاس بدون تعديل كلاس 
    // يعني تضيف ميزة جديدة لكلاس جاهز بدون وراثة و بدون فتح الملف الاصلي
    public static class SeedDataExtensions 
    {
        public static void Seed(this ModelBuilder modelBuilder)
        {
            CitySeed.Seed(modelBuilder);
            BusTypeSeed.Seed(modelBuilder);
            RoutePriceSeed.Seed(modelBuilder);
            TripDiscountSeed.Seed(modelBuilder);
            UserDiscountSeed.Seed(modelBuilder);
        }
    }
}
