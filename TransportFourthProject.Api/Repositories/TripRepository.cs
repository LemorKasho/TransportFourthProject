using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TransportFourthProject.Api.Data;
using TransportFourthProject.Api.DTOs.BusType;
using TransportFourthProject.Api.DTOs.City;
using TransportFourthProject.Api.DTOs.Trip;
using TransportFourthProject.Api.Enums;
using TransportFourthProject.Api.Models;
namespace TransportFourthProject.Api.Repositories
{
    public class TripRepository : Repository<Trip>, ITripRepository
    {
        private readonly AppDbContext _context;
        public TripRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Trip>> GetAllTripsAsync()
        {
            return await _context.Trips
                .Include(t => t.RoutePrice).ThenInclude(rp => rp.StartCity)
                .Include(t => t.RoutePrice).ThenInclude(rp => rp.EndCity)
                .Include(t => t.Bus).ThenInclude(b => b.BusType)
                .Include(t => t.Employee)
                .Include(t => t.TripDiscount)
                .Where(t =>
                    t.DepartureTime > DateTime.Now.AddHours(4) && 
                    !t.IsDeleted &&                               
                    t.IsArrived != true
                    )
                .ToListAsync();
        }

        public async Task<IEnumerable<Trip>> SearchTripsAsync(
            string? startCity, string? endCity,
            DateTime? date, bool hasTime, bool hasMinutes,
            string? busType, string? sortBy, string? order)
        {
            var query = _context.Trips
                .Include(t => t.Bus).ThenInclude(b => b.BusType)
                .Include(t => t.RoutePrice).ThenInclude(rp => rp.StartCity)
                .Include(t => t.RoutePrice).ThenInclude(rp => rp.EndCity)
                .Include(t => t.Employee)
                .Include(t => t.TripDiscount)
                .Where(t =>
                    t.DepartureTime > DateTime.Now.AddHours(4) &&   
                    !t.IsDeleted &&                                 
                    t.IsArrived != true                        
                )
                .AsQueryable();

            if (date.HasValue)
            {
                if (hasMinutes)
                {
                    query = query.Where(t =>
                        t.DepartureTime.Date == date.Value.Date &&
                        t.DepartureTime.Hour == date.Value.Hour &&
                        t.DepartureTime.Minute == date.Value.Minute);
                }
                else if (hasTime)
                {
                    query = query.Where(t =>
                        t.DepartureTime.Date == date.Value.Date &&
                        t.DepartureTime.Hour == date.Value.Hour);
                }
                else
                {
                    query = query.Where(t => t.DepartureTime.Date == date.Value.Date);
                }
            }

            if (!string.IsNullOrEmpty(startCity))
                query = query.Where(t => t.RoutePrice.StartCity.Name == startCity);

            if (!string.IsNullOrEmpty(endCity))
                query = query.Where(t => t.RoutePrice.EndCity.Name == endCity);

            if (!string.IsNullOrEmpty(busType))
                query = query.Where(t => t.Bus.BusType.Type == busType);

            if (!string.IsNullOrEmpty(sortBy))
            {
                bool desc = order?.ToLower() == "desc";
                var sortFields = sortBy.Split(',');

                IOrderedQueryable<Trip>? orderedQuery = null;

                foreach (var field in sortFields)
                {
                    switch (field.ToLower())
                    {
                        case "date":
                            orderedQuery = orderedQuery == null
                                ? (desc ? query.OrderByDescending(t => t.DepartureTime)
                                        : query.OrderBy(t => t.DepartureTime))
                                : (desc ? orderedQuery.ThenByDescending(t => t.DepartureTime)
                                        : orderedQuery.ThenBy(t => t.DepartureTime));
                            break;

                        case "startcity":
                            orderedQuery = orderedQuery == null
                                ? (desc ? query.OrderByDescending(t => t.RoutePrice.StartCity.Name)
                                        : query.OrderBy(t => t.RoutePrice.StartCity.Name))
                                : (desc ? orderedQuery.ThenByDescending(t => t.RoutePrice.StartCity.Name)
                                        : orderedQuery.ThenBy(t => t.RoutePrice.StartCity.Name));
                            break;

                        case "endcity":
                            orderedQuery = orderedQuery == null
                                ? (desc ? query.OrderByDescending(t => t.RoutePrice.EndCity.Name)
                                        : query.OrderBy(t => t.RoutePrice.EndCity.Name))
                                : (desc ? orderedQuery.ThenByDescending(t => t.RoutePrice.EndCity.Name)
                                        : orderedQuery.ThenBy(t => t.RoutePrice.EndCity.Name));
                            break;
                        case "bustype":
                            orderedQuery = orderedQuery == null
                                ? (desc ? query.OrderByDescending(t => t.Bus.BusType.Type)
                                        : query.OrderBy(t => t.Bus.BusType.Type))
                                : (desc ? orderedQuery.ThenByDescending(t => t.Bus.BusType.Type)
                                        : orderedQuery.ThenBy(t => t.Bus.BusType.Type));
                            break;
                    }
                }

                if (orderedQuery != null)
                    query = orderedQuery;
            }

            return await query.ToListAsync();
        }

