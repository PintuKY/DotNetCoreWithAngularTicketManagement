using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TicketManagement.Server.Models.OnlineEducation;
using TicketManagement.Server.Services;

namespace TicketManagement.Server.Controllers.OnlineEducation
{
    public class AuthController
    {
        private readonly IConfiguration _configuration;
        private readonly IJwtTokenService _jwtTokenService;

        public AuthController(IConfiguration configuration, IJwtTokenService jwtTokenService)
        {
            _configuration = configuration;
            _jwtTokenService = jwtTokenService;
        }

        public string GenerateJwtTokens(Users user)
        {
            //var jwtSettings = _configuration.GetSection("Jwt");
            var jwtSettings = _configuration.GetValue<string>("JWT_KEY")!;

            var claims = new[]
     {
        new Claim(JwtRegisteredClaimNames.Sub,
            user.Email),

        new Claim("UserId",
            user.Id.ToString()),

        new Claim("FullName",
            user.FullName ?? ""),

        new Claim(ClaimTypes.Role,
            user.Role ?? "Student"),

        new Claim(JwtRegisteredClaimNames.Jti,
            Guid.NewGuid().ToString())
    };

            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(jwtSettings));

            var credentials =
                new SigningCredentials(
                    key,
                    SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration["Issuer"],
                audience: _configuration["Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(
                    Convert.ToDouble(_configuration["DurationInMinutes"])),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler()
                .WriteToken(token);
        }
    }
}
