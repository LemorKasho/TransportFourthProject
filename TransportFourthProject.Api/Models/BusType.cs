using System.ComponentModel.DataAnnotations;

namespace TransportFourthProject.Api.Models
{
    public class BusType
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Type { get; set; } = string.Empty;
        [Required]
        [Range(1, 100)]
        public int Capacity { get; set; }
        public bool IsDeleted { get; set; } = false;

        #region for relationship with RoutePrice
        public ICollection<RoutePrice> RoutePrices { get; set; } = new List<RoutePrice>();
        #endregion

        #region for relationship with Bus
        public ICollection<Bus> Buses { get; set; } = new List<Bus>();
        #endregion
    }
}
