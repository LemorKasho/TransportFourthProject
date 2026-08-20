using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TransportFourthProject.Api.Data;
using TransportFourthProject.Api.DTOs.Booking;
using TransportFourthProject.Api.DTOs.Pricing;
using TransportFourthProject.Api.Enums;
using TransportFourthProject.Api.Models;
using TransportFourthProject.Api.Services.Pricing;

namespace TransportFourthProject.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BookingController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly PriceCalculatorService _priceService;

        public BookingController(AppDbContext context, PriceCalculatorService priceService)
        {
            _context = context;
            _priceService = priceService;
        }
        [Authorize]
        [HttpPost("cancel-booking/{bookingId}")]
        public async Task<IActionResult> CancelBooking(int bookingId)
        {
            var booking = await _context.Bookings
                .FirstOrDefaultAsync(b => b.Id == bookingId);

            if (booking == null)
                return NotFound(new BookingResponseDto
                {
                    BookingId = bookingId,
                    BookingStatus = "NotFound",
                    Message = "Booking not found"
                });

            if (booking.Status == BookingStatus.PendingPayment)
            {
                if (booking.ExpirationTime.HasValue && booking.ExpirationTime < DateTime.Now)
                    return BadRequest(new BookingResponseDto
                    {
                        BookingId = booking.Id,
                        SeatNumber = booking.SeatNumber,
                        BookingStatus = "Expired",
                        Message = "Booking already expired"
                    });

                booking.Status = BookingStatus.Cancelled;
                booking.SeatStatus = SeatStatus.Available;
                booking.ExpirationTime = null;

                await _context.SaveChangesAsync();

                return Ok(new BookingResponseDto    
                {
                    BookingId = booking.Id,
                    SeatNumber = booking.SeatNumber,
                    BookingStatus = booking.Status.ToString(),
                    Message = "Temporary booking cancelled successfully"
                });
            }

            if (booking.Status == BookingStatus.Confirmed)
            {
                booking.Status = BookingStatus.Cancelled;
                booking.SeatStatus = SeatStatus.Available;
                booking.ExpirationTime = null;

                await _context.SaveChangesAsync();

                return Ok(new BookingResponseDto
                {
                    BookingId = booking.Id,
                    SeatNumber = booking.SeatNumber,
                    BookingStatus = booking.Status.ToString(),
                    Message = "Confirmed booking cancelled successfully"
                });
            }

            return BadRequest(new BookingResponseDto
            {
                BookingId = booking.Id,
                SeatNumber = booking.SeatNumber,
                BookingStatus = booking.Status.ToString(),
                Message = "Only temporary or confirmed bookings can be cancelled"
            });
        }

        [Authorize]
        [HttpPost("temporary-booking/{bookingId}")]
        public async Task<IActionResult> TemporaryBooking(int bookingId)
        {
            var booking = await _context.Bookings.FindAsync(bookingId);
            if (booking == null)
                return NotFound(new BookingResponseDto
                {
                    BookingId = bookingId,
                    BookingStatus = "NotFound",
                    Message = "Booking not found"
                });
            if(booking.Status == BookingStatus.PendingPayment ||
                  booking.Status == BookingStatus.Confirmed)
            {
                return BadRequest( new BookingResponseDto
                {
                    BookingId = booking.Id,
                    SeatNumber = booking.SeatNumber,
                    BookingStatus = booking.Status.ToString(),
                    Message = "Booking is already temporary or confirmed."
                });
            }
            booking.Status = BookingStatus.PendingPayment;
            booking.SeatStatus = SeatStatus.Reserved;
            booking.ExpirationTime = DateTime.Now.AddHours(2);

            await _context.SaveChangesAsync();

            var response = new BookingResponseDto
            {
                BookingId = booking.Id,
                SeatNumber = booking.SeatNumber,
                ExpirationTime = booking.ExpirationTime,
                BookingStatus = booking.Status.ToString(),
                Message = "Temporary booking started. Seat reserved until expiration."
            };
            return Ok(response);
        }
    }
}