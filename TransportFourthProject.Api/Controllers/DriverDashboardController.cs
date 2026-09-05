using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TransportFourthProject.Api.Repositories;

namespace TransportFourthProject.Api.Controllers
{
    [ApiController]
    [Route("api/driver-hashboard")]
    // [Authorize(Roles = "Driver")]
    public class DriverDashboardController : ControllerBase
    {
        private readonly IDriverDashboardRepo _driverDashboardRepo;
        public DriverDashboardController(IDriverDashboardRepo repo)
        {
            _driverDashboardRepo = repo;
        }

        [HttpGet("today-trip-driver")]
        public async Task<IActionResult> GetTodayTrips()
        {
            var driverId = int.Parse(User.FindFirst("EmployeeId").Value);
            var result = await _driverDashboardRepo.GetTodayTripsAsync(driverId);
            return Ok(result);
        }

        [HttpGet("tomorrow-trip-driver")]
        public async Task<IActionResult> GetTomorrowTrips()
        {
            var driverId = int.Parse(User.FindFirst("EmployeeId").Value);

            var result = await _driverDashboardRepo.GetTomorrowTripsAsync(driverId);

            return Ok(result);
        }

        [HttpGet("after-tomorrow")]
        public async Task<IActionResult> GetAfterTomorrowTrips()
        {
            var driverId = int.Parse(User.FindFirst("EmployeeId").Value);

            var result = await _driverDashboardRepo.GetAfterTomorrowTripsAsync(driverId);

            return Ok(result);
        }

        [HttpGet("monthly/{year}/{month}")]
        public async Task<IActionResult> GetMonthlyTripsCount(int year, int month)
        {
            var driverId = int.Parse(User.FindFirst("EmployeeId").Value);

            var result = await _driverDashboardRepo.GetMonthlyTripsCountAsync(driverId, year, month);

            return Ok(result);
        }

        [HttpGet("yearly/{year}")]
        public async Task<IActionResult> GetYearlyTripsCount(int year)
        {
            var driverId = int.Parse(User.FindFirst("EmployeeId").Value);

            var result = await _driverDashboardRepo.GetYearlyTripsCountAsync(driverId, year);

            return Ok(result);
        }


    }
}









