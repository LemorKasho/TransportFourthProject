using System.ComponentModel.DataAnnotations;

namespace TransportFourthProject.Api.DTOs
{
    public class RefreshTokenDto
    {
        [Required]
        public string RefreshToken { get; set; } = string.Empty;
    }
}
