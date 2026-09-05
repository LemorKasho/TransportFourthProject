using System.ComponentModel.DataAnnotations;

namespace TransportFourthProject.Api.DTOs.City
{
    public class AddCityForEmployeeDto
    {
        [Required(ErrorMessage = "City name is required.")]
        [MaxLength(50,ErrorMessage = "City name cannot exceed 50 characters.")]
        public string CityName { get; set; }
    }
}
