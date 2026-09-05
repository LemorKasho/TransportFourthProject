using Microsoft.EntityFrameworkCore;
using TransportFourthProject.Api.Data;

namespace TransportFourthProject.Api.Services
{
    public class TripArrivalService : BackgroundService
    {
        private readonly IServiceScopeFactory _scopeFactory;

        public TripArrivalService(IServiceScopeFactory scopeFactory)
        {
            _scopeFactory = scopeFactory;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                using (var scope = _scopeFactory.CreateScope())
                {
                    var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

                    var now = DateTime.Now;

                    var trips = await context.Trips
                        .Where(t => !t.IsDeleted &&
                                    t.IsArrived == false &&
                                    t.ArrivalTime <= now)
                        .ToListAsync();
                    foreach (var trip in trips)
                    {
                        trip.IsArrived = true;
                    }

                    await context.SaveChangesAsync();
                }
                await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);
            }
        }
    }
}