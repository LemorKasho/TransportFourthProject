using System.ComponentModel.DataAnnotations;

namespace TransportFourthProject.Api.DTOs.User
{
    public class UserChangePasswordDto
    {
        [Required(ErrorMessage = "Old password is required")]
        [MinLength(8, ErrorMessage = "Old password must be at least 8 characters")]
        [MaxLength(49, ErrorMessage = "Old password must be less than 50 characters")]
        public string OldPassword { get; set; } = string.Empty;

        [Required(ErrorMessage = "New password is required")]
        [MinLength(8, ErrorMessage = "New password must be at least 8 characters")]
        [MaxLength(49, ErrorMessage = "New password must be less than 50 characters")]
        public string NewPassword { get; set; } = string.Empty;
    }
}