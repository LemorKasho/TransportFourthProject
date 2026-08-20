using System.ComponentModel.DataAnnotations;

namespace TransportFourthProject.Api.Models
{
    public class User
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string FirstName { get; set; }  = string.Empty;

        [Required]
        [MaxLength(100)]
        public string LastName { get; set; } = string.Empty;

        [Required]
        [MaxLength(10)]
        [RegularExpression(@"^09\d{8}$")]
        public string Phone { get; set; } = string.Empty;

        [Required]
        [MaxLength(200)]
        public string Password { get; set; } = string.Empty;

        [Required]
        [StringLength(11,MinimumLength = 11)]
        public string NationalNumber { get; set; } = string.Empty;

        #region for relationship with Booking
        public ICollection<Booking> Bookings { get; set; } = new List<Booking>();
        #endregion

        #region for relationship with UserDiscountTicket
        public ICollection<UserDiscountTicket> UserDiscountTickets { get; set; } = new List<UserDiscountTicket>();
        #endregion

        #region for relationship with RefreshToken
        public ICollection<RefreshToken> RefreshTokens { get; set; } = new List<RefreshToken>();
        #endregion
    }
}