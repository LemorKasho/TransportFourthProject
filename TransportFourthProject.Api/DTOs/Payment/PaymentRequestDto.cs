using TransportFourthProject.Api.Enums;

namespace TransportFourthProject.Api.DTOs.Payment
{
    public class PaymentRequestDto
    {
        public int BookingId { get; set; }
        public PaymentMethod PaymentMethod { get; set; }
    }
}