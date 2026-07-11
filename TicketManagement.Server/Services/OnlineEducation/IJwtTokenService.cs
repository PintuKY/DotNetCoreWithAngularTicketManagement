using TicketManagement.Server.Models.OnlineEducation;
using TicketManagement.Server.Services;

namespace TicketManagement.Server.Services
{
    public interface IJwtTokenService
    {
        string GenerateToken(Users user);
    }
}