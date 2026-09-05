namespace TransportFourthProject.Api.DTOs.Trip
{
    public class SeatStatusSummaryDto
    {
        public int TotalSeats { get; set; }
        public int AvailableSeats { get; set; }
        public int ReservedSeats { get; set; }
        public int ConfirmedSeats { get; set; }

        public double AvailablePercentage { get; set; }
        public double ReservedPercentage { get; set; }
        public double ConfirmedPercentage { get; set; }
    }
}
