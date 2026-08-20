using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TransportFourthProject.Api.Enums;

namespace TransportFourthProject.Api.Models
{
    public class Payment
    {
        public int Id { get; set; }

        [Required]
        public int BookingId { get; set; }

        [Required]
        public PaymentStatus Status { get; set; }
        [Required]
        public string PaymentMethod { get; set; } = string.Empty;

        [Required]
        [Column(TypeName = "decimal(10,2)")]
        [Range(0, double.MaxValue)]
        public decimal Amount { get; set; }

        [Required]
        public DateTime PaymentDate { get; set; } = DateTime.Now;

        public string? TransactionReference { get; set; }
        [Required]
        public string IdempotencyKey { get; set; } = string.Empty;

        public ICollection<PaymentAttempt> Attempts { get; set; } = new List<PaymentAttempt>();

        #region for relationship with Booking
        [ForeignKey(nameof(BookingId))]
        public Booking Booking { get; set; } = null!;
        #endregion

    }
}