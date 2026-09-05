using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using TransportFourthProject.Api.Data.Seed;
using TransportFourthProject.Api.Models;

namespace TransportFourthProject.Api.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<City> Cities { get; set; }
        public DbSet<BusType> BusTypes { get; set; }
        public DbSet<RoutePrice> RoutePrices { get; set; }
        public DbSet<Bus> Buses { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<TripDiscount> TripDiscounts { get; set; }
        public DbSet<Trip> Trips { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<UserDiscountTicket> UserDiscountTickets { get; set; }
        public DbSet<UserDiscount> UserDiscounts { get; set; }
        public DbSet<Booking> Bookings { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<PaymentAttempt> PaymentAttempts { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            #region for relationship with Booking and User
            modelBuilder.Entity<Booking>()
                .HasOne(b => b.User)
                .WithMany(u => u.Bookings)
                .HasForeignKey(b => b.UserId)
                .OnDelete(DeleteBehavior.Restrict);
            #endregion

            #region for relationship with Booking and Trip
            modelBuilder.Entity<Booking>()
                .HasOne(b => b.Trip)
                .WithMany(t => t.Bookings)
                .HasForeignKey(b => b.TripId)
                .OnDelete(DeleteBehavior.Restrict);
            #endregion

            #region for relationship with Booking and UserDiscountTicket
            modelBuilder.Entity<Booking>()
                .HasOne(b => b.UserDiscountTicket)
                .WithMany(udt => udt.Bookings)
                .HasForeignKey(b => b.UserDiscountTicketId)
                .OnDelete(DeleteBehavior.Restrict);
            #endregion

            #region for relationship with UserDiscountTicket and User
            modelBuilder.Entity<UserDiscountTicket>()
                .HasOne(udt => udt.User)
                .WithMany(u => u.UserDiscountTickets)
                .HasForeignKey(udt => udt.UserId)
                .OnDelete(DeleteBehavior.Restrict);
            #endregion

            #region for relationship with UserDiscountTicket and UserDiscount
            modelBuilder.Entity<UserDiscountTicket>()
                .HasOne(udt => udt.UserDiscount)
                .WithMany(ud => ud.UserDiscountTickets)
                .HasForeignKey(udt => udt.DiscountId)
                .OnDelete(DeleteBehavior.Restrict);
            #endregion

            #region for relationship with Booking and Payment
            modelBuilder.Entity<Booking>()
                .HasOne(b => b.Payment)
                .WithOne(p => p.Booking)
                .HasForeignKey<Booking>(b => b.PaymentId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Payment>()
                .HasOne(p => p.Booking)
                .WithOne(b => b.Payment)
                .HasForeignKey<Payment>(p => p.BookingId)
                .OnDelete(DeleteBehavior.Restrict);


            #endregion

            #region for relationship with Payment and PaymentAttempt
            modelBuilder.Entity<PaymentAttempt>()
                .HasOne(pa => pa.Payment)
                .WithMany(p => p.Attempts)
                .HasForeignKey(pa => pa.PaymentId)
                .OnDelete(DeleteBehavior.Restrict);
            #endregion

            #region for relationship with Trip and TripDiscount
            modelBuilder.Entity<Trip>()
                .HasOne(t => t.TripDiscount)
                .WithMany(td => td.Trips)
                .HasForeignKey(t => t.TripDiscountId)
                .OnDelete(DeleteBehavior.Restrict);
            #endregion

            #region for relationship with Trip and Employee
            modelBuilder.Entity<Trip>()
                .HasOne(t => t.Employee)
                .WithMany(e => e.Trips)
                .HasForeignKey(t => t.EmployeeId)
                .OnDelete(DeleteBehavior.Restrict);
            #endregion

            #region for relationship with Trip and Bus
            modelBuilder.Entity<Trip>()
                .HasOne(t => t.Bus)
                .WithMany(b => b.Trips)
                .HasForeignKey(t => t.BusId)
                .OnDelete(DeleteBehavior.Restrict);
            #endregion

            #region for relationship with Trip and RoutePrice
            modelBuilder.Entity<Trip>()
                .HasOne(t => t.RoutePrice)
                .WithMany(r => r.Trips)
                .HasForeignKey(t => t.RoutePriceId)
                .OnDelete(DeleteBehavior.Restrict);
            #endregion

            #region for relationship with RoutePrice and City (StartCity)
            modelBuilder.Entity<RoutePrice>()
                .HasOne(r => r.StartCity)
                .WithMany(c => c.StartCityRoutePrices)
                .HasForeignKey(r => r.StartCityId)
                .OnDelete(DeleteBehavior.Restrict);
            #endregion

            #region for relationship with RoutePrice and City (EndCity)
            modelBuilder.Entity<RoutePrice>()
                .HasOne(r => r.EndCity)
                .WithMany(c => c.EndCityRoutePrices)
                .HasForeignKey(r => r.EndCityId)
                .OnDelete(DeleteBehavior.Restrict);
            #endregion

            #region for relationship with RoutePrice and BusType
            modelBuilder.Entity<RoutePrice>()
                .HasOne(r => r.BusType)
                .WithMany(bt => bt.RoutePrices)
                .HasForeignKey(r => r.BusTypeId)
                .OnDelete(DeleteBehavior.Restrict);
            #endregion

            #region for relationship with Bus and BusType
            modelBuilder.Entity<Bus>()
                .HasOne(b => b.BusType)
                .WithMany(bt => bt.Buses)
                .HasForeignKey(b => b.BusTypeId)
                .OnDelete(DeleteBehavior.Restrict);
            #endregion

            modelBuilder.Seed();

            modelBuilder.Entity<RoutePrice>()
                .Property(r => r.Price)
                .HasColumnType("decimal(10,2)");

            modelBuilder.Entity<Booking>()
                .Property(b => b.FinalPrice)
                .HasColumnType("decimal(10,2)");

            modelBuilder.Entity<Payment>()
                .Property(p => p.Amount)
                .HasColumnType("decimal(10,2)");

            modelBuilder.Entity<Employee>()
                .Property(e => e.Salary)
                .HasColumnType("decimal(10,2)");

            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                foreach (var property in entityType.GetProperties())
                {
                    var clrType = property.ClrType;

                    if (clrType.IsGenericType && clrType.GetGenericTypeDefinition() == typeof(Nullable<>))
                    {
                        clrType = Nullable.GetUnderlyingType(clrType);
                    }

                    if (clrType != null && clrType.IsEnum)
                    {
                        var converterType = typeof(EnumToStringConverter<>)
                            .MakeGenericType(clrType);

                        var converter = (ValueConverter)
                            Activator.CreateInstance(converterType)!;

                        property.SetValueConverter(converter);
                    }
                }
            }

            //unique IdempotencyKey for Payment
            modelBuilder.Entity<Payment>()
                .HasIndex(p => p.IdempotencyKey)
                .IsUnique();

            //unique IdempotencyKey for PaymentAttempt
            modelBuilder.Entity<PaymentAttempt>()
                .HasIndex(pa => pa.IdempotencyKey)
                .IsUnique();

            //unique index for phone
            modelBuilder.Entity<User>()
                .HasIndex(u => u.Phone)
                .IsUnique();

            //unique index for national number
            modelBuilder.Entity<User>()
                .HasIndex(u => u.NationalNumber)
                .IsUnique();
            //unique index for national number
            modelBuilder.Entity<Employee>()
                .HasIndex(e => e.NationalNumber)
                .IsUnique();

            modelBuilder.Entity<Bus>()
                .HasIndex(b => b.BusNumber)
                .IsUnique();
        }
    }
}
