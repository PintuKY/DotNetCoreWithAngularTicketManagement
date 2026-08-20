using System.Security.Claims;
using System.Collections.Generic;
using System.Threading.Tasks;
using TicketManagement.Server.Models.DTOs;

namespace TicketManagement.Server.Services.OnlineEducation
{
    public interface IUserTestAttempted
    {
        Task<List<UserTestAttemptedDTO>> GetDataUserTestAttempted(ClaimsPrincipal userClaims);
    }
}
