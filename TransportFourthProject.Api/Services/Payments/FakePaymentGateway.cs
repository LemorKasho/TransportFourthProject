using TransportFourthProject.Api.Models;
using TransportFourthProject.Api.Models.Payments;

namespace TransportFourthProject.Api.Services.Payments
{
    public class FakePaymentGateway
    {
        public async Task<FakePaymentResult> ProcessAsync(Payment payment, decimal amount,
                                                    string idempotencyKey)
        {
            await Task.Delay(500);
            var random = new Random();
            bool isSuccess = random.Next(1, 100) <= 95;
            return new FakePaymentResult
            {
                IsSuccess = isSuccess,
                TransactionReference = Guid.NewGuid().ToString(),
                ErrorMessage = isSuccess ? null : "Payment failed"
            };
        }
    }
}
