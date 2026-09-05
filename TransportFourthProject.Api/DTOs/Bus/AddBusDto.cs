using System.ComponentModel.DataAnnotations;
using TransportFourthProject.Api.Enums;

namespace TransportFourthProject.Api.DTOs.Bus
{
    public class AddBusDto
    {
        [Required(ErrorMessage ="BusNumber is required")]
        [RegularExpression("^[A-Za-z]{3}[0-9]{3}$",
            ErrorMessage = "BusNumber must be 3 letters followed by 3 digits (abc123)")]
        public string BusNumber { get; set; }
        [Required(ErrorMessage = "BusTypeId is required")]
        [Range(1,int.MaxValue, ErrorMessage = "BusTypeId must be greater than 0")]
        public int BusTypeId { get; set; }
        [Required(ErrorMessage = "Status is required")]
        public BusStatus Status { get; set; }
    }
}
