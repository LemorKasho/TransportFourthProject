using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using TransportFourthProject.Api.Enums;

namespace TransportFourthProject.Api.DTOs.Payment
{
    public class PaymentRequestDto
    {
        [Required(ErrorMessage ="BookingId is required.")]
        [Range(1,int.MaxValue, ErrorMessage ="Booking id must be a positive number.")]
        public int BookingId { get; set; }

        [Required(ErrorMessage = "PaymentMethod is required.")]
        public PaymentMethod PaymentMethod { get; set; }
    }
}