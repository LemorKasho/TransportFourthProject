namespace TransportFourthProject.Api.DTOs.Employee
{
    public class DriverTripDto
    {
        public int TripId { get; set; }
        public DateTime DepartureTime { get; set; }
        public DateTime ArrivalTime { get; set; }
        public string StartCity { get; set; }
        public string EndCity { get; set; }
        public string BusNumber { get; set; }
    }
}
