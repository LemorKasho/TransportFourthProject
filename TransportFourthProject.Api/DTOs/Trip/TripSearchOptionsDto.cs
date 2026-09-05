using TransportFourthProject.Api.DTOs.Bus;
using TransportFourthProject.Api.DTOs.BusType;
using TransportFourthProject.Api.DTOs.City;
using TransportFourthProject.Api.DTOs.Employee;

namespace TransportFourthProject.Api.DTOs.Trip
{
    public class TripSearchOptionsDto
    {
        public List<CityDto> Cities { get; set; }
        public List<BusTypeDto> BusTypes { get; set; }
        public List<DriverForSearchTripDto> Drivers { get; set; }
        public List<BusForSearchTripDto> Buses { get; set; }
        public List<string> Statuses { get; set; }
        public List<string> CapacityStatuses { get; set; }
        public List<string> DiscountOptions { get; set; }
        public List<string> SortFields { get; set; }
        public List<string> OrderOptions { get; set; }
    }
}
