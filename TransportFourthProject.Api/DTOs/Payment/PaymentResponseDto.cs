namespace TransportFourthProject.Api.DTOs.Payment
{
    public class PaymentResponseDto
    {
        public int? PaymentId { get; set; }
        public int? BookingId { get; set; }  
        public decimal? Amount { get; set; } 
        public string PaymentStatus { get; set; } = ""; 
        public string Message { get; set; } = "";
    }
}