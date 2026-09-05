namespace TransportFourthProject.Api.DTOs.Trip
{
    public class RoutePriceTripsCountDto
    {
        public int RoutePriceId { get; set; }
        public string StartCityName { get; set; }
        public string EndCityName { get; set; }
        public string BusTypeName { get; set; }
        public int TripsCount { get; set; }
    }
}
