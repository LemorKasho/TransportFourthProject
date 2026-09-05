namespace TransportFourthProject.Api.DTOs.RoutePrice
{
    public class RoutePriceByBusTypeDto
    {
        public int RoutePriceId { get; set; }
        public string StartCityName { get; set; }
        public string EndCityName { get; set; }
        public int BusTypeId { get; set; }
        public string BusTypeName { get; set; }
        public decimal Price { get; set; }
        public int DurationHours { get; set; }
        public bool IsDeleted { get; set; }
        public decimal DistanceKm { get; set; }
    }
}
