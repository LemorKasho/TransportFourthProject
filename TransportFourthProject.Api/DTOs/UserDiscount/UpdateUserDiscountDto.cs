using System.ComponentModel.DataAnnotations;

namespace TransportFourthProject.Api.DTOs.UserDiscount
{
    public class UpdateUserDiscountDto
    {
        [Required(ErrorMessage = "Trip discount ID is required")]
        public int UserDiscountId { get; set; }

        [StringLength(50, ErrorMessage = "Name cannot exceed 50 characters")]
        public string? Name { get; set; }

        [Range(1, 100, ErrorMessage = "Discount percentage must be between 1 and 100")]
        public int? DiscountPercentage { get; set; }
    }
}
