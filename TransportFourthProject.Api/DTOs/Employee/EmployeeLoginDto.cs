using System.ComponentModel.DataAnnotations;

namespace TransportFourthProject.Api.DTOs.Employee
{
    public class EmployeeLoginDto
    {
        [Required(ErrorMessage = "Phone is required")]
        [RegularExpression(@"^09\d{8}$", ErrorMessage = "Phone must start with 09 and be 10 digits")]
        public string Phone { get; set; }

        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; }
    }
}
