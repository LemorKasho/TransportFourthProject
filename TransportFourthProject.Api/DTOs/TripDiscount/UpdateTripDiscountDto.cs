using System.ComponentModel.DataAnnotations;
using TransportFourthProject.Api.Enums;

namespace TransportFourthProject.Api.DTOs.TripDiscount
{
    public class UpdateTripDiscountDto
    {
        [Required(ErrorMessage = "Trip discount ID is required")]
        public int TripDiscountId { get; set; }


        [StringLength(50, ErrorMessage = "Name cannot exceed 50 characters")]
        public string? Name { get; set; }


        [Range(1, 100, ErrorMessage = "Discount percentage must be between 1 and 100")]
        public int? Percentage { get; set; }
    }
}
