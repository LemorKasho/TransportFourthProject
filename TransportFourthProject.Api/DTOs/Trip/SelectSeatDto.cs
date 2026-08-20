using System.ComponentModel.DataAnnotations;

namespace TransportFourthProject.Api.DTOs.Trip
{
    public class SelectSeatDto
    {
        [Required(ErrorMessage = "TripId is required.")]        
        public int TripId { get; set; }
        [Required(ErrorMessage = "SeatNumber is required.")]
        public int SeatNumber { get; set; }
    }
}
