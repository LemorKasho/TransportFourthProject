using Microsoft.AspNetCore.JsonPatch;
using Microsoft.EntityFrameworkCore;
using TransportFourthProject.Api.Data;
using TransportFourthProject.Api.DTOs.RoutePrice;
using TransportFourthProject.Api.Models;

namespace TransportFourthProject.Api.Repositories
{
    public class EmployeeRoutePriceRepository : Repository<RoutePrice>, IEmployeeRoutePriceRepository
    {
        private readonly AppDbContext _context;
        public EmployeeRoutePriceRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }
        public async Task<List<RoutePriceDto>> GetAllRoutePricesAsync()
        {
            return await _context.RoutePrices
                .Where(r => !r.IsDeleted)
                .Select(r => new RoutePriceDto
                {
                    RoutePriceId = r.Id,
                    BusTypeId = r.BusTypeId,
                    BusTypeName = r.BusType.Type,

                    StartCityId = r.StartCityId,
                    StartCity = r.StartCity.Name,

                    EndCityId = r.EndCityId,
                    EndCity = r.EndCity.Name,

                    Price = r.Price,
                    DurationHours = r.DurationHours,
                    DistanceKm = r.DistanceKm

                })
                .ToListAsync();
        }
        public async Task<List<RoutePriceDto>> GetDeletedRoutePricesAsync()
        {
            return await _context.RoutePrices
                .Where(r => r.IsDeleted)
                .Select(r => new RoutePriceDto
                {
                    RoutePriceId = r.Id,

                    BusTypeId = r.BusTypeId,
                    BusTypeName = r.BusType.Type,

                    StartCityId = r.StartCityId,
                    StartCity = r.StartCity.Name,

                    EndCityId = r.EndCityId,
                    EndCity = r.EndCity.Name,

                    Price = r.Price,
                    DurationHours = r.DurationHours,
                    DistanceKm = r.DistanceKm
                })
                .ToListAsync();
        }

        public async Task<string> PatchRoutePriceAsync(int id, JsonPatchDocument<RoutePrice> patchDoc)
        {
            var route = await _context.RoutePrices.FindAsync(id);

            if (route == null)
                return "Route not found";

            bool hasActiveTrips = await _context.Trips.AnyAsync(t =>
                t.RoutePriceId == id &&
                !t.IsDeleted &&
                (
                    t.DepartureTime > DateTime.Now ||
                    t.ArrivalTime > DateTime.Now
                )
            );

            if (hasActiveTrips)
                return "Cannot update route because it is linked to active or future trips";

            patchDoc.ApplyTo(route);

            foreach (var op in patchDoc.Operations)
            {
                if (op.path.ToLower() == "/price")
                {
                    if (route.Price <= 0)
                        return "Price must be greater than zero";
                }

                if (op.path.ToLower() == "/durationhours")
                {
                    if (route.DurationHours < 1 || route.DurationHours > 24)
                        return "Duration must be between 1 and 24 hours";
                }
                if (op.path.ToLower() == "/distancekm")
                {
                    if (route.DistanceKm < 1)
                        return "Distance must be greater than 1";
                }
            }

            await _context.SaveChangesAsync();
            return "Route updated successfully";
        }

        public async Task<string> DeleteRoutePriceAsync(int id)
        {
            var route = await _context.RoutePrices.FindAsync(id);

            if (route == null)
                return "Route not found";
            if (route.IsDeleted)
                return "Route is already deleted";

            bool hasActiveTrips = await _context.Trips.AnyAsync(t =>
                t.RoutePriceId == id &&
                !t.IsDeleted &&
                (
                    t.DepartureTime > DateTime.Now ||
                    t.ArrivalTime > DateTime.Now
                )
            );

            if (hasActiveTrips)
                return "Cannot delete route because it is linked to active or future trips";

            route.IsDeleted = true;

            await _context.SaveChangesAsync();
            return "Route deleted successfully";
        }

        public async Task<string> RestoreRoutePriceAsync(int id)
        {
            var route = await _context.RoutePrices.FindAsync(id);

            if (route == null)
                return "Route not found";

            if (!route.IsDeleted)
                return "Route is not deleted";

            route.IsDeleted = false;

            await _context.SaveChangesAsync();
            return "Route restored successfully";
        }

