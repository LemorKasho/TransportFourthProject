namespace TransportFourthProject.Api.DTOs.Booking
{
    public class BookingResponseDto
    {
        public int? BookingId { get; set; }
        public int? SeatNumber { get; set; }
        public DateTime? ExpirationTime { get; set; }
        public string BookingStatus { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
    }
}
