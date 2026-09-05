using Microsoft.AspNetCore.Mvc.TagHelpers.Cache;
using TransportFourthProject.Api.Enums;
using TransportFourthProject.Api.Models;

namespace TransportFourthProject.Api.Repositories
{
    public interface IAdminAllOperationOnEmployeeTableRepo
    {
        Task AddAsync(Employee employee);
        Task<Employee?> GetByIdAsync(int id);
        void Update(Employee employee);
        Task SaveChangesAsync();

    }
}
