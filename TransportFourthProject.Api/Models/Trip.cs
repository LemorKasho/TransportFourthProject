using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TransportFourthProject.Api.Models
{
    public class Trip
    {
        public int Id { get; set; }
        [Required]
        public int EmployeeId { get; set; }
        [Required]
        public int BusId { get; set; }
        [Required]
        public DateTime DepartureTime { get; set; }
        [Required]
        public DateTime ArrivalTime { get; set; }
        [Required]
        public int RoutePriceId { get; set; }
        public int? TripDiscountId { get; set; }
        public bool IsDeleted { get; set; } = false;
        public bool? IsArrived { get; set; } = false;

        #region for relationship with Booking
        public ICollection<Booking> Bookings { get; set; } = new List<Booking>();
        #endregion

        #region for relationship with TripDiscount
        [ForeignKey(nameof(TripDiscountId))]
        public TripDiscount? TripDiscount { get; set; } = null!;
        #endregion

        #region for relationship with Employee
        [ForeignKey(nameof(EmployeeId))]
        public Employee Employee { get; set; } = null!;
        #endregion

        #region for relationship with Bus
        [ForeignKey(nameof(BusId))]
        public Bus Bus { get; set; } = null!;
        #endregion
         
        #region for relationship with RoutePrice
        [ForeignKey(nameof(RoutePriceId))]
        public RoutePrice RoutePrice { get; set; } = null!;
        #endregion
    }
}