using System.ComponentModel.DataAnnotations;
using TransportFourthProject.Api.Enums;

namespace TransportFourthProject.Api.DTOs.Employee
{
    public class AddEmployeeDto
    {
        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        [RegularExpression(@"^09\d{8}$", ErrorMessage = "Phone must start with 09 and be 10 digits")]
        public string Phone { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        [RegularExpression(@"^\d{11}$", ErrorMessage = "National number must be 11 digits")]
        public string NationalNumber { get; set; }

        [Required]
        public decimal Salary { get; set; }

        [Required]
        public EmployeeRole Role { get; set; }

        // هذا الحقل اختياري للسائق فقط
        public string? LicenseNumber { get; set; }
    }
}
