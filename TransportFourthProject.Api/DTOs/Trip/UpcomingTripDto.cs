namespace TransportFourthProject.Api.DTOs.Trip
{
    public class UpcomingTripDto
    {
        public int TripId { get; set; }
        public int RoutePriceId { get; set; }
        public string StartCityName { get; set; }
        public string EndCityName { get; set; }
        public string BusTypeName { get; set; }
        public string BusNumber { get; set; }
        public DateTime DepartureTime { get; set; }
        public DateTime ArrivalTime { get; set; }
    }
}
