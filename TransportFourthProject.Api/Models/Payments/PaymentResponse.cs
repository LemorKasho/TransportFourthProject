namespace TransportFourthProject.Api.Models.Payments
{
    public class PaymentResponse
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; }

        public static PaymentResponse Success(string msg)
            => new PaymentResponse { IsSuccess = true, Message = msg };

        public static PaymentResponse Failed(string msg)
            => new PaymentResponse { IsSuccess = false, Message = msg };
    }
}