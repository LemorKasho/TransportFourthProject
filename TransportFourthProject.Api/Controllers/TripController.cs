using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Text.RegularExpressions;
using TransportFourthProject.Api.Data;
using TransportFourthProject.Api.DTOs.Trip;
using TransportFourthProject.Api.Models;
using TransportFourthProject.Api.Repositories;
namespace TransportFourthProject.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TripController : ControllerBase
    {
        private readonly ITripRepository _tripRepo;
        private readonly AppDbContext _context;

        public TripController(ITripRepository tripRepo, AppDbContext context)
        {
            _tripRepo = tripRepo;
            _context = context;
        }

        [HttpGet("all-trips")]
        public async Task<IActionResult> GetAllTrips()
        {
            var trips = await _tripRepo.GetAllTripsAsync();

            var result = trips.Select(t => new TripListDto
            {
                TripId = t.Id,
                DepartureTime = t.DepartureTime.ToString("yyyy-MM-dd HH:mm"),
                StartCity = t.RoutePrice.StartCity.Name,
                EndCity = t.RoutePrice.EndCity.Name,
                BusType = t.Bus.BusType.Type,
                BasePrice = (t.RoutePrice.Price.ToString()) + " SYP",
                DiscountName = t.TripDiscount?.Name ?? "no discount",
                DiscountPercentage = (t.TripDiscount?.Percentage ?? 0) + "%"
            });

            return Ok(result);
        }

        [HttpGet("search")]
        public async Task<IActionResult> SearchTrips(string? startCity, string? endCity, DateTime? date,
                                                     string? busType, string? sortBy, string? order)
        {
            bool hasTime = date.HasValue && date.Value.TimeOfDay != TimeSpan.Zero;
            bool hasMinutes = hasTime && date.Value.Minute != 0;

            var trips = await _tripRepo.SearchTripsAsync(startCity, endCity, date,
                                                        hasTime, hasMinutes, busType,
                                                        sortBy, order);

            var result = trips.Select(t => new TripListDto
            {
                TripId = t.Id,
                DepartureTime = t.DepartureTime.ToString("yyyy-MM-dd HH:mm"),
                StartCity = t.RoutePrice.StartCity.Name,
                EndCity = t.RoutePrice.EndCity.Name,
                BusType = t.Bus.BusType.Type,
                BasePrice = (t.RoutePrice.Price.ToString()) + " SYP",
                DiscountName = t.TripDiscount?.Name ?? "no discount",
                DiscountPercentage = (t.TripDiscount?.Percentage ?? 0) + "%"
            });

            return Ok(result);
        }

        [HttpGet("details/{tripId}")]
        public async Task<IActionResult> GetTripDetails(int tripId)
        {
            var trip = await _tripRepo.GetTripDetailsAsync(tripId);

            if (tripId <= 0)
                return BadRequest("Invalid trip ID");
            if (trip == null)
                return NotFound("Trip not found");

            var bookedSeats = await _tripRepo.GetBookedSeatsAsync(tripId);

            int availableSeats = trip.Bus.BusType.Capacity - bookedSeats;

            string tripStatus = availableSeats == 0 ? "complete" : "available";

            var dto = new TripDetailsDto
            {
                TripId = trip.Id,
                DepartureTime = trip.DepartureTime.ToString("yyyy-MM-dd HH:mm"),

                StartCity = trip.RoutePrice.StartCity.Name,
                EndCity = trip.RoutePrice.EndCity.Name,

                BusNumber = trip.Bus.Id,
                BusType = trip.Bus.BusType.Type,

                AvailableSeats = availableSeats,
                TripStatus = tripStatus,

                BasePrice = (trip.RoutePrice.Price.ToString()) + " SYP",

                DriverName = $"{trip.Employee.FirstName} {trip.Employee.LastName}",

                DiscountName = trip.TripDiscount?.Name ?? "no discount",
                DiscountPercentage = (trip.TripDiscount?.Percentage ?? 0) + "%"

            };

            return Ok(dto);
        }

        [Authorize]
        [HttpGet("{tripId}/seats")]
        public async Task<IActionResult> GetTripSeats(int tripId)
        {
            var trip = await _tripRepo.GetTripDetailsAsync(tripId);
            if (tripId <= 0)
                return BadRequest("Invalid trip ID");

            if (trip == null)
                return NotFound("Trip not found.");

            var seats = await _tripRepo.GetTripSeatsAsync(tripId);

            return Ok(seats);
        }

        [Authorize]
        [HttpGet("{tripId}/available-seats")]
        public async Task<IActionResult> GetAvailableSeats(int tripId)
        {
            var trip = await _tripRepo.GetTripDetailsAsync(tripId);
            if (tripId <= 0)
                return BadRequest("Invalid trip ID");

            if (trip == null)
                return NotFound("Trip not found.");

            var seats = await _tripRepo.GetAvailableSeatsAsync(tripId);

            return Ok(seats);
        }

        [Authorize]
        [HttpPost("select-seat")]
        public async Task<SelectSeatResponseDto> SelectSeat([FromBody] SelectSeatDto dto)
        {
            if (!ModelState.IsValid)
                return new SelectSeatResponseDto
                {
                    Message = "Only numbers are allowed."
                };

            if(dto.SeatNumber <= 0 || dto.TripId <= 0)
                return new SelectSeatResponseDto
                {
                    Message = "Seat number and TripId must be positive numbers."
                };

            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == null)
                return new SelectSeatResponseDto
                {
                    Message = "Invalid token"
                };

            return await _tripRepo.SelectSeatAsync(dto, int.Parse(userId));
        }
    }
}