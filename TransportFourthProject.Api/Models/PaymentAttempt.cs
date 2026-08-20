using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TransportFourthProject.Api.Enums;

namespace TransportFourthProject.Api.Models
{
    public class PaymentAttempt
    {
        public int Id { get; set; }
        [Required]
        public int PaymentId { get; set; }
        [Required]
        public string IdempotencyKey { get; set; } = string.Empty;
        [Required]
        public PaymentAttemptStatus Status { get; set; }

        [Required]
        [Column(TypeName = "decimal(10,2)")]
        [Range(0, double.MaxValue)]
        public decimal Amount { get; set; }
        public string? TransactionReference { get; set; }
        public string? ErrorCode { get; set; }
        public string? ErrorMessage { get; set; }

        [Required]
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime? CompletedAt { get; set; }
        #region relation for payment
        [ForeignKey(nameof(PaymentId))]
        public Payment Payment { get; set; } = null!;
        #endregion
    }
}
