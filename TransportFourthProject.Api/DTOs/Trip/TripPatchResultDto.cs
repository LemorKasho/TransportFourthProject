namespace TransportFourthProject.Api.DTOs.Trip
{
    public class TripPatchResultDto
    {
        public bool Success { get; set; }
        public string? ErrorField { get; set; }
        public string? Message { get; set; }
        public string? Suggestion { get; set; }
    }
}
