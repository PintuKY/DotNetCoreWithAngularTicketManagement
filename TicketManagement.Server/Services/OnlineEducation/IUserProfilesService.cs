using System.Security.Claims;
using System.Threading.Tasks;
using TicketManagement.Server.Models.DTOs;

namespace TicketManagement.Server.Services.OnlineEducation
{
    public interface IUserProfilesService
    {
        Task<bool> SaveUserprofileAsync(UserProfilesDTO data, ClaimsPrincipal userClaims);
        Task<UserProfilesDTO?> GetUserProfileAsync(ClaimsPrincipal userClaims);
        Task<bool> ChangePasswordAsync(PasswordUpdateDTO model, ClaimsPrincipal userClaims);
    }
}
