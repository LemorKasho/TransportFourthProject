using Microsoft.AspNetCore.Mvc;

namespace TransportFourthProject.Api.DTOs.User
{
    public class UserUpdateProfileDto
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Phone { get; set; }
    }
}
