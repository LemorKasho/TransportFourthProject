using TransportFourthProject.Api.DTOs.Bus;
using TransportFourthProject.Api.Enums;
using TransportFourthProject.Api.Models;
using TransportFourthProject.Api.Repositories;

namespace TransportFourthProject.Api.Repositories
{
    public interface IEmployeeBusRepository : IRepository<Bus>
    {
        Task<List<BusListForEmployeeDto>> GetAllBusesAsync();
        Task<List<BusListForEmployeeDto>> SearchBusesAsync(BusStatus? status, int? busTypeId);
        Task<bool> AddBusAsync(AddBusDto dto);
        Task<bool> UpdateBusAsync(int busId, UpdateBusStatusDto dto);
        Task<List<BusUsageDto>> GetMostUsedBusesAsync();
        Task<List<BusUsageDto>> GetLeastUsedBusesAsync();
        Task<List<BusUsageDto>> GetMostActiveBusesInMonthAsync(int month, int year);
        Task<List<BusDeletionSuggestionDto>> SuggestBusDeletionAsync();
        Task<List<BusOnRoadsDto>> GetBusesOnRoadsAsync();

    }
}