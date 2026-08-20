using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Text.RegularExpressions;
using TransportFourthProject.Api.Models;
using TransportFourthProject.Api.Repositories;
namespace TransportFourthProject.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class UserUpdateController : ControllerBase
    {
        private readonly IRepository<User> _userRepo;

        public UserUpdateController(IRepository<User> userRepo)
        {
            _userRepo = userRepo;
        }

        [HttpPatch("update")]
        [Consumes("application/json-patch+json")]
        public async Task<IActionResult> PatchUpdate([FromBody] JsonPatchDocument<User> patchDoc)
        {
            if (patchDoc == null)
                return BadRequest(new { Message = "Invalid patch document" });

            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == null)
                return Unauthorized(new { Message = "Invalid token" });

            var user = await _userRepo.GetByIdAsync(int.Parse(userId));
            if (user == null)
                return Unauthorized(new { Message = "User not found" });

            bool firstNameUpdated = false;
            bool lastNameUpdated = false;
            bool phoneUpdated = false;

            foreach (var op in patchDoc.Operations)
            {
                if (op.path == null || op.op == null)
                    return BadRequest(new { Message = "Patch operation must include a valid path and op field" });

                var path = op.path.ToLower();

                if (path != "/firstname" &&
                    path != "/lastname" &&
                    path != "/phone")
                {
                    return BadRequest(new { Message = $"Field '{op.path}' is not allowed to be updated" });
                }
                else
                {
                    if (path == "/firstname") firstNameUpdated = true;
                    if (path == "/lastname") lastNameUpdated = true;
                    if (path == "/phone") phoneUpdated = true;

                }
            }
            patchDoc.ApplyTo(user,ModelState);
            if(!ModelState.IsValid)
                { return BadRequest(ModelState); }

            if (firstNameUpdated)
            { 
                if (string.IsNullOrWhiteSpace(user.FirstName))
                    return BadRequest(new { Message = "First name cannot be empty" });
                if (user.FirstName.Length < 2 || user.FirstName.Length > 49)
                    return BadRequest(new { Message = "First name must be between 2 and 49 characters" });
                if (!Regex.IsMatch(user.FirstName, @"^[a-zA-Z]+$"))
                    return BadRequest(new { Message = "First name must contain only letters" });

            }
            if (lastNameUpdated)
            {
                if (string.IsNullOrWhiteSpace(user.LastName))
                    return BadRequest(new { Message = "Last name cannot be empty" });
                if (user.LastName.Length < 2 || user.LastName.Length > 49)
                    return BadRequest(new { Message = "Last name must be between 2 and 49 characters" });
                if (!Regex.IsMatch(user.LastName, @"^[a-zA-Z]+$"))
                    return BadRequest(new { Message = "Last name must contain only letters" });
            }
            if (phoneUpdated)
            {
                if (string.IsNullOrWhiteSpace(user.Phone))
                    return BadRequest(new { Message = "Phone cannot be empty" });
                if (!Regex.IsMatch(user.Phone, @"^09\d{8}$"))
                    return BadRequest(new { Message = "Phone number must start with 09 and be 10 digits" });
            }
            _userRepo.Update(user);
            await _userRepo.SaveChangesAsync();

            return Ok(new
            {
                Message = "User updated successfully"
            });
        }
    }
}