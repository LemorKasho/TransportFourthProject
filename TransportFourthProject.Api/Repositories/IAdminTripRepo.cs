using TransportFourthProject.Api.DTOs.Trip;
using TransportFourthProject.Api.Models;

namespace TransportFourthProject.Api.Repositories
{
    public interface IAdminTripRepo : IRepository<Trip>
    {
        Task<List<TripConfirmedPassengersDto>> GetConfirmedPassengersAsync(int tripId);
    }
}
