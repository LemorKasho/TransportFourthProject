using TransportFourthProject.Api.Enums;

namespace TransportFourthProject.Api.DTOs.Bus
{
    public class BusOnRoadsDto
    {
        public int BusId { get; set; }
        public string BusNumber { get; set; }
        public int Capacity { get; set; }
        public DateTime DepartureTime { get; set; }
        public DateTime ArrivalTime { get; set; }
    }
}
