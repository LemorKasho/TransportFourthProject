using System.ComponentModel.DataAnnotations;

namespace TransportFourthProject.Api.DTOs.UserDiscountTicket
{
    public class AddUserDiscountTicketDto
    {
        [Required(ErrorMessage = "UserId is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "UserId must be greater than 0.")]
        public int UserId { get; set; }


        [Required(ErrorMessage = "UserDiscountId is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "UserDiscountId must be greater than 0.")]
        public int UserDiscountId { get; set; }

        [Required(ErrorMessage = "StartDate is required.")]
        [DataType(DataType.Date, ErrorMessage = "StartDate must be a valid date.")]
        public DateTime StartDate { get; set; }

        [Required(ErrorMessage = "EndDate is required.")]
        [DataType(DataType.Date, ErrorMessage = "EndDate must be a valid date.")]
        public DateTime EndDate { get; set; }

    }
}
