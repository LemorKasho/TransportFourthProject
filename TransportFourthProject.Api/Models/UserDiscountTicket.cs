using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TransportFourthProject.Api.Models
{
    public class UserDiscountTicket
    {
        public int Id { get; set; }
        [Required]
        public int UserId { get; set; }
        [Required]
        public int DiscountId { get; set; }
        [Required]
        public DateTime StartDate { get; set; }
        [Required]
        public DateTime EndDate { get; set; }

        #region for relationship with Booking
        public ICollection<Booking> Bookings { get; set; } = new List<Booking>();
        #endregion

        #region for relationship with User
        [ForeignKey(nameof(UserId))]
        public User User { get; set; } = null!;
        #endregion

        #region for relationship with UserDiscount
        [ForeignKey(nameof(DiscountId))]
        public UserDiscount UserDiscount { get; set; } = null!;
        #endregion
    }
}