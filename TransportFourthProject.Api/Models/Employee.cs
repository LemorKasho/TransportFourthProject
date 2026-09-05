using System.ComponentModel.DataAnnotations;
using TransportFourthProject.Api.Enums;
namespace TransportFourthProject.Api.Models
{
    public class Employee
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string FirstName { get; set; } = string.Empty;

        [Required]
        [MaxLength(100)]
        public string LastName { get; set; } = string.Empty;
        [Required]
        [Range(1, double.MaxValue)]
        public decimal Salary { get; set; }
        [Required]
        [RegularExpression(@"^09\d{8}$")]
        public string Phone { get; set; } = string.Empty;
        [Required]
        [MaxLength(200)]
        public string Password { get; set; } = string.Empty;

        [Required]
        [MaxLength(200)]
        public string NationalNumber { get; set; } = string.Empty;

        [Required]
        public DateTime HireDate { get; set; }
        [Required]
        public EmployeeStatus Status { get; set; }
        [Required]
        public EmployeeRole Role { get; set; }
        [MaxLength(100)]
        public string? LicenseNumber { get; set; }

        #region relationship with Trip
        public ICollection<Trip> Trips { get; set; } = new List<Trip>();
        #endregion
    }
}
