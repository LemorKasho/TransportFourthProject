namespace TransportFourthProject.Api.DTOs.Trip
{
    public class RoutePriceForEmployeeDto
    {
        public int RoutePriceId { get; set; }
        public string StartCity { get; set; }
        public string EndCity { get; set; }
        public decimal Price { get; set; }
        public string BusType { get; set; }
        public int DurationHours { get; set; }
    }
}
