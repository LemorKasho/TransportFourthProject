using Microsoft.AspNetCore.Mvc;

namespace TransportFourthProject.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SortByController : ControllerBase
    {
        [HttpGet("options")]
        public IActionResult GetSortByOptions()
        {
            var options = new[]
            {
                new { Key = "date", Label = "Date" },
                new { Key = "startcity", Label = "Start City" },
                new { Key = "endcity", Label = "End City" },
                new { Key = "bustype", Label = "Bus Type" }
            };

            return Ok(options);
        }
    }
}