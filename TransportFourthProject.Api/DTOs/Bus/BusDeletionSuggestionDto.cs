using TransportFourthProject.Api.Enums;

namespace TransportFourthProject.Api.DTOs.Bus
{
    public class BusDeletionSuggestionDto
    {
        public int BusId { get; set; }
        public string BusNumber { get; set; }
        public int Capacity { get; set; }
        public int UsageCount { get; set; }
        public BusStatus Status { get; set; }
        public string Suggestion { get; set; }
    }
}
