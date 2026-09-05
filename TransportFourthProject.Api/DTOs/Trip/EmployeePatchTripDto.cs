using System.ComponentModel.DataAnnotations;

namespace TransportFourthProject.Api.DTOs.Trip
{
    public class EmployeePatchTripDto
    {
        [Required(ErrorMessage = "TripId is required")]
        public int TripId { get; set; }
        public DateTime? DepartureTime { get; set; }
        public int? BusId { get; set; }
        public int? EmployeeId { get; set; }
        public int? TripDiscountId { get; set; }
    }
}