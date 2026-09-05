using System.ComponentModel.DataAnnotations;

namespace TransportFourthProject.Api.DTOs.Trip
{
    public class TripListDto
    {
        public int TripId { get; set; }
        public string DepartureTime { get; set; } = string.Empty;
        public string StartCity { get; set; } = string.Empty;
        public string EndCity { get; set; } = string.Empty;
        public string BusType { get; set; } = string.Empty;
        public string BasePrice { get; set; } = string.Empty;
        public string DiscountName { get; set; } = string.Empty;
        public string DiscountPercentage { get; set; } = string.Empty;
        public bool IsDeleted { get; set; } = false;
    }
}