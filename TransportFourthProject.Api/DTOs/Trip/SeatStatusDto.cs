using System.Text.Json.Serialization;
using TransportFourthProject.Api.Enums;

namespace TransportFourthProject.Api.DTOs.Trip
{
    public class SeatStatusDto
    {
        public int SeatNumber { get; set; }
        public SeatStatus Status { get; set; }
    }
}
