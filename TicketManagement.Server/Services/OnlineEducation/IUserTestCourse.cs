using System.Security.Claims;
using System.Threading.Tasks;
using System.Collections.Generic;
using TicketManagement.Server.Models.DTOs;

namespace TicketManagement.Server.Services.OnlineEducation
{
    public interface IUserTestCourse
    {
        Task<List<UserTestCourseDTO>> GetUserTestCourseAsync(ClaimsPrincipal userClaims);
    }
}
