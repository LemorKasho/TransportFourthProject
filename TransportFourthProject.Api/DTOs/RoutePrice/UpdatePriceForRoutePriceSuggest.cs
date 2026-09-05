namespace TransportFourthProject.Api.DTOs.RoutePrice
{
    public class UpdatePriceForRoutePriceSuggest
    {
        public int RoutePriceId { get; set; }
        public string StartCityName { get; set; }
        public string EndCityName { get; set; }
        public string BusTypeName { get; set; }
        public int UsageCount { get; set; }
        public decimal CurrentPrice { get; set; }
        public decimal SuggestedPrice { get; set; }
        public string Suggestion { get; set; }
        public decimal DistanceKm { get; set; }
    }
}
