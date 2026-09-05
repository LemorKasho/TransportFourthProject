using System.ComponentModel.DataAnnotations;
using TransportFourthProject.Api.Enums;

namespace TransportFourthProject.Api.DTOs.Bus
{
    public class UpdateBusStatusDto
    {
        [Required(ErrorMessage = "Status is required")]
        public BusStatus Status { get; set; }
    }
}
