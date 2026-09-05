using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TransportFourthProject.Api.Data;
using TransportFourthProject.Api.DTOs.Bus;
using TransportFourthProject.Api.DTOs.BusType;
using TransportFourthProject.Api.DTOs.City;
using TransportFourthProject.Api.DTOs.Employee;
using TransportFourthProject.Api.DTOs.Trip;
using TransportFourthProject.Api.Enums;
using TransportFourthProject.Api.Models;

namespace TransportFourthProject.Api.Repositories
{
    public class EmployeeTripRepository : Repository<Trip>, IEmployeeTripRepository
    {
        private readonly AppDbContext _context;
        public EmployeeTripRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }
        public async Task<IEnumerable<Trip>> GetAllTripsForEmployeeAsync()
        {
            return await _context.Trips
                .Include(t => t.RoutePrice)
                    .ThenInclude(rp => rp.StartCity)
                .Include(t => t.RoutePrice)
                  .ThenInclude(rp => rp.EndCity)
                .Include(t => t.Bus)
                    .ThenInclude(b => b.BusType)
                .Include(t => t.TripDiscount)
                .Where(t => t.DepartureTime > DateTime.Now &&
                       !t.RoutePrice.IsDeleted &&
                       !t.Bus.BusType.IsDeleted &&
                       t.Bus.Status == BusStatus.Active &&
                       t.Employee.Status == EmployeeStatus.Active)
                .ToListAsync();
        }
        public async Task<int> GetBookedSeatsForEmployeeAsync(int tripId)
        {
            return await _context.Bookings
                .Where(b => b.TripId == tripId &&
                            (b.Status == BookingStatus.Confirmed ||
                            b.Status == BookingStatus.PendingPayment))
                .CountAsync();
        }
        public async Task<Trip?> GetTripDetailsForEmployeeAsync(int tripId)
        {
            return await _context.Trips
                .Include(t => t.Bus)
                    .ThenInclude(b => b.BusType)
                .Include(t => t.RoutePrice)
                    .ThenInclude(rp => rp.StartCity)
                .Include(t => t.RoutePrice)
                    .ThenInclude(rp => rp.EndCity)
                .Include(t => t.Employee)
                .Include(t => t.TripDiscount)
                .Include(t => t.Bookings)
                .FirstOrDefaultAsync(t => t.Id == tripId);
        }
        public async Task<IEnumerable<Trip>> SearchTripsForEmployeeAsync(
            string? startCity, string? endCity,
            DateTime? date, bool hasTime, bool hasMinutes,
            string? busType, string? sortBy, string? order,
            string? status, int? driverId, int? busId,
            bool? hasDiscount, string? capacityStatus)
        {
            var query = _context.Trips
                .Include(t => t.Bus).ThenInclude(b => b.BusType)
                .Include(t => t.Employee)
                .Include(t => t.RoutePrice).ThenInclude(rp => rp.StartCity)
                .Include(t => t.RoutePrice).ThenInclude(rp => rp.EndCity)
                .Include(t => t.TripDiscount)
                .Include(t => t.Bookings)
                .Where(t =>
                    t.DepartureTime > DateTime.Now &&           
                    !t.RoutePrice.IsDeleted &&                   
                    !t.Bus.BusType.IsDeleted &&                  
                    t.Bus.Status == BusStatus.Active &&        
                    t.Employee.Status == EmployeeStatus.Active  
                )
                .AsQueryable();

            if (!string.IsNullOrEmpty(status))
            {
                switch (status.ToLower())
                {
                    case "active":
                        query = query.Where(t => !t.IsDeleted);
                        break;
                    case "deleted":
                        query = query.Where(t => t.IsDeleted);
                        break;
                }
            }

            if (driverId.HasValue)
                query = query.Where(t => t.EmployeeId == driverId.Value);

            if (busId.HasValue)
                query = query.Where(t => t.BusId == busId.Value);

            if (hasDiscount.HasValue)
            {
                query = hasDiscount.Value
                    ? query.Where(t => t.TripDiscountId != null)
                    : query.Where(t => t.TripDiscountId == null);
            }

            if (!string.IsNullOrEmpty(capacityStatus))
            {
                switch (capacityStatus.ToLower())
                {
                    case "full":
                        query = query.Where(t =>
                            t.Bookings.Count(b =>
                                b.Status == BookingStatus.Confirmed ||
                                b.Status == BookingStatus.PendingPayment)
                            >= t.Bus.BusType.Capacity);
                        break;

                    case "available":
                        query = query.Where(t =>
                            t.Bookings.Count(b =>
                                b.Status == BookingStatus.Confirmed ||
                                b.Status == BookingStatus.PendingPayment)
                            < t.Bus.BusType.Capacity);
                        break;
                }
            }

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
                    query = query.Where(t =>
                        t.DepartureTime.Date == date.Value.Date);
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
                        case "startcity":
                            orderedQuery = orderedQuery == null
                                ? (desc ? query.OrderByDescending(t => t.RoutePrice.StartCity.Name)
                                        : query.OrderBy(t => t.RoutePrice.StartCity.Name))
                                : (desc ? orderedQuery.ThenByDescending(t => t.RoutePrice.StartCity.Name)
                                        : orderedQuery.ThenBy(t => t.RoutePrice.StartCity.Name));
                            break;

                        case "bustype":
                            orderedQuery = orderedQuery == null
                                ? (desc ? query.OrderByDescending(t => t.Bus.BusType.Type)
                                        : query.OrderBy(t => t.Bus.BusType.Type))
                                : (desc ? orderedQuery.ThenByDescending(t => t.Bus.BusType.Type)
                                        : orderedQuery.ThenBy(t => t.Bus.BusType.Type));
                            break;

                        case "date":
                            orderedQuery = orderedQuery == null
                                ? (desc ? query.OrderByDescending(t => t.DepartureTime)
                                        : query.OrderBy(t => t.DepartureTime))
                                : (desc ? orderedQuery.ThenByDescending(t => t.DepartureTime)
                                        : orderedQuery.ThenBy(t => t.DepartureTime));
                            break;

                        case "endcity":
                            orderedQuery = orderedQuery == null
                                ? (desc ? query.OrderByDescending(t => t.RoutePrice.EndCity.Name)
                                        : query.OrderBy(t => t.RoutePrice.EndCity.Name))
                                : (desc ? orderedQuery.ThenByDescending(t => t.RoutePrice.EndCity.Name)
                                        : orderedQuery.ThenBy(t => t.RoutePrice.EndCity.Name));
                            break;
                    }
                }

                if (orderedQuery != null)
                    query = orderedQuery;
            }

            return await query.ToListAsync();
        }
        public async Task<TripSearchOptionsDto> GetTripSearchOptionsForEmployeeAsync()
        {
            var cities = await _context.Cities
                .Select(c => new CityDto
                {
                    Id = c.Id,
                    Name = c.Name
                })
                .ToListAsync();

            var busTypes = await _context.BusTypes
                .Where(bt => !bt.IsDeleted)
                .Select(bt => new BusTypeDto
                {
                    BusTypeId = bt.Id,
                    Name = bt.Type
                })
                .ToListAsync();
            busTypes.Insert(0, new BusTypeDto
            {
                BusTypeId = 0,
                Name = "all"
            });

            var drivers = await _context.Employees
                .Where(e => e.Role == EmployeeRole.Driver &&
                            e.Status == EmployeeStatus.Active)
                .Select(e => new DriverForSearchTripDto
                {
                    EmployeeId = e.Id,
                    FullName = e.FirstName + " " + e.LastName,
                })
                .ToListAsync();

            var buses = await _context.Buses
                .Include(b => b.BusType)
                .Where(b => b.Status == BusStatus.Active && 
                      !b.BusType.IsDeleted)
                .Select(b => new BusForSearchTripDto
                {
                    BusId = b.Id,
                    BusNumber = b.BusNumber,
                    BusTypeName = b.BusType.Type
                })
                .ToListAsync();

            var statuses = new List<string>
             {
                 "active",
                 "deleted",
                 "all"
             };

            var capacityStatuses = new List<string>
             {
                 "full",
                 "available",
                 "all"
             };

            var discountOptions = new List<string>
              {
                  "true",
                  "false"
              };

            var sortFields = new List<string>
            {
                 "date",
                 "startcity",
                 "endcity",
                 "bustype"
            };
            var orderOptions = new List<string>
            {
                "asc",
                "desc"
            };

            return new TripSearchOptionsDto
            {
                Cities = cities,
                BusTypes = busTypes,
                Drivers = drivers,
                Buses = buses,

                Statuses = statuses,
                CapacityStatuses = capacityStatuses,
                DiscountOptions = discountOptions,
                SortFields = sortFields,
                OrderOptions = orderOptions
            };
        }
        public async Task<List<Employee>> GetAvailableDriversAsync(DateTime departureTime)
        {
            var drivers = await _context.Employees
                .Where(e => e.Role == EmployeeRole.Driver &&
                e.Status == EmployeeStatus.Active)
                .ToListAsync();

            var availableDrivers = new List<Employee>();

            foreach (var driver in drivers)
            {
                var lastTrip = await _context.Trips
                    .Include(t => t.RoutePrice)
                    .Include(t => t.Bus).ThenInclude(b => b.BusType)
                    .Where(t => t.EmployeeId == driver.Id &&
                           !t.IsDeleted && 
                           !t.RoutePrice.IsDeleted &&
                           !t.Bus.BusType.IsDeleted &&
                           t.Bus.Status == BusStatus.Active)
                    .OrderByDescending(t => t.ArrivalTime)
                    .FirstOrDefaultAsync();
                if(lastTrip == null)
                {
                    availableDrivers.Add(driver);
                    continue;
                }
                bool conflict = 
                    departureTime < lastTrip.ArrivalTime &&
                    departureTime.AddHours(1) > lastTrip.DepartureTime;

                if (conflict) continue;

                bool restEnough = (departureTime - lastTrip.ArrivalTime).TotalHours >= 8;

                if (!restEnough) continue;

                availableDrivers.Add(driver);
            }
            return availableDrivers;
        }
        public async Task<List<Bus>> GetAvailableBusesAsync(DateTime departureTime, int busTypeId)
        {
            var buses = await _context.Buses
                .Where(b => b.Status == BusStatus.Active && b.BusTypeId == busTypeId &&
                !b.BusType.IsDeleted)
                .Include(b => b.BusType)
                .ToListAsync();

            var availableBuses = new List<Bus>();

            foreach (var bus in buses)
            {
                var lastTrip = await _context.Trips
                    .Include(t => t.RoutePrice)
                    .Include(t => t.Bus).ThenInclude(t => t.BusType)
                    .Include(t => t.Employee)
                    .Where(t =>
                            t.BusId == bus.Id &&
                            !t.IsDeleted &&                       
                            !t.RoutePrice.IsDeleted &&            
                            !t.Bus.BusType.IsDeleted &&         
                            t.Bus.Status == BusStatus.Active && 
                            t.Employee.Status == EmployeeStatus.Active)
                    .OrderByDescending(t => t.ArrivalTime)
                    .FirstOrDefaultAsync();

                if (lastTrip == null)
                {
                    availableBuses.Add(bus);
                    continue;
                }

                bool conflict =
                    departureTime < lastTrip.ArrivalTime &&
                    departureTime.AddHours(1) > lastTrip.DepartureTime;

                if (conflict)
                    continue;

                bool restEnough =
                    (departureTime - lastTrip.ArrivalTime).TotalHours >= 8;

                if (!restEnough)
                    continue;

                availableBuses.Add(bus);
            }

            return availableBuses;
        }
        public async Task<List<TripDiscount>> GetAvailableTripDiscountsAsync()
        {
            return await _context.TripDiscounts
                .Where(td => td.Status == TripDiscountStatus.Active)
                .ToListAsync();
        }
        public async Task<List<RoutePrice>> GetRoutePricesAsync()
        {
            return await _context.RoutePrices
                .Include(rp => rp.StartCity)
                .Include(rp => rp.EndCity)
                .Include(rp => rp.BusType)
                .Where(rp =>
                       !rp.IsDeleted &&
                       !rp.BusType.IsDeleted)
                .ToListAsync();
        }
        public async Task<Trip> AddTripForEmployeeAsync(EmployeeAddTripDto dto)
        {
            // check for routePrice
            var route = await _context.RoutePrices
                .Include(r => r.BusType)
                .FirstOrDefaultAsync(r => r.Id == dto.RoutePriceId);

            if (route == null)
                throw new Exception("Route not found");

            if (route.IsDeleted)
                throw new Exception("Route is deleted");

            if (route.BusType.IsDeleted)
                throw new Exception("Bus type for this route is deleted");

            // calculate arrival time
            var arrivalTime = dto.DepartureTime.AddHours(route.DurationHours);

            // check for bus
            var bus = await _context.Buses
                .Include(b => b.BusType)
                .FirstOrDefaultAsync(b => b.Id == dto.BusId);

            if (bus == null)
                throw new Exception("Bus not found");

            if (bus.Status != BusStatus.Active)
                throw new Exception("Bus is not active");

            if (bus.BusType.IsDeleted)
                throw new Exception("Bus type is deleted");

            if (bus.BusTypeId != route.BusTypeId)
                throw new Exception("Bus type does not match the route requirements");

            // check last bus trip
            var lastBusTrip = await _context.Trips
                .Include(t => t.RoutePrice)
                .Include(t => t.Bus).ThenInclude(b => b.BusType)
                .Include(t => t.Employee)
                .Where(t =>
                    t.BusId == bus.Id &&
                    !t.IsDeleted &&
                    !t.RoutePrice.IsDeleted &&
                    !t.Bus.BusType.IsDeleted &&
                    t.Bus.Status == BusStatus.Active &&
                    t.Employee.Status == EmployeeStatus.Active
                )
                .OrderByDescending(t => t.ArrivalTime)
                .FirstOrDefaultAsync();

            if (lastBusTrip != null)
            {
                bool busConflict =
                    dto.DepartureTime < lastBusTrip.ArrivalTime &&
                    arrivalTime > lastBusTrip.DepartureTime;

                if (busConflict)
                    throw new Exception("Bus is already assigned to another trip at this time");

                bool busRestEnough =
                    (dto.DepartureTime - lastBusTrip.ArrivalTime).TotalHours >= 8;

                if (!busRestEnough)
                    throw new Exception("Bus must rest 8 hours before starting a new trip");
            }

            // check for driver
            var driver = await _context.Employees
                .FirstOrDefaultAsync(e => e.Id == dto.EmployeeId);

            if (driver == null)
                throw new Exception("Driver not found");

            if (driver.Role != EmployeeRole.Driver)
                throw new Exception("Selected employee is not a driver");

            if (driver.Status != EmployeeStatus.Active)
                throw new Exception("Driver is not active");

            // check last driver trip
            var lastDriverTrip = await _context.Trips
                .Include(t => t.RoutePrice)
                .Include(t => t.Bus).ThenInclude(b => b.BusType)
                .Include(t => t.Employee)
                .Where(t =>
                    t.EmployeeId == driver.Id &&
                    !t.IsDeleted &&
                    !t.RoutePrice.IsDeleted &&
                    !t.Bus.BusType.IsDeleted &&
                    t.Bus.Status == BusStatus.Active &&
                    t.Employee.Status == EmployeeStatus.Active
                )
                .OrderByDescending(t => t.ArrivalTime)
                .FirstOrDefaultAsync();

            if (lastDriverTrip != null)
            {
                bool driverConflict =
                    dto.DepartureTime < lastDriverTrip.ArrivalTime &&
                    arrivalTime > lastDriverTrip.DepartureTime;

                if (driverConflict)
                    throw new Exception("Driver has another trip at this time");

                bool driverRestEnough =
                    (dto.DepartureTime - lastDriverTrip.ArrivalTime).TotalHours >= 8;

                if (!driverRestEnough)
                    throw new Exception("Driver must rest 8 hours before starting a new trip");
            }

            // check for tripDiscount
            if (dto.TripDiscountId.HasValue)
            {
                var discount = await _context.TripDiscounts
                    .FirstOrDefaultAsync(d => d.Id == dto.TripDiscountId);

                if (discount == null)
                    throw new Exception("Discount not found");

                if (discount.Status != TripDiscountStatus.Active)
                    throw new Exception("Discount is expired");
            }
            // add a new trip
            var trip = new Trip
            {
                RoutePriceId = dto.RoutePriceId,
                DepartureTime = dto.DepartureTime,
                ArrivalTime = arrivalTime,
                BusId = dto.BusId,
                EmployeeId = dto.EmployeeId,
                TripDiscountId = dto.TripDiscountId,
                IsDeleted = false
            };

            _context.Trips.Add(trip);
            await _context.SaveChangesAsync();

            return trip;
        }
        public async Task<TripPatchResultDto> PatchTripForEmployeeAsync([FromBody] EmployeePatchTripDto dto)
        {
            // Basic validations
            if (dto.TripId <= 0)
                return new TripPatchResultDto { Success = false, ErrorField = "TripId", Message = "Trip ID must be greater than zero.", Suggestion = "Provide a valid trip ID." };

            if (dto.EmployeeId != null && dto.EmployeeId <= 0)
                return new TripPatchResultDto { Success = false, ErrorField = "EmployeeId", Message = "Invalid driver id", Suggestion = "EmployeeId must be greater than 0" };

            if (dto.BusId != null && dto.BusId <= 0)
                return new TripPatchResultDto { Success = false, ErrorField = "BusId", Message = "Invalid bus id", Suggestion = "BusId must be greater than 0" };

            if (dto.TripDiscountId != null && dto.TripDiscountId <= 0)
                return new TripPatchResultDto { Success = false, ErrorField = "TripDiscountId", Message = "Invalid discount id", Suggestion = "TripDiscountId must be greater than 0" };

            if (dto.DepartureTime != null && dto.DepartureTime <= DateTime.Now)
                return new TripPatchResultDto { Success = false, ErrorField = "DepartureTime", Message = "Departure time must be in the future", Suggestion = "Choose a future time" };

            var trip = await _context.Trips
                .Include(t => t.RoutePrice)
                .Include(t => t.Employee)
                .Include(t => t.Bus).ThenInclude(b => b.BusType)
                .Include(t => t.TripDiscount)
                .FirstOrDefaultAsync(t => t.Id == dto.TripId);

            if (trip == null)
                return new TripPatchResultDto { Success = false, ErrorField = "TripId", Message = "Trip not found", Suggestion = "Check trip ID" };

            var newDepartureTime = dto.DepartureTime ?? trip.DepartureTime;
            var newArrivalTime = newDepartureTime.AddHours(trip.RoutePrice.DurationHours);
            if (dto.EmployeeId != null)
            {
                var driver = await _context.Employees
                    .FirstOrDefaultAsync(e => e.Id == dto.EmployeeId &&
                                              e.Role == EmployeeRole.Driver &&
                                              e.Status == EmployeeStatus.Active);

                if (driver == null)
                    return new TripPatchResultDto { Success = false, ErrorField = "EmployeeId", Message = "Selected driver is not valid or not active.", Suggestion = "Choose another driver." };

                var lastDriverTrip = await _context.Trips
                .Include(t => t.RoutePrice)
                .Include(t => t.Bus).ThenInclude(b => b.BusType)
                .Include(t => t.Employee)
                .Where(t =>
                    t.EmployeeId == driver.Id &&
                    !t.IsDeleted &&
                    !t.RoutePrice.IsDeleted &&
                    !t.Bus.BusType.IsDeleted &&
                    t.Bus.Status == BusStatus.Active &&
                    t.Employee.Status == EmployeeStatus.Active &&
                    t.Id != trip.Id
                )
                .OrderByDescending(t => t.ArrivalTime)
                .FirstOrDefaultAsync();

                if (lastDriverTrip != null)
                {
                    bool conflict =
                        newDepartureTime < lastDriverTrip.ArrivalTime &&
                        newArrivalTime > lastDriverTrip.DepartureTime;

                    if (conflict)
                        return new TripPatchResultDto { Success = false, ErrorField = "EmployeeId", Message = "Driver has a conflicting trip at the selected departure time.", Suggestion = "Please choose another driver." };

                    bool restEnough =
                        (newDepartureTime - lastDriverTrip.ArrivalTime).TotalHours >= 8;

                    if (!restEnough)
                        return new TripPatchResultDto { Success = false, ErrorField = "EmployeeId", Message = "Driver does not have enough rest time before this trip.", Suggestion = "Choose another driver or change departure time." };
                }

                trip.EmployeeId = dto.EmployeeId.Value;
            }
            if (dto.BusId != null)
            {
                var bus = await _context.Buses
                    .Include(b => b.BusType)
                    .FirstOrDefaultAsync(b => b.Id == dto.BusId &&
                                              b.Status == BusStatus.Active);
                if (bus == null)
                    return new TripPatchResultDto { Success = false, ErrorField = "BusId", Message = "Selected bus is not valid or not active.", Suggestion = "Choose another bus." };

                if (bus.BusTypeId != trip.RoutePrice.BusTypeId)
                    return new TripPatchResultDto { Success = false, ErrorField = "BusId", Message = "Bus type does not match route requirements.", Suggestion = "Choose a bus with the correct type." };

                var lastBusTrip = await _context.Trips
                .Include(t => t.RoutePrice)
                .Include(t => t.Bus).ThenInclude(b => b.BusType)
                .Include(t => t.Employee)
                .Where(t =>
                    t.BusId == bus.Id &&
                    !t.IsDeleted &&
                    !t.RoutePrice.IsDeleted &&
                    !t.Bus.BusType.IsDeleted &&
                    t.Bus.Status == BusStatus.Active &&
                    t.Employee.Status == EmployeeStatus.Active &&
                    t.Id != trip.Id
                )
                .OrderByDescending(t => t.ArrivalTime)
                .FirstOrDefaultAsync();

                if (lastBusTrip != null)
                {
                    bool conflict =
                        newDepartureTime < lastBusTrip.ArrivalTime &&
                        newArrivalTime > lastBusTrip.DepartureTime;

                    if (conflict)
                        return new TripPatchResultDto { Success = false, ErrorField = "BusId", Message = "Bus is not available at the selected departure time.", Suggestion = "Please choose another bus." };

                    bool restEnough =
                        (newDepartureTime - lastBusTrip.ArrivalTime).TotalHours >= 8;

                    if (!restEnough)
                        return new TripPatchResultDto { Success = false, ErrorField = "BusId", Message = "Bus does not have enough rest time before this trip.", Suggestion = "Choose another bus or change departure time." };
                }

                trip.BusId = dto.BusId.Value;
            }
            if (dto.DepartureTime != null)
            {
                // Check driver
                var lastDriverTrip = await _context.Trips
                    .Include(t => t.RoutePrice)
                    .Include(t => t.Bus).ThenInclude(b => b.BusType)
                    .Include(t => t.Employee)
                    .Where(t =>
                        t.EmployeeId == trip.EmployeeId &&
                        !t.IsDeleted &&
                        !t.RoutePrice.IsDeleted &&
                        !t.Bus.BusType.IsDeleted &&
                        t.Bus.Status == BusStatus.Active &&
                        t.Employee.Status == EmployeeStatus.Active &&
                        t.Id != trip.Id
                    )
                    .OrderByDescending(t => t.ArrivalTime)
                    .FirstOrDefaultAsync();

                if (lastDriverTrip != null)
                {
                    bool conflict =
                        newDepartureTime < lastDriverTrip.ArrivalTime &&
                        newArrivalTime > lastDriverTrip.DepartureTime;

                    if (conflict)
                        return new TripPatchResultDto { Success = false, ErrorField = "DepartureTime", Message = "Driver has a conflicting trip at the new departure time.", Suggestion = "Change the driver or choose another time." };

                    bool restEnough =
                        (newDepartureTime - lastDriverTrip.ArrivalTime).TotalHours >= 8;

                    if (!restEnough)
                        return new TripPatchResultDto { Success = false, ErrorField = "DepartureTime", Message = "Driver does not have enough rest time before this trip.", Suggestion = "Change the driver or choose another time." };
                }

                // Check bus
                var lastBusTrip = await _context.Trips
                .Include(t => t.RoutePrice)
                .Include(t => t.Bus).ThenInclude(b => b.BusType)
                .Include(t => t.Employee)
                .Where(t =>
                    t.BusId == trip.BusId &&
                    !t.IsDeleted &&
                    !t.RoutePrice.IsDeleted &&
                    !t.Bus.BusType.IsDeleted &&
                    t.Bus.Status == BusStatus.Active &&
                    t.Employee.Status == EmployeeStatus.Active &&
                    t.Id != trip.Id
                )
                .OrderByDescending(t => t.ArrivalTime)
                .FirstOrDefaultAsync();

                if (lastBusTrip != null)
                {
                    bool conflict =
                        newDepartureTime < lastBusTrip.ArrivalTime &&
                        newArrivalTime > lastBusTrip.DepartureTime;

                    if (conflict)
                        return new TripPatchResultDto { Success = false, ErrorField = "DepartureTime", Message = "Bus has a conflicting trip at the new departure time.", Suggestion = "Change the bus or choose another time." };

                    bool restEnough =
                        (newDepartureTime - lastBusTrip.ArrivalTime).TotalHours >= 8;

                    if (!restEnough)
                        return new TripPatchResultDto { Success = false, ErrorField = "DepartureTime", Message = "Bus does not have enough rest time before this trip.", Suggestion = "Change the bus or choose another time." };
                }

                trip.DepartureTime = newDepartureTime;
                trip.ArrivalTime = newArrivalTime;
            }
            if (dto.TripDiscountId != null)
            {
                var discount = await _context.TripDiscounts
                    .FirstOrDefaultAsync(d => d.Id == dto.TripDiscountId &&
                                              d.Status == TripDiscountStatus.Active);

                if (discount == null)
                    return new TripPatchResultDto { Success = false, ErrorField = "TripDiscountId", Message = "Selected discount is not active.", Suggestion = "Choose another discount." };

                trip.TripDiscountId = dto.TripDiscountId.Value;
            }

            await _context.SaveChangesAsync();

            return new TripPatchResultDto
            {
                Success = true,
                Message = "Trip updated successfully"
            };
        }
        public async Task<TripEditDataDto> GetTripEditDataAsync(int tripId)
        {
            var trip = await _context.Trips
                .Include(t => t.RoutePrice).ThenInclude(rp => rp.StartCity)
                .Include(t => t.RoutePrice).ThenInclude(rp => rp.EndCity)
                .Include(t => t.Employee)
                .Include(t => t.Bus).ThenInclude(b => b.BusType)
                .Include(t => t.TripDiscount)
                .FirstOrDefaultAsync(t => t.Id == tripId);

            if (trip == null)
                return null;

            if (trip.RoutePrice.IsDeleted ||
                trip.Bus.BusType.IsDeleted ||
                trip.Bus.Status != BusStatus.Active ||
                trip.Employee.Status != EmployeeStatus.Active)
            {
                return null;
            }

            var tripDetailsForEditTrip = new TripDetailsForEditTripDto
            {
                TripId = trip.Id,
                EmployeeId = trip.EmployeeId,
                BusId = trip.BusId,
                DepartureTime = trip.DepartureTime,
                ArrivalTime = trip.ArrivalTime,
                TripDiscountId = trip.TripDiscountId,
                StartCityName = trip.RoutePrice.StartCity.Name,
                EndCityName = trip.RoutePrice.EndCity.Name,
                Price = trip.RoutePrice.Price
            };

            var allDrivers = await _context.Employees
                .Where(e => e.Role == EmployeeRole.Driver &&
                            e.Status == EmployeeStatus.Active)
                .ToListAsync();

            var availableDrivers = new List<DriverDto>();

            foreach (var driver in allDrivers)
            {
                var lastDriverTrip = await _context.Trips
                    .Include(t => t.RoutePrice)
                    .Include(t => t.Bus).ThenInclude(b => b.BusType)
                    .Include(t => t.Employee)
                    .Where(t =>
                        t.EmployeeId == driver.Id &&
                        !t.IsDeleted &&
                        !t.RoutePrice.IsDeleted &&
                        !t.Bus.BusType.IsDeleted &&
                        t.Bus.Status == BusStatus.Active &&
                        t.Employee.Status == EmployeeStatus.Active &&
                        t.Id != trip.Id
                    )
                    .OrderByDescending(t => t.ArrivalTime)
                    .FirstOrDefaultAsync();

                bool conflict = false;
                bool restEnough = true;

                if (lastDriverTrip != null)
                {
                    conflict =
                        trip.DepartureTime < lastDriverTrip.ArrivalTime &&
                        trip.ArrivalTime > lastDriverTrip.DepartureTime;

                    restEnough =
                        (trip.DepartureTime - lastDriverTrip.ArrivalTime).TotalHours >= 8;
                }

                if (!conflict && restEnough)
                {
                    availableDrivers.Add(new DriverDto
                    {
                        EmployeeId = driver.Id,
                        FullName = driver.FirstName + " " + driver.LastName
                    });
                }
            }

            var allBuses = await _context.Buses
                .Include(b => b.BusType)
                .Where(b =>
                    b.Status == BusStatus.Active &&
                    !b.BusType.IsDeleted &&
                    b.BusTypeId == trip.RoutePrice.BusTypeId)
                .ToListAsync();

            var availableBuses = new List<BusDto>();

            foreach (var bus in allBuses)
            {
                var lastBusTrip = await _context.Trips
                    .Include(t => t.RoutePrice)
                    .Include(t => t.Bus).ThenInclude(b => b.BusType)
                    .Include(t => t.Employee)
                    .Where(t =>
                        t.BusId == bus.Id &&
                        !t.IsDeleted &&
                        !t.RoutePrice.IsDeleted &&
                        !t.Bus.BusType.IsDeleted &&
                t.Bus.Status == BusStatus.Active &&
                t.Employee.Status == EmployeeStatus.Active &&
                t.Id != trip.Id
            )
            .OrderByDescending(t => t.ArrivalTime)
            .FirstOrDefaultAsync();

                bool conflict = false;
                bool restEnough = true;

                if (lastBusTrip != null)
                {
                    conflict =
                        trip.DepartureTime < lastBusTrip.ArrivalTime &&
                        trip.ArrivalTime > lastBusTrip.DepartureTime;

                    restEnough =
                        (trip.DepartureTime - lastBusTrip.ArrivalTime).TotalHours >= 8;
                }

                if (!conflict && restEnough)
                {
                    availableBuses.Add(new BusDto
                    {
                        BusId = bus.Id,
                        BusNumber = bus.BusNumber,
                        BusTypeName = bus.BusType.Type
                    });
                }
            }

            var availableDiscounts = await _context.TripDiscounts
                .Where(d => d.Status == TripDiscountStatus.Active)
                .Select(d => new TripDiscountDto
                {
                    TripDiscountId = d.Id,
                    Name = d.Name,
                    Percentage = d.Percentage
                })
                .ToListAsync();

            return new TripEditDataDto
            {
                Trip = tripDetailsForEditTrip,
                AvailableDrivers = availableDrivers,
                AvailableBuses = availableBuses,
                AvailableDiscounts = availableDiscounts
            };
        }
        public async Task<bool> DeleteTripAsync(int tripId)
        {
            var trip = await _context.Trips
                .Include(t => t.Bookings)
                .FirstOrDefaultAsync(t => t.Id == tripId);

            if (trip == null)
                return false;

            if (trip.IsDeleted)
                return false;

            if (trip.Bookings.Any(b =>
                b.Status == BookingStatus.PendingPayment ||
                b.Status == BookingStatus.Confirmed))
            {
                return false;
            }

            trip.IsDeleted = true;
            trip.IsArrived = null;

            await _context.SaveChangesAsync();
            return true;
        }
        public async Task<List<SeatStatusWithUserDto>?> GetTripSeatsWithUsersAsync(int tripId)
        {
            var trip = await _context.Trips
                .Include(t => t.Bus).ThenInclude(b => b.BusType)
                .FirstOrDefaultAsync(t => t.Id == tripId);

            if (trip == null || trip.IsDeleted)
                return null;

            int capacity = trip.Bus.BusType.Capacity;

            var bookings = await _context.Bookings
                .Include(b => b.User)
                .Where(b => b.TripId == tripId &&
                            (b.Status == BookingStatus.Confirmed ||
                             b.Status == BookingStatus.PendingPayment)
                )
                .ToListAsync();

            var seats = new List<SeatStatusWithUserDto>();

            for (int seat = 1; seat <= capacity; seat++)
            {
                var booking = bookings.FirstOrDefault(b => b.SeatNumber == seat);

                seats.Add(new SeatStatusWithUserDto
                {
                    SeatNumber = seat,

                    SeatStatus = booking?.SeatStatus ?? SeatStatus.Available,

                    FullName = booking != null
                        ? $"{booking.User.FirstName} {booking.User.LastName}"
                        : "—"
                });
            }

            return seats;
        }
        public async Task<SeatStatusSummaryDto?> GetTripSeatStatusSummaryAsync(int tripId)
        {
            var trip = await _context.Trips
                .Include(t => t.Bus).ThenInclude(b => b.BusType)
                .FirstOrDefaultAsync(t => t.Id == tripId);

            if (trip == null || trip.IsDeleted)
                return null;

            int capacity = trip.Bus.BusType.Capacity;

            var bookings = await _context.Bookings
                .Where(b => b.TripId == tripId)
                .ToListAsync();

            int confirmed = bookings.Count(b => b.SeatStatus == SeatStatus.Confirmed);
            int reserved = bookings.Count(b => b.SeatStatus == SeatStatus.Reserved);
            int available = capacity - (confirmed + reserved);

            double availablePercent = Math.Round((double)available / capacity * 100, 2);
            double reservedPercent = Math.Round((double)reserved / capacity * 100, 2);
            double confirmedPercent = Math.Round((double)confirmed / capacity * 100, 2);

            return new SeatStatusSummaryDto
            {
                TotalSeats = capacity,
                AvailableSeats = available,
                ReservedSeats = reserved,
                ConfirmedSeats = confirmed,

                AvailablePercentage = availablePercent,
                ReservedPercentage = reservedPercent,
                ConfirmedPercentage = confirmedPercent
            };
        }
        public async Task<int> GetTripsCountInMonthAsync(int month, int year)
        {
            if (month < 1 || month > 12)
                throw new ArgumentException("Month must be between 1 and 12.");

            if (year < 2020 || year > 2070)
                throw new ArgumentException("Year must be between 2020 and 2070.");

            var count = await _context.Trips
                .Where(t =>
                    !t.IsDeleted &&             
                    t.DepartureTime.Month == month &&    
                    t.DepartureTime.Year == year         
                )
                .CountAsync();

            return count;
        }
        public async Task<int> GetTripsCountInYearAsync(int year)
        {
            if (year < 2020 || year > 2070)
                throw new ArgumentException("Year must be between 2020 and 2070.");

            var count = await _context.Trips
                .Where(t =>
                    !t.IsDeleted &&
                    t.DepartureTime.Year == year
                )
                .CountAsync();

            return count;
        }
        public async Task<List<RoutePriceTripsCountDto>> GetTripsCountByRoutePriceAsync()
        {
            var result = await _context.RoutePrices
                .Include(rp => rp.StartCity)
                .Include(rp => rp.EndCity)
                .Include(rp => rp.BusType)
                .Select(rp => new RoutePriceTripsCountDto
                {
                    RoutePriceId = rp.Id,
                    StartCityName = rp.StartCity.Name,
                    EndCityName = rp.EndCity.Name,
                    BusTypeName = rp.BusType.Type,

                    TripsCount = _context.Trips
                        .Count(t => t.RoutePriceId == rp.Id && !t.IsDeleted)
                })
                .ToListAsync();

            return result;
        }
        public async Task<List<BusTypeTripsCountDto>> GetTripsCountByBusTypeAsync()
        {
            var result = await _context.BusTypes
                .Select(bt => new BusTypeTripsCountDto
                {
                    BusTypeId = bt.Id,
                    BusTypeName = bt.Type,
                    Capacity = bt.Capacity,

                    TripsCount = _context.Trips
                        .Count(t =>
                            !t.IsDeleted &&
                            t.Bus.BusTypeId == bt.Id
                        )
                })
                .ToListAsync();

            return result;
        }
        public async Task<List<TripOnRoadDto>> GetTripsOnRoadsAsync()
        {
            DateTime now = DateTime.Now;

            var trips = await _context.Trips
                .Include(t => t.RoutePrice)
                    .ThenInclude(rp => rp.StartCity)
                .Include(t => t.RoutePrice)
                    .ThenInclude(rp => rp.EndCity)
                .Include(t => t.Bus)
                    .ThenInclude(b => b.BusType)
                .Where(t =>
                    !t.IsDeleted &&
                    now >= t.DepartureTime &&
                    now <= t.ArrivalTime
                )
                .ToListAsync(); 

            var result = trips.Select(t =>
            {
                int speed = 80;
                int totalDistance = (int)t.RoutePrice.DistanceKm;

                double elapsedHours = (now - t.DepartureTime).TotalHours;
                if (elapsedHours < 0) elapsedHours = 0;

                double distanceCovered = speed * elapsedHours;
                if (distanceCovered > totalDistance)
                    distanceCovered = totalDistance;

                double remainingDistance = totalDistance - distanceCovered;

                double etaHours = remainingDistance / speed;
                TimeSpan etaSpan = TimeSpan.FromHours(etaHours);
                string etaFormatted = $"{etaSpan.Hours}h {etaSpan.Minutes}m";

                return new TripOnRoadDto
                {
                    TripId = t.Id,
                    RoutePriceId = t.RoutePriceId,
                    StartCityName = t.RoutePrice.StartCity.Name,
                    EndCityName = t.RoutePrice.EndCity.Name,
                    BusTypeName = t.Bus.BusType.Type,
                    BusNumber = t.Bus.BusNumber,
                    DriverFullName = t.Employee.FirstName + " " + t.Employee.LastName,
                    DepartureTime = t.DepartureTime,
                    ArrivalTime = t.ArrivalTime,

                    PassengersCount = _context.Bookings
                        .Count(b =>
                            b.TripId == t.Id &&
                            b.Status == BookingStatus.Confirmed
                        ),

                    TotalDistance = totalDistance,
                    DistanceCovered = Math.Round(distanceCovered, 2),
                    RemainingDistance = Math.Round(remainingDistance, 2),
                    ETA = etaFormatted,
                    Progress = $"{Math.Round((distanceCovered / totalDistance) * 100, 2)}%"
                };
            }).ToList();

            return result;
        }
        public async Task<List<UpcomingTripDto>> GetUpcomingTripsAsync()
        {
            DateTime now = DateTime.Now;

            var trips = await _context.Trips
                .Include(t => t.RoutePrice)
                    .ThenInclude(rp => rp.StartCity)
                .Include(t => t.RoutePrice)
                    .ThenInclude(rp => rp.EndCity)
                .Include(t => t.Bus)
                    .ThenInclude(b => b.BusType)
                .Where(t =>
                    !t.IsDeleted &&
                    t.DepartureTime > now
                )
                .Select(t => new UpcomingTripDto
                {
                    TripId = t.Id,
                    RoutePriceId = t.RoutePriceId,
                    StartCityName = t.RoutePrice.StartCity.Name,
                    EndCityName = t.RoutePrice.EndCity.Name,
                    BusTypeName = t.Bus.BusType.Type,
                    BusNumber = t.Bus.BusNumber,
                    DepartureTime = t.DepartureTime,
                    ArrivalTime = t.ArrivalTime,
                })
                .ToListAsync();

            return trips;
        }
        public async Task<List<TodayTripsDto>> GetTodayTripsAsync()
        {
            DateTime today = DateTime.Today;

            var trips = await _context.Trips
                .Include(t => t.RoutePrice)
                    .ThenInclude(rp => rp.StartCity)
                .Include(t => t.RoutePrice)
                    .ThenInclude(rp => rp.EndCity)
                .Include(t => t.Bus)
                    .ThenInclude(b => b.BusType)
                .Where(t =>
                    !t.IsDeleted &&
                    t.DepartureTime.Date == today
                )
                .Select(t => new TodayTripsDto
                {
                    TripId = t.Id,
                    RoutePriceId = t.RoutePriceId,
                    StartCityName = t.RoutePrice.StartCity.Name,
                    EndCityName = t.RoutePrice.EndCity.Name,
                    BusTypeName = t.Bus.BusType.Type,
                    BusNumber = t.Bus.BusNumber,
                    DepartureTime = t.DepartureTime,
                    ArrivalTime = t.ArrivalTime
                })
                .ToListAsync();

            return trips;
        }
        public async Task<List<TodayTripsDto>> GetTomorrowTripsAsync()
        {
            DateTime tomorrow = DateTime.Today.AddDays(1);

            var trips = await _context.Trips
                .Include(t => t.RoutePrice)
                    .ThenInclude(rp => rp.StartCity)
                .Include(t => t.RoutePrice)
                    .ThenInclude(rp => rp.EndCity)
                .Include(t => t.Bus)
                    .ThenInclude(b => b.BusType)
                .Where(t =>
                    !t.IsDeleted &&
                    t.DepartureTime.Date == tomorrow
                )
                .Select(t => new TodayTripsDto
                {
                    TripId = t.Id,
                    RoutePriceId = t.RoutePriceId,
                    StartCityName = t.RoutePrice.StartCity.Name,
                    EndCityName = t.RoutePrice.EndCity.Name,
                    BusTypeName = t.Bus.BusType.Type,
                    BusNumber = t.Bus.BusNumber,
                    DepartureTime = t.DepartureTime,
                    ArrivalTime = t.ArrivalTime
                })
                .ToListAsync();

            return trips;
        }



    }
}