using System.ComponentModel.DataAnnotations;

namespace TransportFourthProject.Api.DTOs.Trip
{
    public class SelectSeatDto
    {
        [Required(ErrorMessage = "TripId is required.")]
        [Range(1,int.MaxValue, ErrorMessage = "Trip Id must be a positive number.")]
        public int TripId { get; set; }

        [Required(ErrorMessage = "SeatNumber is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "Seat bumber must be a positive number.")]

        public int SeatNumber { get; set; }
    }
}
