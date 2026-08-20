using System.ComponentModel.DataAnnotations;

namespace TransportFourthProject.Api.DTOs.User
{
    public class UserRegisterDto
    {
        [Required(ErrorMessage = "First name is required")]
        [MinLength(2, ErrorMessage = "First name must be at least 2 characters")]
        [MaxLength(49, ErrorMessage = "First name must be less than 50 characters")]
        public string FirstName { get; set; } = string.Empty;


        [Required(ErrorMessage = "Last name is required")]
        [MinLength(2, ErrorMessage = "Last name must be at least 2 characters")]
        [MaxLength(49, ErrorMessage = "Last name must be less than 50 characters")]
        public string LastName { get; set; } = string.Empty;


        [Required(ErrorMessage = "Phone number is required")]
        [RegularExpression(@"^09\d{8}$", ErrorMessage = "Phone number must start with 09 and be 10 digits")]
        public string Phone { get; set; } = string.Empty;


        [Required(ErrorMessage = "National number is required")]
        [StringLength(11, MinimumLength = 11, ErrorMessage = "National number must be exactly 11 digits")]
        public string NationalNumber { get; set; } = string.Empty;

        [Required(ErrorMessage = "Password is required")]
        [MinLength(8, ErrorMessage = "Password must be at least 8 characters")]
        [MaxLength(49, ErrorMessage = "Password must be less than 50 characters")]
        public string Password { get; set; } = string.Empty;

        [Required(ErrorMessage = "Confirm password is required")]
        [MinLength(8, ErrorMessage = "Confirm password must be at least 8 characters")]
        [MaxLength(49, ErrorMessage = "Confirm password must be less than 50 characters")]
        public string ConfirmPassword { get; set; } = string.Empty;
    }
}
