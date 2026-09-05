using TransportFourthProject.Api.Data;
using TransportFourthProject.Api.Models;
using static TransportFourthProject.Api.Repositories.AdminAllOperationOnEmployeeTableRepo;

namespace TransportFourthProject.Api.Repositories
{
    public class AdminAllOperationOnEmployeeTableRepo : IAdminAllOperationOnEmployeeTableRepo
    {
        private readonly AppDbContext _context;
        public AdminAllOperationOnEmployeeTableRepo(AppDbContext context)
        {
            _context = context;
        }
        public async Task AddAsync(Employee employee)
            => await _context.Employees.AddAsync(employee);

        public async Task<Employee?> GetByIdAsync(int id)
            => await _context.Employees.FindAsync(id);

        public void Update(Employee employee)
            => _context.Employees.Update(employee);

        public async Task SaveChangesAsync()
            => await _context.SaveChangesAsync();

    }
}
