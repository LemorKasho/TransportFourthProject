using Microsoft.AspNetCore.Mvc;
using TransportFourthProject.Api.Enums;

namespace TransportFourthProject.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PaymentMethodController : ControllerBase
    {
        [HttpGet("options")]
        public IActionResult GetPaymentMethods()
        {
            var options = new[]
            {
                new { Key = PaymentMethod.FakeCard, Label = "Fake Card" },
                new { Key = PaymentMethod.CashAtOffice, Label = "Cash At Office" }
            };

            return Ok(options);
        }
    }
}