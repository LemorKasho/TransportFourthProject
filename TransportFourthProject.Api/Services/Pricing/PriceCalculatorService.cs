using Microsoft.EntityFrameworkCore;
using TransportFourthProject.Api.Data;
using TransportFourthProject.Api.DTOs.Pricing;

namespace TransportFourthProject.Api.Services.Pricing
{
    public class PriceCalculatorService
    {
        private readonly AppDbContext _context;

        public PriceCalculatorService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<PriceResponseDto?> CalculateFinalPriceAsync(PriceRequestDto request)
        {
            var booking = await _context.Bookings
                .Include(b => b.Trip)
                    .ThenInclude(t => t.RoutePrice)
                .Include(b => b.Trip)
                    .ThenInclude(t => t.TripDiscount)
                .Include(b => b.User)
                    .ThenInclude(u => u.UserDiscountTickets)
                        .ThenInclude(ud => ud.UserDiscount)
                .FirstOrDefaultAsync(b => b.Id == request.BookingId);

            if (booking == null)
                return null;

            decimal basePrice = booking.Trip.RoutePrice.Price;

            decimal tripDiscountPercentage = booking.Trip.TripDiscount?.Percentage ?? 0;
            decimal tripDiscountAmount = basePrice * (tripDiscountPercentage / 100m);

            decimal priceAfterTripDiscount = basePrice - tripDiscountAmount;

            decimal userDiscountPercentage = 0;
            decimal userDiscountAmount = 0;

            var activeUserDiscounts = booking.User.UserDiscountTickets
                .Where(d => d.StartDate <= DateTime.Now && d.EndDate >= DateTime.Now)
                .ToList();

            if (activeUserDiscounts.Any())
            {
                userDiscountPercentage = activeUserDiscounts
                    .Max(d => d.UserDiscount.Percentage);

                userDiscountAmount = priceAfterTripDiscount * (userDiscountPercentage / 100m);
            }

            decimal finalPrice = priceAfterTripDiscount - userDiscountAmount;

            return new PriceResponseDto
            {
                BasePrice = basePrice,
                TripDiscountPercentage = tripDiscountPercentage,
                UserDiscountPercentage = userDiscountPercentage,
                FinalPrice = finalPrice
            };
        }
    }
}