        public async Task<string> AddRoutePriceAsync(AddRoutePriceForEmployeeDto dto)
        {
            if (dto.StartCityId == dto.EndCityId)
                return "End city cannot be the same as start city";

            var startCity = await _context.Cities.FindAsync(dto.StartCityId);
            var endCity = await _context.Cities.FindAsync(dto.EndCityId);

            if (startCity == null || endCity == null)
                return "Start or End city not found";

            var busType = await _context.BusTypes.FindAsync(dto.BusTypeId);
            if (busType == null)
                return "Bus type not found";

            var existingRoute = await _context.RoutePrices
                .FirstOrDefaultAsync(r =>
                    r.BusTypeId == dto.BusTypeId &&
                    r.StartCityId == dto.StartCityId &&
                    r.EndCityId == dto.EndCityId
                );

            if (existingRoute != null)
            {
                if (existingRoute.IsDeleted)
                {
                    existingRoute.IsDeleted = false;
                    existingRoute.Price = dto.Price;
                    existingRoute.DurationHours = dto.DurationHours;
                    existingRoute.DistanceKm = dto.DistanceKm;

                    await _context.SaveChangesAsync();
                    return "Route restored and updated successfully";
                }

                return "Route already exists";
            }

            var newRoute = new RoutePrice
            {
                BusTypeId = dto.BusTypeId,
                StartCityId = dto.StartCityId,
                EndCityId = dto.EndCityId,
                Price = dto.Price,
                DurationHours = dto.DurationHours,
                DistanceKm = dto.DistanceKm,
                IsDeleted = false
            };

            await _context.RoutePrices.AddAsync(newRoute);
            await _context.SaveChangesAsync();

            return "Route added successfully";
        }

        public async Task<RoutePriceStatusesDto> GetRoutePriceStatusesAsync()
        {
            var total = await _context.RoutePrices.CountAsync();

            var deleted = await _context.RoutePrices
                .CountAsync(r => r.IsDeleted);

            var active = total - deleted;

            double deletedPercentage = total == 0 ? 0 : (deleted * 100.0 / total);
            double activePercentage = total == 0 ? 0 : (active * 100.0 / total);

            return new RoutePriceStatusesDto
            {
                TotalRoutePrices = total,
                ActiveRoutePrices = active,
                DeletedRoutePrices = deleted,
                ActiveRoutePricesPercentage = activePercentage,
                DeletedRoutePricesPercentage = deletedPercentage
            };
        }

        public async Task<List<RoutePriceByBusTypeDto>> GetAllRoutePricesByBusTypeIdAsync(int busTypeId)
        {
            return await _context.RoutePrices
                .Where(r => r.BusTypeId == busTypeId)
                .Select(r => new RoutePriceByBusTypeDto
                {
                    RoutePriceId = r.Id,
                    BusTypeId = r.BusTypeId,
                    BusTypeName = r.BusType.Type,
                    StartCityName = r.StartCity.Name,
                    EndCityName = r.EndCity.Name,
                    Price = r.Price,
                    DurationHours = r.DurationHours,
                    IsDeleted = r.IsDeleted,
                    DistanceKm = r.DistanceKm,
                })
                .ToListAsync();
        }

        public async Task<List<UsedRoutePriceDto>> GetMostActiveUsedRoutePricesAsync()
        {
            var routes = await _context.RoutePrices
                .Where(r => !r.IsDeleted)
                .Select(r => new UsedRoutePriceDto
                {
                    RoutePriceId = r.Id,
                    StartCityName = r.StartCity.Name,
                    EndCityName = r.EndCity.Name,
                    BusTypeId = r.BusTypeId,
                    BusTypeName = r.BusType.Type,
                    Price = r.Price,
                    DurationHours = r.DurationHours,
                    IsDeleted = r.IsDeleted,
                    DistanceKm = r.DistanceKm,
                    UsageCount = _context.Trips
                        .Count(t => t.RoutePriceId == r.Id && !t.IsDeleted)
                })
                .OrderByDescending(r => r.UsageCount)
                .ToListAsync();

            return routes;
        }

