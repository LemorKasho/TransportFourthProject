using Microsoft.EntityFrameworkCore;
using TransportFourthProject.Api.Data;
using TransportFourthProject.Api.DTOs.Trip;
using TransportFourthProject.Api.Models;
using TransportFourthProject.Api.Services;
using static TransportFourthProject.Api.Repositories.AdminTripRepo;

namespace TransportFourthProject.Api.Repositories
{
    public class AdminTripRepo : Repository<Trip>, IAdminTripRepo
    {
        private readonly AppDbContext _context;
        private readonly AesEncryptionService _aes;

        public AdminTripRepo(AppDbContext context, AesEncryptionService aes) : base(context)
        {
            _context = context;
            _aes = aes;
        }
        public async Task<List<TripConfirmedPassengersDto>> GetConfirmedPassengersAsync(int tripId)
        {
            var passengers = await _context.Bookings
                .Where(b => b.TripId == tripId && b.Status == Enums.BookingStatus.Confirmed)
                .Include(b => b.User)
                .Select(b => new TripConfirmedPassengersDto
                {
                    TripId = b.TripId,
                    SeatNumber = b.SeatNumber,
                    FullName = b.User.FirstName + " " + b.User.LastName,
                    NationalNumber = _aes.Decrypt(b.User.NationalNumber)
                })
                .ToListAsync();

            return passengers;
        }
    }
}
