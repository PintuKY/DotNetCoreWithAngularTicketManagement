using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using TicketManagement.Server.Models;
using TicketManagement.Server.Models.DTOs;
using TicketManagement.Server.Models.OnlineEducation;
namespace TicketManagement.Server.Services.OnlineEducation
{
    public interface IUsersTestScoreDetailsService
    {
        // Task<IEnumerable<UserTestResultsDTO>> GetUserTestScoreDetailsAsync(ClaimsPrincipal userclaims);
        //Task<UserTestScoreResponseDTO> GetUserTestScoreDetailsAsync(
        //     ClaimsPrincipal userClaims,
        //     Guid? testGuid,
        //     Guid? syllabusGuid,
        //     Guid? chapterGuid);
        Task<UserTestScoreDashboardDTO> GetUserTestScoreDetailsAsync(
           ClaimsPrincipal userClaims,
           Guid? testGuid,
           Guid? syllabusGuid,
           Guid? chapterGuid);
    }
}
