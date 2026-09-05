using TransportFourthProject.Api.DTOs.City;
using TransportFourthProject.Api.Models;

namespace TransportFourthProject.Api.Repositories
{
    public interface IEmployeeCityRepository : IRepository<City>
    {
        Task<List<CityDto>> GetAllCitiesAsync();
        Task<string> AddCityAsync(AddCityForEmployeeDto dto);
        Task<string> UpdateCityAsync(int id, UpdateCityForEmployeeDto dto);
        Task<List<CityUsageDto>> GetLeastUsedCitiesByTripsAsync();
        Task<List<CityUsageDto>> GetMostUsedCitiesByTripsAsync();
        Task<List<CityUsageDto>> GetMostUsedCitiesByRoutesAsync();
        Task<List<CityUsageDto>> GetLeastUsedCitiesByRoutesAsync();
        Task<List<CityUsageDto>> GetMostTravelCitiesInMonthAsync(int month, int year);
    }
}
