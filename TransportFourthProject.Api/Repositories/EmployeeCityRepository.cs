using Microsoft.EntityFrameworkCore;
using TransportFourthProject.Api.Data;
using TransportFourthProject.Api.DTOs.City;
using TransportFourthProject.Api.Models;

namespace TransportFourthProject.Api.Repositories
{
    public class EmployeeCityRepository :Repository<City>, IEmployeeCityRepository
    {
        private readonly AppDbContext _context;
        
        public EmployeeCityRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<List<CityDto>> GetAllCitiesAsync()
        {
            return await _context.Cities
                .OrderBy(c => c.Name)
                .Select(c => new CityDto
                {
                    Id = c.Id,
                    Name = c.Name
                })
                .ToListAsync();
        }

        public async Task<string> AddCityAsync(AddCityForEmployeeDto dto)
        {
            bool existsActive = await _context.Cities
                .AnyAsync(c => c.Name.ToLower() == dto.CityName.ToLower());

            if (existsActive)
                return "City already exists";

            var city = new City
            {
                Name = dto.CityName,
            };

            await _context.Cities.AddAsync(city);
            await _context.SaveChangesAsync();

            return "City added successfully";
        }

        public async Task<string> UpdateCityAsync(int id, UpdateCityForEmployeeDto dto)
        {
            var city = await _context.Cities.FindAsync(id);

            if (city == null)
                return "City not found";

            bool exists = await _context.Cities
                .AnyAsync(c => c.Id != id && c.Name.ToLower() == dto.CityName.ToLower());

            if (exists)
                return "City name already exists";

            city.Name = dto.CityName;

            await _context.SaveChangesAsync();
            return "City updated successfully";
        }





        public async Task<List<CityUsageDto>> GetLeastUsedCitiesByTripsAsync()
        {
            var cities = await _context.Cities
            .Select(c => new CityUsageDto
            {
                CityId = c.Id,
                CityName = c.Name,
                UsageCount = _context.Trips
            .Count(t => t.RoutePrice.StartCityId == c.Id || t.RoutePrice.EndCityId == c.Id)
            })
            .OrderBy(c => c.UsageCount)
            .ToListAsync();

            return cities;
        }

        public async Task<List<CityUsageDto>> GetMostUsedCitiesByTripsAsync()
        {
            var cities = await _context.Cities
            .Select(c => new CityUsageDto
            {
                CityId = c.Id,
                CityName = c.Name,
                UsageCount = _context.Trips
            .Count(t => t.RoutePrice.StartCityId == c.Id || t.RoutePrice.EndCityId == c.Id)
            })
            .OrderByDescending(c => c.UsageCount)
            .ToListAsync();

            return cities;
        }

        public async Task<List<CityUsageDto>> GetMostUsedCitiesByRoutesAsync()
        {
            var cities = await _context.Cities
            .Select(c => new CityUsageDto
            {
                CityId = c.Id,
                CityName = c.Name,
                UsageCount = _context.RoutePrices
            .Count(r => r.StartCityId == c.Id || r.EndCityId == c.Id)
            })
            .OrderByDescending(c => c.UsageCount)
            .ToListAsync();

            return cities;
        }

        public async Task<List<CityUsageDto>> GetLeastUsedCitiesByRoutesAsync()
        {
            var cities = await _context.Cities
            .Select(c => new CityUsageDto
            {
                CityId = c.Id,
                CityName = c.Name,
                UsageCount = _context.RoutePrices
            .Count(r => r.StartCityId == c.Id || r.EndCityId == c.Id)
            })
            .OrderBy(c => c.UsageCount)
            .ToListAsync();

            return cities;
        }

        public async Task<List<CityUsageDto>> GetMostTravelCitiesInMonthAsync(int month, int year)
        {
            if (month < 1 || month > 12)
                throw new ArgumentException("Month must be between 1 and 12");
            if (year < 2020 || year > 2070)
                throw new ArgumentException("Year must be between 2020 and 2070");

            var cities = await _context.Cities
            .Select(c => new CityUsageDto
            {
                CityId = c.Id,
                CityName = c.Name,
                UsageCount = _context.Trips
            .Count(t =>
            (t.RoutePrice.StartCityId == c.Id || t.RoutePrice.EndCityId == c.Id) &&
            t.DepartureTime.Month == month &&
            t.DepartureTime.Year == year
            )
            })
            .OrderByDescending(c => c.UsageCount)
            .ToListAsync();

            return cities;
        }
    }
}