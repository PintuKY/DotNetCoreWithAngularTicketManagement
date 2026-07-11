using TicketManagement.Server.Services;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using TicketManagement.Server.Constants;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using TicketManagement.Server.Models.OnlineEducation;

namespace TicketManagement.Server.Services
{
    public class JwtTokenService : IJwtTokenService
    {
        private readonly IConfiguration _configuration;

        public JwtTokenService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string GenerateToken(Users user)
        {
            
            var key = _configuration["JWT_KEY"];
            var issuer = _configuration["JWT_ISSUER"];
            var audience = _configuration["JWT_AUDIENCE"];
            var duration = _configuration.GetValue<int>("JWT_DURATION");

            if (string.IsNullOrWhiteSpace(key))
                throw new Exception("JWT_KEY not found.");

            if (string.IsNullOrWhiteSpace(issuer))
                throw new Exception("JWT_ISSUER not found.");

            if (string.IsNullOrWhiteSpace(audience))
                throw new Exception("JWT_AUDIENCE not found.");


            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Email ?? ""),

                new Claim(JwtClaimNames.Email, user.Email ?? ""),

                new Claim(JwtClaimNames.UserId, user.Id.ToString()),

                new Claim(JwtClaimNames.UserGuid, user.UserGuid.ToString()),

                new Claim(JwtClaimNames.FullName, user.FullName ?? ""),

                new Claim(ClaimTypes.Role, user.Role ?? Roles.Student),

                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var securityKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(key));

            var credentials = new SigningCredentials(
                securityKey,
                SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(duration),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
    
}