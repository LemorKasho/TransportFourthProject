using System.ComponentModel.DataAnnotations;
using TransportFourthProject.Api.Enums;

namespace TransportFourthProject.Api.Models
{
    public class UserDiscount
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = string.Empty;

        [Required]
        [Range(0, 100)]
        public int Percentage { get; set; }

        public UserDiscountStatus Status { get; set; } = UserDiscountStatus.Active;


        #region for relationship with UserDiscountTicket
        public ICollection<UserDiscountTicket> UserDiscountTickets { get; set; } = new List<UserDiscountTicket>();
        #endregion
    }
}
