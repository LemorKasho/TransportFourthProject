using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TransportFourthProject.Api.Models;
using TransportFourthProject.Api.Settings;
using System.Security.Cryptography;

namespace TransportFourthProject.Api.Services
{
    public class TokenService
    {
        private readonly JwtSettings _jwt;
        public TokenService(JwtSettings jwt)
        {
            _jwt = jwt;
        }
        public string GenerateUserToken(object person)
        {
            var claims = new List<Claim>();

            if (person is User user)
            {
                claims.Add(new Claim("AccountType", "User"));
                claims.Add(new Claim("sub", user.Id.ToString()));
                claims.Add(new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()));
                claims.Add(new Claim(ClaimTypes.Name, $"{user.FirstName} {user.LastName}"));
                claims.Add(new Claim(ClaimTypes.MobilePhone, user.Phone));
            }
            else if (person is Employee emp)
            {
                claims.Add(new Claim("AccountType", "Employee"));
                claims.Add(new Claim("sub", emp.Id.ToString()));
                claims.Add(new Claim(ClaimTypes.NameIdentifier, emp.Id.ToString()));
                claims.Add(new Claim(ClaimTypes.Name, $"{emp.FirstName} {emp.LastName}"));
                claims.Add(new Claim(ClaimTypes.MobilePhone, emp.Phone));

                claims.Add(new Claim(ClaimTypes.Role, emp.Role.ToString()));
            }

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwt.Key));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _jwt.Issuer,
                audience: _jwt.Audience,
                claims: claims,
                expires: DateTime.Now.AddMinutes(_jwt.DurationInMinutes),
                signingCredentials: creds
            );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public string GenerateRefreshToken()
        {
            var randomNumber = RandomNumberGenerator.GetBytes(64);
            return Convert.ToBase64String(randomNumber);
        }
    }
}
