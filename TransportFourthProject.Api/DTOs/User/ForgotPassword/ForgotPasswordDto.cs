using System.ComponentModel.DataAnnotations;

namespace TransportFourthProject.Api.DTOs.User.ForgotPassword
{
    public class ForgotPasswordDto
    {
        [Required(ErrorMessage = "Phone number is required")]
        [RegularExpression(@"^09\d{8}$", ErrorMessage ="Phone number must start with 09 and be 10 digits.")]
        public string PhoneNumber { get; set; }
    }
}
