namespace TransportFourthProject.Api.DTOs.Trip
{
    public class BusTypeTripsCountDto
    {
        public int BusTypeId { get; set; }
        public string BusTypeName { get; set; } = null!;
        public int Capacity { get; set; }
        public int TripsCount { get; set; }
    }
}
