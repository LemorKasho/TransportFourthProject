using TransportFourthProject.Api.DTOs.UserDiscountTicket;
using TransportFourthProject.Api.Models;

namespace TransportFourthProject.Api.Repositories
{
    public interface IAdminUserDiscountTicketRepo : IRepository<UserDiscountTicket>
    {
        Task<string> AddUserDiscountTicketAsync(AddUserDiscountTicketDto dto);
        Task<List<UserDiscountTicketViewDto>> GetUserDiscountTicketsAsync(int userId);
    }
}