using System.ComponentModel.DataAnnotations;

namespace TransportFourthProject.Api.DTOs.User.ForgotPassword
{
    public class ResetPasswordDto
    {
        [Required(ErrorMessage = "Phone number is required")]
        [RegularExpression(@"^09\d{8}$", ErrorMessage = "Phone number must start with 09 and be 10 digits.")]
        public string PhoneNumber { get; set;}

        [Required(ErrorMessage = "Password is required")]
        [RegularExpression(
            @"^(?!.*[\u0600-\u06FF])(?=.*[A-Za-z])(?=.*\d)(?=.*[!@#$%^&*()_\-=\[\]{};':""\\|,.<>\/?])[A-Za-z0-9!@#$%^&*()_\-=\[\]{};':""\\|,.<>\/?]{8,}$",
            ErrorMessage = "Password must contain at least one English letter, one number, one symbol, and no Arabic characters.")]
        [MinLength(8,ErrorMessage ="Password must be at least 8 characters.")]
        [MaxLength(49, ErrorMessage = "Password must be less than 50 characters")]
        public string NewPassword { get; set; }

        [Required(ErrorMessage = "Confirm password is required")]
        [Compare("NewPassword", ErrorMessage ="Passwords do not match")]
        public string ConfirmPassword { get; set; }
    }
}
