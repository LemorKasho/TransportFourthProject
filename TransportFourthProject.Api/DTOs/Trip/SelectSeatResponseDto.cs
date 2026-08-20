namespace TransportFourthProject.Api.DTOs.Trip
{
    public class SelectSeatResponseDto
    {
        public int? BookingId { get; set; }
        public int? SeatNumber { get; set; }
        public string? Message { get; set; } = string.Empty;
    }
}