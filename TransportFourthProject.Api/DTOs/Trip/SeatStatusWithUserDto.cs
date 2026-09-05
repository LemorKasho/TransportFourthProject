using TransportFourthProject.Api.Enums;

namespace TransportFourthProject.Api.DTOs.Trip
{
    public class SeatStatusWithUserDto
    {
        public int SeatNumber { get; set; }
        public SeatStatus SeatStatus { get; set; }
        public string FullName { get; set; }
    }
}
