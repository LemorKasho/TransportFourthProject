using Microsoft.AspNetCore.Mvc;

namespace TransportFourthProject.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SortOptionsController : ControllerBase
    {
        [HttpGet("order")]
        public IActionResult GetOrderOptions()
        {
            var options = new[]
            {
                new { Key = "asc", Label = "Ascending" },
                new { Key = "desc", Label = "Descending" }
            };

            return Ok(options);
        }
    }
}