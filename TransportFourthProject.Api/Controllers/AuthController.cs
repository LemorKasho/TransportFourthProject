using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Text.RegularExpressions;
using TransportFourthProject.Api.DTOs;
using TransportFourthProject.Api.DTOs.User;
using TransportFourthProject.Api.Models;
using TransportFourthProject.Api.Repositories;
using TransportFourthProject.Api.Services;
using TransportFourthProject.Api.Settings;

namespace TransportFourthProject.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IRepository<User> _userRepo;
        private readonly IRepository<RefreshToken> _refreshTokenRepo;
        private readonly TokenService _tokenService;
        private readonly JwtSettings _jwt;
        private readonly PasswordHasher _passwordHasher;
        private readonly AesEncryptionService _aesEncryptionService;
        public AuthController(IRepository<User> userRepo, IRepository<RefreshToken> refreshTokenRepo,
            TokenService tokenService, AesEncryptionService aesEncryptionService,
            PasswordHasher passwordHasher, JwtSettings jwt)
        {
            _userRepo = userRepo;
            _refreshTokenRepo = refreshTokenRepo;
            _tokenService = tokenService;
            _passwordHasher = passwordHasher;
            _aesEncryptionService = aesEncryptionService;
            _jwt = jwt;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UserLoginDto dto)
        {

            if(!Regex.IsMatch(dto.Phone, @"^09\d{8}$"))
                return BadRequest(new { Message = "Phone number must start with 09 and be 10 digits" });

            var user = (await _userRepo.GetAsync(u => u.Phone == dto.Phone));
            if (user == null)
                return Unauthorized(new { Message = "Login failed" });

            var passwordCheck = _passwordHasher.VerifyPassword(dto.Password, user.Password);
            if (!passwordCheck)
                return Unauthorized(new { Message = "Login failed" });

            var existingRefresh = await _refreshTokenRepo.GetAsync(r => r.UserId == user.Id && r.ExpiresAt > DateTime.Now);
            if(existingRefresh != null)
            {
                return Ok(new
                {
                    Message = "You are already logged in",
                    AccessToken = _tokenService.GenerateUserToken(user),
                    RefreshToken = existingRefresh.Token,
                    User = new
                    {
                        user.Id,
                        user.FirstName,
                        user.LastName,
                        user.Phone,
                    }
                });
            }

            var accessToken = _tokenService.GenerateUserToken(user);
            var refreshToken = _tokenService.GenerateRefreshToken();

            var refresh = new RefreshToken
            {
                Token = refreshToken,
                UserId = user.Id,
                ExpiresAt = DateTime.Now.AddDays(7),
            };
            
            await _refreshTokenRepo.AddAsync(refresh);
            await _refreshTokenRepo.SaveChangesAsync();

            return Ok(new
            {
                Message = "Login successful",
                AccessToken = accessToken,
                RefreshToken = refreshToken,
                User = new
                {
                    user.Id,
                    user.FirstName,
                    user.LastName,
                    user.Phone,
                }
            });
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] UserRegisterDto dto)
        {
            if(!Regex.IsMatch(dto.FirstName, @"^[a-zA-Z]+$"))
                return BadRequest(new { Message = "First name must contain only letters" });

            if (!Regex.IsMatch(dto.LastName, @"^[a-zA-Z]+$"))
                return BadRequest(new { Message = "Last name must contain only letters" });

            if (!Regex.IsMatch(dto.Phone, @"^09\d{8}$"))
                return BadRequest(new { Message = "Phone number must start with 09 and be 10 digits" });

            if (!Regex.IsMatch(dto.NationalNumber, @"^\d{11}$"))
                return BadRequest(new { Message = "National number must be 11 digits" });

            if (dto.Password != dto.ConfirmPassword)
                return BadRequest(new { Message = "Registration failed" });

            var phoneExists = await _userRepo.ExistsAsync(u => u.Phone == dto.Phone);
            if (phoneExists)
                return BadRequest(new { Message = "Registration failed" });

            var allUser = await _userRepo.GetAllAsync();
            var nationalExists = allUser.Any(u => _passwordHasher.VerifyPassword(dto.NationalNumber, u.NationalNumber));
            if (nationalExists)
                return BadRequest(new { Message = "Registration failed" });

            var hashedPassword = _passwordHasher.HashPassword(dto.Password);
            var encryptedNationalNumber = _aesEncryptionService.Encrypt(dto.NationalNumber);
            var user = new User
            {
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                Phone = dto.Phone,
                NationalNumber = encryptedNationalNumber,
                Password = hashedPassword
            };

            await _userRepo.AddAsync(user);
            await _userRepo.SaveChangesAsync();

            var accessToken = _tokenService.GenerateUserToken(user);
            var refreshToken = _tokenService.GenerateRefreshToken();

            var refresh = new RefreshToken
            {
                Token = refreshToken,
                UserId = user.Id,
                ExpiresAt = DateTime.Now.AddDays(7),
                IsRevoked = false
            };

            await _refreshTokenRepo.AddAsync(refresh);
            await _refreshTokenRepo.SaveChangesAsync();

            return Ok(new
            {
                Message = "Registered successfully",
                AccessToken = accessToken,
                RefreshToken = refreshToken,
                User = new
                {
                    user.Id,
                    user.FirstName,
                    user.LastName,
                    user.Phone,
                }
            });
        }

        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenDto dto)
        {
            var storedToken = await _refreshTokenRepo.GetAsync(t => t.Token == dto.RefreshToken);

            if (storedToken == null ||
                storedToken.ExpiresAt < DateTime.Now ||
                storedToken.IsRevoked)
            {
                return BadRequest(new { Message = "Refresh failed" });
            }

            var user = await _userRepo.GetAsync(u => u.Id == storedToken.UserId);

            if (user == null)
                return BadRequest(new { Message = "Refresh failed" });

            var newAccessToken = _tokenService.GenerateUserToken(user);

            var newRefreshToken = _tokenService.GenerateRefreshToken();

            storedToken.IsRevoked = true;
            _refreshTokenRepo.Update(storedToken);

            var newToken = new RefreshToken
            {
                Token = newRefreshToken,
                UserId = user.Id,
                ExpiresAt = DateTime.Now.AddDays(7)
            };

            await _refreshTokenRepo.AddAsync(newToken);
            await _refreshTokenRepo.SaveChangesAsync();

            return Ok(new
            {
                Message = "Token refreshed",
                AccessToken = newAccessToken,
                RefreshToken = newRefreshToken,
                ExpiresIn = _jwt.DurationInMinutes * 60
            });
        }

        [Authorize]
        [HttpGet("me")]
        public async Task<IActionResult> Me()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == null)
                return Unauthorized(new { Message = "Invalid token" });

            var user = await _userRepo.GetByIdAsync(int.Parse(userId));
            if (user == null)
                return Unauthorized(new { Message = "User not found" });

            return Ok(new
            {
                user.Id,
                user.FirstName,
                user.LastName,
                user.Phone,
            });
        }

        [Authorize]
        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == null)
                return Unauthorized(new { Message = "Invalid token" });

            var tokens = await _refreshTokenRepo.FindAsync(r => r.UserId == int.Parse(userId)
                                                           && !r.IsRevoked);

            if(tokens == null || !tokens.Any())
                return Ok(new { Message = "You are already logged out" });

            foreach (var token in tokens)
            {
                token.IsRevoked = true;
                _refreshTokenRepo.Update(token);
            }

            await _refreshTokenRepo.SaveChangesAsync();

            return Ok(new { Message = "Logged out successfully" });
        }
    }
}