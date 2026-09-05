using TransportFourthProject.Api.DTOs.UserDiscount;
using TransportFourthProject.Api.Models;

namespace TransportFourthProject.Api.Repositories
{
    public interface IAdminUserDiscountRepository : IRepository<UserDiscount>
    {
        Task<string> AddUserDiscountAsync(AddUserDiscountDto dto);
        Task<string> UpdateUserDiscountAsync(UpdateUserDiscountDto dto);
        Task<string> DeleteUserDiscountAsync(int userDiscountId);
        Task<string> RestoreUserDiscountAsync(int userDiscountId);

        Task<List<UserDiscount>> GetDeletedUserDiscountsAsync();
        Task<List<UserDiscount>> GetActiveUserDiscountsAsync();
    }
}