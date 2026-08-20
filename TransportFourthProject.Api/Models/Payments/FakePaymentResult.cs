namespace TransportFourthProject.Api.Models.Payments
{
    public class FakePaymentResult
    {
        public bool IsSuccess { get; set; }
        public string TransactionReference { get; set; } = string.Empty;
        public string? ErrorMessage { get; set; }
    }
}