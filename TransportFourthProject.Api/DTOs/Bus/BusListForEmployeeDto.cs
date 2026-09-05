using TransportFourthProject.Api.Enums;

namespace TransportFourthProject.Api.DTOs.Bus
{
    public class BusListForEmployeeDto
    {
        public int BusId { get; set; }
        public string BusNumber { get; set; }
        public string Type { get; set; }
        public int Capacity { get; set; }
        public BusStatus Status { get; set; }
    }
}
