using System.ComponentModel.DataAnnotations;

namespace TransportFourthProject.Api.DTOs.RoutePrice
{
    public class AddRoutePriceForEmployeeDto
    {
        [Required(ErrorMessage = "BusTypeId is required")]
        [Range(1, int.MaxValue, ErrorMessage = "BusTypeId must be greater than 0")]
        public int BusTypeId { get; set; }

        [Required(ErrorMessage = "StartCityId is required")]
        [Range(1, int.MaxValue, ErrorMessage = "StartCityId must be greater than 0")]
        public int StartCityId { get; set; }

        [Required(ErrorMessage = "EndCityId is required")]
        [Range(1, int.MaxValue, ErrorMessage = "EndCityId must be greater than 0")]
        public int EndCityId { get; set; }

        [Required(ErrorMessage = "Price is required")]
        [Range(1, double.MaxValue, ErrorMessage = "Price must be greater than 0")]
        public decimal Price { get; set; }

        [Required(ErrorMessage = "DurationHours is required")]
        [Range(1, 24, ErrorMessage = "DurationHours must be between 1 and 24")]
        public int DurationHours { get; set; }
        [Required(ErrorMessage = "DistanceKm is required")]

        [Range(1, double.MaxValue, ErrorMessage = "DistanceKm must be greater than 1")]
        public decimal DistanceKm { get; set; } = 0;
    }
}
