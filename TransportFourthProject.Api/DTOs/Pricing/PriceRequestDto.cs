using System.ComponentModel.DataAnnotations;

namespace TransportFourthProject.Api.DTOs.Pricing
{
    public class PriceRequestDto
    {
        [Required]
        public int BookingId { get; set; }
    }
}
