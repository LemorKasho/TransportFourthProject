using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace TransportFourthProject.Api.DTOs.User
{
    public class UserUpdateProfileDto
    {
        [RegularExpression(@"^(?!.*[\u0600-\u06FF])[A-Za-z]{2,49}$",
            ErrorMessage = "First name must contain English letters only (2–49 characters).")]
        public string? FirstName { get; set; }

        [RegularExpression(@"^(?!.*[\u0600-\u06FF])[A-Za-z]{2,49}$",
            ErrorMessage = "Last name must contain English letters only (2–49 characters).")]
        public string? LastName { get; set; }

        [RegularExpression(@"^09\d{8}$",
            ErrorMessage = "Phone number must start with 09 and be 10 digits.")]
        public string? Phone { get; set; }
    }
}
