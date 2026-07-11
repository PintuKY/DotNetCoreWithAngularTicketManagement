using System.Security.Claims;
using TicketManagement.Server.Models.OnlineEducation;

namespace TicketManagement.Server.Services.OnlineEducation
{
    public interface IUserService
    {
        Task<Users?> GetCurrentUserAsync(ClaimsPrincipal userClaims);

        int? GetCurrentUserId(ClaimsPrincipal userClaims);

        Guid? GetCurrentUserGuid(ClaimsPrincipal userClaims);

        string? GetCurrentUserEmail(ClaimsPrincipal userClaims);
    }
}
