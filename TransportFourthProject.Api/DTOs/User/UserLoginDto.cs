using System.ComponentModel.DataAnnotations;

namespace TransportFourthProject.Api.DTOs.User
{
    public class UserLoginDto
    {
        [Required(ErrorMessage = "Phone number is required")]
        [RegularExpression(@"^09\d{8}$", ErrorMessage = "Phone number must start with 09 and be 10 digits")]
        public string Phone { get; set; } = string.Empty;

        [Required(ErrorMessage = "Password is required")]
        [RegularExpression(@"^(?!.*[\u0600-\u06FF])[A-Za-z0-9!@#$%^&*()_+\-=\[\]{};':""\\|,.<>\/?]{8,}$",
                  ErrorMessage = "Password must contain English letters, numbers, symbols, and no Arabic characters.")]
        [MinLength(8, ErrorMessage = "Password must be at least 8 characters")]
        [MaxLength(49, ErrorMessage = "Password must be less than 50 characters")]
        public string Password {  get; set; } = string.Empty;
    }
}
