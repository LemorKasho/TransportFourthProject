using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TransportFourthProject.Api.Models
{
    public class Bus
    {
        public int Id { get; set; }
        [Required]
        public int BusTypeId { get; set; }

        #region for relationship with Trip
        public ICollection<Trip> Trips { get; set; } = new List<Trip>();
        #endregion

        #region for relationship with BusType
        [ForeignKey(nameof(BusTypeId))]
        public BusType BusType { get; set; } = null!;
        #endregion
    }
}
