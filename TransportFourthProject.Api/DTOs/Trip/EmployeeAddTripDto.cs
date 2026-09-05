using Microsoft.Identity.Client;
using System.ComponentModel.DataAnnotations;

namespace TransportFourthProject.Api.DTOs.Trip
{
    public class EmployeeAddTripDto
    {
        [Required(ErrorMessage = "RoutePriceId is required.")]
        public int RoutePriceId { get; set; }

        [Required(ErrorMessage = "DepartureTime is required.")]
        public DateTime DepartureTime { get; set; }

        [Required(ErrorMessage = "BusId is required.")]
        public int BusId { get; set; }

        [Required(ErrorMessage = "EmployeeId is required.")]
        public int EmployeeId { get; set; }
        
        public int? TripDiscountId { get; set; }
    }
}
