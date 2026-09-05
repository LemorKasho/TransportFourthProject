using System.ComponentModel.DataAnnotations;

namespace TransportFourthProject.Api.DTOs.User.ForgotPassword
{
    public class VerifyResetCodeDto
    {
        [Required(ErrorMessage = "Phone number is required")]
        [RegularExpression(@"^09\d{8}$", ErrorMessage = "Phone number must start with 09 and be 10 digits.")]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "Code is required")]
        [RegularExpression(@"^\d{4}$", ErrorMessage ="Code must be exactly 4 digits" )]
        public string Code { get; set; }
    }
}
