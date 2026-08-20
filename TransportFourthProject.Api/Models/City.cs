using System.ComponentModel.DataAnnotations;

namespace TransportFourthProject.Api.Models
{
    public class City
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = string.Empty;

        #region for relationship with RoutePrice (StartCity)
        public ICollection<RoutePrice> StartCityRoutePrices { get; set; } = new List<RoutePrice>();
        #endregion

        #region for relationship with RoutePrice (EndCity)
        public ICollection<RoutePrice> EndCityRoutePrices { get; set; } = new List<RoutePrice>();
        #endregion
    }
}
