using TransportFourthProject.Api.DTOs.Trip;
using TransportFourthProject.Api.Models;

namespace TransportFourthProject.Api.Repositories
{
    public interface ITripRepository : IRepository<Trip>
    {
        Task<IEnumerable<Trip>> GetAllTripsAsync();
        Task<IEnumerable<Trip>> SearchTripsAsync(string? startCity, string? endCity, DateTime? date,
                                                     bool hasTime,bool hasMinutes, string? busType, string? sortBy, string? order);

        Task<Trip?> GetTripDetailsAsync(int tripId);
        Task<int> GetBookedSeatsAsync(int tripId);

        Task<SelectSeatResponseDto> SelectSeatAsync(SelectSeatDto dto, int userId);
        Task<List<SeatStatusDto>> GetTripSeatsAsync(int tripId);

        Task<List<int>> GetAvailableSeatsAsync(int tripId);
    }
}