namespace TransportFourthProject.Api.DTOs.RoutePrice
{
    public class RoutePriceDto
    {
        public int RoutePriceId { get; set; }
        public int BusTypeId { get; set; }
        public string BusTypeName { get; set; }
        public int StartCityId { get; set; }
        public string StartCity { get; set; }
        public int EndCityId { get; set; }
        public string EndCity { get; set; }
        public decimal Price { get; set; }
        public int DurationHours { get; set; }
        public decimal DistanceKm { get; set; }
    }
}
