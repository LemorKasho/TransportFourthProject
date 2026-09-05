using System.ComponentModel.DataAnnotations;

namespace TransportFourthProject.Api.DTOs.User
{
    public class UserChangePasswordDto
    {
        [Required(ErrorMessage = "Old password is required")]
        [MinLength(8, ErrorMessage = "Old password must be at least 8 characters")]
        [MaxLength(49, ErrorMessage = "Old password must be less than 50 characters")]
        [RegularExpression(@"^(?!.*[\u0600-\u06FF])[A-Za-z0-9!@#$%^&*()_+\-=\[\]{};':""\\|,.<>\/?]{8,}$",
            ErrorMessage = "Password must contain English letters, numbers, symbols, and no Arabic characters.")]
        public string OldPassword { get; set; } = string.Empty;

        [Required(ErrorMessage = "Password is required")]
        [RegularExpression(
            @"^(?!.*[\u0600-\u06FF])(?=.*[A-Za-z])(?=.*\d)(?=.*[!@#$%^&*()_\-=\[\]{};':""\\|,.<>\/?])[A-Za-z0-9!@#$%^&*()_\-=\[\]{};':""\\|,.<>\/?]{8,}$",
            ErrorMessage = "Password must contain at least one English letter, one number, one symbol, and no Arabic characters.")]
        [MinLength(8, ErrorMessage = "New password must be at least 8 characters")]
        [MaxLength(49, ErrorMessage = "New password must be less than 50 characters")]
        public string NewPassword { get; set; } = string.Empty;
    }
}