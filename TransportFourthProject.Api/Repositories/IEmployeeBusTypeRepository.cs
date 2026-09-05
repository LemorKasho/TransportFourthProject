using TransportFourthProject.Api.DTOs.BusType;
using TransportFourthProject.Api.Models;

namespace TransportFourthProject.Api.Repositories
{
    public interface IEmployeeBusTypeRepository : IRepository<BusType>
    {
        Task<List<BusTypeDto>> GetAllBusTypesAsync();
        Task<List<BusTypeDto>> GetAllDeletedBusTypesAsync();
        Task<string> AddBusTypeAsync(AddBusTypeForEmployeeDto busTypeDto);
        Task<string> DeleteBusTypeAsync(int busYpeId);
        Task<string> RestoreBusTypeAsync(int busYpeId);
    }
}