        public async Task<List<UsedRoutePriceDto>> GetLeastActiveUsedRoutePricesAsync()
        {
            var routes = await _context.RoutePrices
                .Where(r => !r.IsDeleted)
                .Select(r => new UsedRoutePriceDto
                {
                    RoutePriceId = r.Id,
                    StartCityName = r.StartCity.Name,
                    EndCityName = r.EndCity.Name,
                    BusTypeId = r.BusTypeId,
                    BusTypeName = r.BusType.Type,
                    Price = r.Price,
                    DurationHours = r.DurationHours,
                    IsDeleted = r.IsDeleted,
                    DistanceKm = r.DistanceKm,
                    UsageCount = _context.Trips
                        .Count(t => t.RoutePriceId == r.Id && !t.IsDeleted)
                })
                .OrderBy(r => r.UsageCount)
                .ToListAsync();

            return routes;
        }

        public decimal SuggestPriceForRoutePrice(decimal distanceKm, int capacity)
        {
            if (capacity <= 0)
                throw new ArgumentException("Capacity must be greater than 0.");

            if (distanceKm <= 0)
                throw new ArgumentException("Distance must be greater than 0.");

            decimal pricePerKm;

            if (capacity <= 20)
                pricePerKm = 6;
            else if (capacity <= 30)
                pricePerKm = 5;
            else if (capacity <= 40)
                pricePerKm = 4;
            else if (capacity <= 50)
                pricePerKm = 3;
            else
                pricePerKm = 2;

            return (int)distanceKm * pricePerKm;
        }

        public async Task<List<UsedRoutePriceDto>> SuggestRoutePricesForDeletionAsync()
        {
            var leastUsedRoute = await GetLeastActiveUsedRoutePricesAsync();
            var routesToDelete = leastUsedRoute
                .Where(r => r.UsageCount == 0)
                .Select(r => new UsedRoutePriceDto
                {
                    RoutePriceId = r.RoutePriceId,
                    StartCityName = r.StartCityName,
                    EndCityName = r.EndCityName,
                    BusTypeId = r.BusTypeId,
                    BusTypeName = r.BusTypeName,
                    Price = r.Price,
                    DurationHours = r.DurationHours,
                    IsDeleted = r.IsDeleted,
                    DistanceKm = r.DistanceKm,
                    UsageCount = r.UsageCount,
                })
            .ToList();
            return routesToDelete;
        }

        public async Task<List<UpdatePriceForRoutePriceSuggest>> UpdatePriceForRoutePriceSuggestAsync()
        {
            var leastUsedRoutes = await GetLeastActiveUsedRoutePricesAsync();

            var suggestions = leastUsedRoutes
                .Where(r => r.UsageCount <= 5)
                .Select(r =>
                {
                    decimal discountPercent =
                        r.UsageCount == 0 ? 0.30m :
                        r.UsageCount <= 2 ? 0.20m :
                        r.UsageCount <= 5 ? 0.10m :
                        0m;

                    decimal suggestedPrice = r.Price - (r.Price * discountPercent);

                    string message =
                        discountPercent == 0 ?
                        "This route is being used. No price adjustment is recommended." :
                        r.UsageCount == 0 ?
                        "This route has no usage at all. A price reduction of 30% is recommended." :
                        r.UsageCount <= 2 ?
                        "This route has very low usage. A price reduction of 20% is recommended." :
                        "This route has low usage. A price reduction of 10% is recommended.";

                    return new UpdatePriceForRoutePriceSuggest
                    {
                        RoutePriceId = r.RoutePriceId,
                        StartCityName = r.StartCityName,
                        EndCityName = r.EndCityName,
                        BusTypeName = r.BusTypeName,
                        DistanceKm = r.DistanceKm,
                        UsageCount = r.UsageCount,
                        CurrentPrice = r.Price,
                        SuggestedPrice = suggestedPrice,
                        Suggestion = message
                    };
                })
                .ToList();

            return suggestions;
        }
    }
}