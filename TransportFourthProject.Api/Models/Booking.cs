using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.CompilerServices;
using TransportFourthProject.Api.Enums;

namespace TransportFourthProject.Api.Models
{
    public class Booking
    {
        public int Id { get; set; }
        [Required]
        public int UserId { get; set; }
        [Required]
        public int TripId { get; set; }

        public int? PaymentId { get; set; }
        [Required]
        public int SeatNumber { get; set; }
        [Required]
        public SeatStatus SeatStatus { get; set; }

        [Required]
        public BookingStatus Status { get; set; }

        public int? UserDiscountTicketId { get; set; }
        [Column(TypeName = "decimal(10,2)")]
        public decimal? FinalPrice { get; set; }

        [Required]
        public DateTime BookingTime { get; set; } = DateTime.Now;
        public DateTime? ExpirationTime { get; set; }

        [MaxLength(50)]
        public string BookingReference { get; set; } = string.Empty;

        #region for relationship with User
        [ForeignKey(nameof(UserId))]
        public User User { get; set; } = null!;
        #endregion

        #region for relationship with Trip 
        [ForeignKey(nameof(TripId))]
        public Trip Trip { get; set; } = null!;
        #endregion

        #region for relationship with UserDiscountTicket
        [ForeignKey(nameof(UserDiscountTicketId))]
        public UserDiscountTicket? UserDiscountTicket { get; set; }
        #endregion

        #region for relationship with payment
        [ForeignKey(nameof(PaymentId))]
        public Payment? Payment { get; set; }
        #endregion
    }
}