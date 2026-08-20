namespace TransportFourthProject.Api.DTOs.Pricing
{
    public class PriceResponseDto
    {
        public decimal BasePrice { get; set; }
        public decimal TripDiscountPercentage { get; set; }
        public decimal UserDiscountPercentage { get; set; }
        public decimal FinalPrice { get; set; }
    }
}
