using System.ComponentModel.DataAnnotations;
using TransportFourthProject.Api.Enums;

namespace TransportFourthProject.Api.DTOs.TripDiscount
{
    public class AddTripDiscountDto
    {
        [Required(ErrorMessage = "Name is required")]
        [StringLength(50, ErrorMessage = "Name cannot exceed 50 characters")]
        public string Name { get; set; }


        [Required(ErrorMessage = "Discount percentage is required")]
        [Range(1, 100, ErrorMessage = "Discount percentage must be between 1 and 100")]
        public int DiscountPercentage { get; set; }
    }
}
