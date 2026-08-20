using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TransportFourthProject.Api.Models
{
    public class RoutePrice
    {
        public int Id { get; set; }
        [Required]
        public int BusTypeId { get; set; }
        [Required]
        public int StartCityId { get; set; }
        [Required]
        public int EndCityId { get; set; }
        [Required]
        [Column(TypeName = "decimal(10,2)")]
        [Range(0, double.MaxValue)]
        public decimal Price { get; set; }
        public int DurationHours { get; set; }

        #region for relationship with Trip
        public ICollection<Trip> Trips { get; set; } = new List<Trip>();
        #endregion

        #region for relationship with City (StartCity)
        [ForeignKey(nameof(StartCityId))]
        public City StartCity { get; set; } = null!;
        #endregion

        #region for relationship with City (EndCity)
        [ForeignKey(nameof(EndCityId))]
        public City EndCity { get; set; } = null!;
        #endregion

        #region for relationship with BusType
        [ForeignKey(nameof(BusTypeId))]
        public BusType BusType { get; set; } = null!;
        #endregion
    }
}