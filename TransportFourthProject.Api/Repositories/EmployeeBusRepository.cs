using Microsoft.EntityFrameworkCore;
using TransportFourthProject.Api.Data;
using TransportFourthProject.Api.DTOs.Bus;
using TransportFourthProject.Api.Enums;
using TransportFourthProject.Api.Models;

namespace TransportFourthProject.Api.Repositories
{
    public class EmployeeBusRepository : Repository<Bus>, IEmployeeBusRepository
    {
        private readonly AppDbContext _context;
        public EmployeeBusRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }
        public async Task<List<BusListForEmployeeDto>> GetAllBusesAsync()
        {
            var buses = await _context.Buses
                .Include(b => b.BusType)
                .ToListAsync();

            return buses.Select(b => new BusListForEmployeeDto
            {
                BusId = b.Id,
                BusNumber = b.BusNumber,
                Type = b.BusType.Type,
                Capacity = b.BusType.Capacity,
                Status = b.Status
            }).ToList();
        }

        public async Task<List<BusListForEmployeeDto>> SearchBusesAsync(BusStatus? status, int? busTypeId)
        {
            var query = _context.Buses
                .Include(b => b.BusType)
                .Where(b => !b.BusType.IsDeleted)
                .AsQueryable();

            if (status.HasValue)
                query = query.Where(b => b.Status == status.Value);

            if (busTypeId.HasValue)
                query = query.Where(b => b.BusTypeId == busTypeId.Value);

            var buses = await query.ToListAsync();

            return buses.Select(b => new BusListForEmployeeDto
            {
                BusId = b.Id,
                BusNumber = b.BusNumber,
                Type = b.BusType.Type,
                Capacity = b.BusType.Capacity,
                Status = b.Status
            }).ToList();
        }

        public async Task<bool> AddBusAsync(AddBusDto dto)
        {
            try
            {
                bool exists = await _context.Buses.AnyAsync(b => b.BusNumber == dto.BusNumber);
                if (exists)
                    return false;

                var bus = new Bus
                {
                    BusNumber = dto.BusNumber,
                    BusTypeId = dto.BusTypeId,
                    Status = dto.Status
                };

                await _context.Buses.AddAsync(bus);
                await _context.SaveChangesAsync();

                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> UpdateBusAsync(int busId, UpdateBusStatusDto dto)
        {
            var bus = await _context.Buses
                .Include(b => b.Trips)
                .FirstOrDefaultAsync(b => b.Id == busId);
            if (bus == null)
                return false;

            bool hasActiveTrips = bus.Trips.Any(t =>
                !t.IsDeleted &&
                (
                    t.DepartureTime >= DateTime.Now ||
                    t.ArrivalTime >= DateTime.Now
                ));
            if (hasActiveTrips)
                return false;

            bus.Status = dto.Status;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<List<BusUsageDto>> GetMostUsedBusesAsync()
        {
            var buses = await _context.Buses
                .Select(b => new BusUsageDto
                {
                    BusId = b.Id,
                    BusNumber = b.BusNumber,
                    Capacity = b.BusType.Capacity,
                    Status = b.Status,
                    UsageCount = _context.Trips.Count(t => t.BusId == b.Id)
                })
                .OrderByDescending(b => b.UsageCount)
                .ToListAsync();

            return buses;
        }

        public async Task<List<BusUsageDto>> GetLeastUsedBusesAsync()
        {
            var buses = await _context.Buses
                .Select(b => new BusUsageDto
                {
                    BusId = b.Id,
                    BusNumber = b.BusNumber,
                    Capacity = b.BusType.Capacity,
                    Status = b.Status,
                    UsageCount = _context.Trips.Count(t => t.BusId == b.Id)
                })
                .OrderBy(b => b.UsageCount)
                .ToListAsync();

            return buses;
        }

        public async Task<List<BusUsageDto>> GetMostActiveBusesInMonthAsync(int month, int year)
        {
            if (month < 1 || month > 12)
                throw new ArgumentException("Month must be between 1 and 12.");

            if (year < 2020 || year > 2070)
                throw new ArgumentException("Year must be between 2020 and 2070 .");

            var buses = await _context.Buses
                .Include(b => b.BusType)
                .Select(b => new BusUsageDto
                {
                    BusId = b.Id,
                    BusNumber = b.BusNumber,
                    Capacity = b.BusType.Capacity,
                    Status = b.Status,

                    UsageCount = _context.Trips
                        .Count(t =>
                            t.BusId == b.Id &&
                            t.DepartureTime.Month == month &&
                            t.DepartureTime.Year == year
                        )
                })
                .OrderByDescending(b => b.UsageCount)
                .ToListAsync();

            return buses;
        }

        public async Task<List<BusDeletionSuggestionDto>> SuggestBusDeletionAsync()
        {
            var buses = await _context.Buses
                .Include(b => b.BusType)
                .Select(b => new BusDeletionSuggestionDto
                {
                    BusId = b.Id,
                    BusNumber = b.BusNumber,
                    Capacity = b.BusType.Capacity,
                    UsageCount = _context.Trips.Count(t => t.BusId == b.Id),
                    Status = b.Status
                })
                .ToListAsync();

            var suggestions = buses
                .Where(b =>
                    b.UsageCount == 0 &&       
                    b.Status == BusStatus.Active        
                )
                .Select(b => new BusDeletionSuggestionDto
                {
                    BusId = b.BusId,
                    BusNumber = b.BusNumber,
                    Capacity = b.Capacity,
                    UsageCount = b.UsageCount,
                    Status = b.Status,
                    Suggestion = "This bus has no usage. It is recommended to delete it."
                })
                .ToList();

            return suggestions;
        }

        public async Task<List<BusOnRoadsDto>> GetBusesOnRoadsAsync()
        {
            DateTime now = DateTime.Now;

            var busesOnRoad = await _context.Trips
                .Include(t => t.Bus)
                .ThenInclude(b => b.BusType)
                .Where(t => now >= t.DepartureTime && now <= t.ArrivalTime)
                .Select(t => new BusOnRoadsDto
                {
                    BusId = t.Bus.Id,
                    BusNumber = t.Bus.BusNumber,
                    Capacity = t.Bus.BusType.Capacity,
                    DepartureTime = t.DepartureTime,
                    ArrivalTime = t.ArrivalTime
                })
                .ToListAsync();

            return busesOnRoad;
        }
    }
}