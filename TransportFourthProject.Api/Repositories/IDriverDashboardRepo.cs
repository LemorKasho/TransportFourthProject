using TransportFourthProject.Api.DTOs.Employee;

namespace TransportFourthProject.Api.Repositories
{
    public interface IDriverDashboardRepo
    {
        Task<List<DriverTripDto>> GetTodayTripsAsync(int driverId);
        Task<List<DriverTripDto>> GetTomorrowTripsAsync(int driverId);
        Task<List<DriverTripDto>> GetAfterTomorrowTripsAsync(int driverId);

        Task<int> GetMonthlyTripsCountAsync(int driverId, int year, int month);
        Task<int> GetYearlyTripsCountAsync(int driverId, int year);
    }
}