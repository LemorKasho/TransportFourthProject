namespace TransportFourthProject.Api.DTOs.RoutePrice
{
    public class RoutePriceStatusesDto
    {
        public int TotalRoutePrices { get; set; }
        public int ActiveRoutePrices { get; set; }
        public int DeletedRoutePrices { get; set; }
        public double ActiveRoutePricesPercentage { get; set; }
        public double DeletedRoutePricesPercentage { get; set; }
    }
}