        public async Task<Trip?> GetTripDetailsAsync(int tripId)
        {
            return await _context.Trips
                .Include(t => t.Bus).ThenInclude(b => b.BusType)
                .Include(t => t.RoutePrice).ThenInclude(rp => rp.StartCity)
                .Include(t => t.RoutePrice).ThenInclude(rp => rp.EndCity)
                .Include(t => t.Employee)
                .Include(t => t.TripDiscount)
                .Where(t =>
                    t.Id == tripId &&
                    t.DepartureTime > DateTime.Now.AddHours(4) &&
                    t.IsArrived != true &&
                    !t.IsDeleted               
                )
                .FirstOrDefaultAsync();
        }

        public async Task<int> GetBookedSeatsAsync(int tripId)
        {
            return await _context.Bookings
                .Where(b => b.TripId == tripId &&
                            (b.Status == BookingStatus.Confirmed ||
                            b.Status == BookingStatus.PendingPayment))
                .CountAsync();
        }

        public async Task<List<SeatStatusDto>> GetTripSeatsAsync(int tripId)
        {
            var trip = await GetTripDetailsAsync(tripId);
            if (trip == null)
                return new List<SeatStatusDto>();

            int capacity = trip.Bus.BusType.Capacity;

            var bookings = await _context.Bookings
                .Where(b => b.TripId == tripId &&
                            (b.Status == BookingStatus.PendingPayment ||
                             b.Status == BookingStatus.Confirmed))
                .ToListAsync();

            var seats = new List<SeatStatusDto>();

            for (int seat = 1; seat <= capacity; seat++)
            {
                var booking = bookings.FirstOrDefault(b => b.SeatNumber == seat);

                SeatStatus status = booking?.SeatStatus ?? SeatStatus.Available;

                seats.Add(new SeatStatusDto
                {
                    SeatNumber = seat,
                    Status = status
                });
            }
            return seats;
        }

        public async Task<List<int>> GetAvailableSeatsAsync(int tripId)
        {
            var trip = await GetTripDetailsAsync(tripId);
            if (trip == null)
                return new List<int>();

            int capacity = trip.Bus.BusType.Capacity;

            var bookedSeats = await _context.Bookings
                .Where(b => b.TripId == tripId &&
                            (b.Status == BookingStatus.PendingPayment ||
                             b.Status == BookingStatus.Confirmed))
                .Select(b => b.SeatNumber)
                .ToListAsync();

            var availableSeats = new List<int>();

            for (int seat = 1; seat <= capacity; seat++)
            {
                if (!bookedSeats.Contains(seat))
                    availableSeats.Add(seat);
            }

            return availableSeats;
        }

        public async Task<SelectSeatResponseDto> SelectSeatAsync(SelectSeatDto dto, int userId)
        {
            var trip = await GetTripDetailsAsync(dto.TripId);
            if (trip == null)
                return new SelectSeatResponseDto { Message = "Trip not available" };

            int capacity = trip.Bus.BusType.Capacity;
            if (dto.SeatNumber < 1 || dto.SeatNumber > capacity)
                return new SelectSeatResponseDto
                {
                    Message = $"Seat exceeds capacity. This bus has only {capacity} seats."
                };

            var existingBookedSeat = await _context.Bookings
                .FirstOrDefaultAsync(b =>
                    b.TripId == dto.TripId &&
                    b.SeatNumber == dto.SeatNumber &&
                    (b.Status == BookingStatus.Confirmed ||
                     b.Status == BookingStatus.PendingPayment)
                );

            if (existingBookedSeat != null)
                return new SelectSeatResponseDto
                {
                    Message = "Seat already confirmed or temporary booked"
                };

            var existingSameSeat = await _context.Bookings
                .FirstOrDefaultAsync(b =>
                    b.TripId == dto.TripId &&
                    b.UserId == userId &&
                    b.SeatNumber == dto.SeatNumber &&
                    b.Status == BookingStatus.Created
                );

            if (existingSameSeat != null)
            {
                return new SelectSeatResponseDto
                {
                    BookingId = existingSameSeat.Id,
                    SeatNumber = existingSameSeat.SeatNumber,
                    Message = "You already selected this seat"
                };
            }

            var existingDifferentSeat = await _context.Bookings
                .FirstOrDefaultAsync(b =>
                    b.TripId == dto.TripId &&
                    b.UserId == userId &&
                    b.Status == BookingStatus.Created
                );

            if (existingDifferentSeat != null)
            {
                existingDifferentSeat.SeatNumber = dto.SeatNumber;
                await _context.SaveChangesAsync();

                return new SelectSeatResponseDto
                {
                    BookingId = existingDifferentSeat.Id,
                    SeatNumber = existingDifferentSeat.SeatNumber,
                    Message = "Seat updated successfully"
                };
            }

            var booking = new Booking
            {
                TripId = dto.TripId,
                SeatNumber = dto.SeatNumber,
                UserId = userId,
                Status = BookingStatus.Created,
                SeatStatus = SeatStatus.Available,
                BookingTime = DateTime.Now,
                BookingReference = Guid.NewGuid().ToString()
            };

            _context.Bookings.Add(booking);
            await _context.SaveChangesAsync();

            return new SelectSeatResponseDto
            {
                BookingId = booking.Id,
                SeatNumber = booking.SeatNumber,
                Message = "Seat selected successfully"
            };
        }
    }
}