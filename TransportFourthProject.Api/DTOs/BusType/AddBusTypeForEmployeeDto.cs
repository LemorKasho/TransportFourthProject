using System.ComponentModel.DataAnnotations;

namespace TransportFourthProject.Api.DTOs.BusType
{
    public class AddBusTypeForEmployeeDto
    {
        [Required(ErrorMessage = "Bus type name is required.")]
        [MaxLength(20, ErrorMessage = "Bus type name cannot exceed 20 characters.")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Bus type capacity is required.")]
        [Range(1,int.MaxValue, ErrorMessage = "Bus type capacity must be a positive number.")]
        public int Capacity { get; set; }
    }
}
