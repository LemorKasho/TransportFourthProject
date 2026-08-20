using System.ComponentModel.DataAnnotations;

namespace TransportFourthProject.Api.Models
{
    public class TripDiscount
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = string.Empty;

        [Required]
        [Range(0, 100)]
        public int Percentage { get; set; }

        #region for relationship with Trip
        public ICollection<Trip> Trips { get; set; } = new List<Trip>();
        #endregion
    }
}
