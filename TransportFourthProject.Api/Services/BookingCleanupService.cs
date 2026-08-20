using TransportFourthProject.Api.Data;
using TransportFourthProject.Api.Enums;
using Microsoft.EntityFrameworkCore;

public class BookingCleanupService : BackgroundService
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly ILogger<BookingCleanupService> _logger;

    public BookingCleanupService(IServiceScopeFactory scopeFactory, 
                                ILogger<BookingCleanupService> logger)
    {
        _scopeFactory = scopeFactory;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                using var scope = _scopeFactory.CreateScope();
                var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

                var expiredBookings = await context.Bookings
                    .Where(b => b.Status == BookingStatus.PendingPayment &&
                                b.ExpirationTime < DateTime.Now)
                    .ToListAsync(stoppingToken);

                foreach (var booking in expiredBookings)
                {
                    booking.Status = BookingStatus.Cancelled;
                    booking.SeatStatus = SeatStatus.Available;
                }

                if (expiredBookings.Any())
                {
                    await context.SaveChangesAsync(stoppingToken);
                    _logger.LogInformation($"Cleaned {expiredBookings.Count} expired bookings at {DateTime.Now}");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while cleaning expired bookings");
            }

            await Task.Delay(TimeSpan.FromSeconds(30), stoppingToken);
        }
    }
}