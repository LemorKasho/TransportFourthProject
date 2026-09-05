using System.ComponentModel.DataAnnotations;

namespace TransportFourthProject.Api.DTOs.User
{
    public class UserRegisterDto
    {
        [Required(ErrorMessage = "First name is required")]
        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "First name must contain english letters only.")]
        [MinLength(2, ErrorMessage = "First name must be at least 2 characters")]
        [MaxLength(49, ErrorMessage = "First name must be less than 50 characters")]
        public string FirstName { get; set; } = string.Empty;


        [Required(ErrorMessage = "Last name is required")]
        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Last name must contain english letters only.")]
        [MinLength(2, ErrorMessage = "Last name must be at least 2 characters")]
        [MaxLength(49, ErrorMessage = "Last name must be less than 50 characters")]
        public string LastName { get; set; } = string.Empty;


        [Required(ErrorMessage = "Phone number is required")]
        [RegularExpression(@"^09\d{8}$", ErrorMessage = "Phone number must start with 09 and be 10 digits")]
        public string Phone { get; set; } = string.Empty;


        [Required(ErrorMessage = "National number is required")]
        [RegularExpression(@"^\d{11}$", ErrorMessage = "National number must be exactly 11 digits")]
        public string NationalNumber { get; set; } = string.Empty;

        [Required(ErrorMessage = "Password is required")]
        [RegularExpression(
             @"^(?!.*[\u0600-\u06FF])(?=.*[A-Za-z])(?=.*\d)(?=.*[!@#$%^&*()_\-=\[\]{};':""\\|,.<>\/?])[A-Za-z0-9!@#$%^&*()_\-=\[\]{};':""\\|,.<>\/?]{8,}$",
             ErrorMessage = "Password must contain at least one English letter, one number, one symbol, and no Arabic characters.")]
        [MinLength(8, ErrorMessage = "Password must be at least 8 characters")]
        [MaxLength(49, ErrorMessage = "Password must be less than 50 characters")]
        public string Password { get; set; } = string.Empty;

        [Required(ErrorMessage = "Confirm password is required")]
        [Compare("Password", ErrorMessage = "Passwords do not match")]
        public string ConfirmPassword { get; set; } = string.Empty;
    }
}
