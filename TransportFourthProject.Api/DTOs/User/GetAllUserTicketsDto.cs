namespace TransportFourthProject.Api.DTOs.User
{
    public class GetAllUserTicketsDto
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public int BookingId { get; set; }
        public int TripId { get; set; }
        public string TripDate { get; set; } = string.Empty;
        public string StartCity { get; set; } = string.Empty;
        public string EndCity { get; set; } = string.Empty;
        public string BusType { get; set; } = string.Empty;
        public string BusNumber { get; set; } = string.Empty;
        public int SeatNumber { get; set; }
        public string Status { get; set; } = string.Empty;
        public DateTime? ExpirationTime { get; set; }
        public string FinalPrice { get; set; } = string.Empty;
    }
}









