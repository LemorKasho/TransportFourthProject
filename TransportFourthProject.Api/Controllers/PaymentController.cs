using Microsoft.AspNetCore.Mvc;
using TransportFourthProject.Api.DTOs.Payment;
using TransportFourthProject.Api.Enums;
using TransportFourthProject.Api.Services.Payments;

namespace TransportFourthProject.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PaymentController : ControllerBase
    {
        private readonly PaymentService _paymentService;

        public PaymentController(PaymentService paymentService)
        {
            _paymentService = paymentService;
        }

        [HttpPost("payment")]
        public async Task<IActionResult> ProcessPayment([FromBody] PaymentRequestDto request)
        {
            var result = await _paymentService.ProcessPaymentAsync(
                request.BookingId,
                request.PaymentMethod
            );

            if (result.PaymentStatus == "NotFound")
                return NotFound(result);

            if (result.PaymentStatus == "Error")
                return BadRequest(result);

            if (result.PaymentStatus == "Failed")
                return BadRequest(result);

            return Ok(result);
        }
    }
}