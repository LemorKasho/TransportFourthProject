namespace TransportFourthProject.Api.DTOs.Trip
{
    public class TripEditDataDto
    {
        public TripDetailsForEditTripDto Trip { get; set; }

        public List<DriverDto> AvailableDrivers { get; set; }

        public List<BusDto> AvailableBuses { get; set; }

        public List<TripDiscountDto> AvailableDiscounts { get; set; }
    }
    public class TripDetailsForEditTripDto
    {
        public int TripId { get; set; }
        public int EmployeeId { get; set; }
        public int BusId { get; set; }
        public DateTime DepartureTime { get; set; }
        public DateTime ArrivalTime { get; set; }
        public int? TripDiscountId { get; set; }
        public string StartCityName { get; set; }
        public string EndCityName { get; set; }
        public decimal Price { get; set; }
    }

    public class DriverDto
    {
        public int EmployeeId { get; set; }
        public string FullName { get; set; }
    }

    public class BusDto
    {
        public int BusId { get; set; }
        public string BusNumber { get; set; }
        public string BusTypeName { get; set; }
    }

    public class TripDiscountDto
    {
        public int TripDiscountId { get; set; }
        public string Name { get; set; }
        public decimal Percentage { get; set; }
    }
}