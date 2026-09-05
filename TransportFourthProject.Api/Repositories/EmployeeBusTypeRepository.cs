using Microsoft.EntityFrameworkCore;
using TransportFourthProject.Api.Data;
using TransportFourthProject.Api.DTOs.BusType;
using TransportFourthProject.Api.Models;

namespace TransportFourthProject.Api.Repositories
{
    public class EmployeeBusTypeRepository :Repository<BusType>, IEmployeeBusTypeRepository
    {   
        private readonly AppDbContext _context;
        public EmployeeBusTypeRepository(AppDbContext context) : base (context)
        {
            _context = context;
        }
        public async Task<List<BusTypeDto>> GetAllBusTypesAsync()
        {
            return await _context.BusTypes
                .Where(t => !t.IsDeleted)
                .Select(t => new BusTypeDto
                {
                    BusTypeId = t.Id,
                    Name = t.Type,
                    Capacity = t.Capacity
                })
                .ToListAsync();
        }

        public async Task<List<BusTypeDto>> GetAllDeletedBusTypesAsync()
        {
            return await _context.BusTypes
                .Where(t => t.IsDeleted)
                .Select(t => new BusTypeDto
                {
                    BusTypeId = t.Id,
                    Name = t.Type,
                    Capacity = t.Capacity
                })
                .ToListAsync();
        }

        public async Task<string> AddBusTypeAsync(AddBusTypeForEmployeeDto dto)
        {
            bool exists = await _context.BusTypes
                .AnyAsync(t => t.Type.ToLower() == dto.Name.ToLower());

            if (exists)
                return "Bus type already exists";

            var busType = new BusType
            {
                Type = dto.Name,
                Capacity = dto.Capacity,
                IsDeleted = false
            };

            await _context.BusTypes.AddAsync(busType);
            await _context.SaveChangesAsync();

            return "Bus type added successfully";
        }

        public async Task<string> DeleteBusTypeAsync(int busTypeId)
        {
            var type = await _context.BusTypes.FindAsync(busTypeId);

            if (type == null)
                return "Bus type not found";

            if (type.IsDeleted)
                return "Bus type is already deleted";

            bool usedByActiveOrFutureTrip = await _context.Trips
                .AnyAsync(t =>
                    !t.IsDeleted &&
                    t.Bus.BusTypeId == busTypeId &&
                    (
                        t.DepartureTime >= DateTime.Now ||  
                        t.ArrivalTime >= DateTime.Now       
                    )
                );

            if (usedByActiveOrFutureTrip)
                return "Cannot delete bus type because it is used by active or upcoming trips.";

            type.IsDeleted = true;

            await _context.SaveChangesAsync();
            return "Bus type deleted successfully";
        }

        public async Task<string> RestoreBusTypeAsync(int busTypeId)
        {
            var type = await _context.BusTypes.FindAsync(busTypeId);

            if (type == null)
                return "Bus type not found";

            if (!type.IsDeleted)
                return "Bus type is already active";

            type.IsDeleted = false;

            await _context.SaveChangesAsync();
            return "Bus type restored successfully";
        }
    }
}
