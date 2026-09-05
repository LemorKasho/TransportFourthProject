using TransportFourthProject.Api.DTOs.Trip;
using TransportFourthProject.Api.Models;

namespace TransportFourthProject.Api.Repositories
{
    public interface IEmployeeTripRepository : IRepository<Trip>
    {
        Task<IEnumerable<Trip>> GetAllTripsForEmployeeAsync();
        Task<Trip?> GetTripDetailsForEmployeeAsync(int tripId);

        Task<int> GetBookedSeatsForEmployeeAsync(int tripId);

        Task<IEnumerable<Trip>> SearchTripsForEmployeeAsync(string? startCity, string? endCity, DateTime? date,
                                                    bool hasTime, bool hasMinutes, string? busType,
                                                    string? sortBy, string? order,
                                                    string? status, int? driverId, int? busId,
                                                    bool? hasDiscount, string? capacityStatus);
        Task<TripSearchOptionsDto> GetTripSearchOptionsForEmployeeAsync();

        Task<List<Employee>> GetAvailableDriversAsync(DateTime departureTime);

        Task<List<Bus>> GetAvailableBusesAsync(DateTime departureTime, int busTypeId);
        
        Task<List<TripDiscount>> GetAvailableTripDiscountsAsync();

        Task<List<RoutePrice>> GetRoutePricesAsync();

        Task<Trip> AddTripForEmployeeAsync(EmployeeAddTripDto dto);
        Task<TripPatchResultDto> PatchTripForEmployeeAsync(EmployeePatchTripDto dto);

        Task<TripEditDataDto> GetTripEditDataAsync(int tripId);

        Task<bool> DeleteTripAsync(int tripId);

        Task<List<SeatStatusWithUserDto>?> GetTripSeatsWithUsersAsync(int tripId);

        Task<SeatStatusSummaryDto?> GetTripSeatStatusSummaryAsync(int tripId);


        Task<int> GetTripsCountInMonthAsync(int month, int year);

        Task<int> GetTripsCountInYearAsync(int year);

        Task<List<RoutePriceTripsCountDto>> GetTripsCountByRoutePriceAsync();

        Task<List<BusTypeTripsCountDto>> GetTripsCountByBusTypeAsync();

        Task<List<TripOnRoadDto>> GetTripsOnRoadsAsync();

        Task<List<UpcomingTripDto>> GetUpcomingTripsAsync();

        Task<List<TodayTripsDto>> GetTodayTripsAsync();
        Task<List<TodayTripsDto>> GetTomorrowTripsAsync();




    }
}
