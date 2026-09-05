using TransportFourthProject.Api.DTOs.TripDiscount;
using TransportFourthProject.Api.Models;

namespace TransportFourthProject.Api.Repositories
{
    public interface IAdminTripDiscountRepository : IRepository<TripDiscount>
    {
        Task<string> AddTripDiscountAsync(AddTripDiscountDto dto);
        Task<string> UpdateTripDiscountAsync(UpdateTripDiscountDto dto);
        Task<string> DeleteTripDiscountAsync(int tripDiscountId);
        Task<string> RestoreTripDiscountAsync(int tripDiscountId);

        Task<List<TripDiscount>> GetDeletedTripDiscountsAsync();
        Task<List<TripDiscount>> GetActiveTripDiscountsAsync();
    }
}
