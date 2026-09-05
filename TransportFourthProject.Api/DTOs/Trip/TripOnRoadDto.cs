namespace TransportFourthProject.Api.DTOs.Trip
{
    public class TripOnRoadDto
    {
        public int TripId { get; set; }
        public int RoutePriceId { get; set; }
        public string StartCityName { get; set; }
        public string EndCityName { get; set; }
        public string BusTypeName { get; set; }
        public string BusNumber { get; set; }
        public string DriverFullName { get; set; }
        public DateTime DepartureTime { get; set; }
        public DateTime ArrivalTime { get; set; }
        public int PassengersCount { get; set; }
        public double TotalDistance { get; set; }
        public double DistanceCovered { get; set; }
        public double RemainingDistance { get; set; }
        public string ETA { get; set; }
        public string Progress { get; set; }
    }
}
