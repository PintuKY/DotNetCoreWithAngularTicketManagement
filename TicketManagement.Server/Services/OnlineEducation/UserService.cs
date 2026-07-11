using Microsoft.EntityFrameworkCore;
using System;
using System.Security.Claims;
using TicketManagement.Server.DBContexts;
using TicketManagement.Server.Constants;
using TicketManagement.Server.Models.OnlineEducation;
namespace TicketManagement.Server.Services.OnlineEducation
{
    public class UserService : IUserService
    {
        private readonly AppDatabaseContext _db;
        private readonly ILogger<UserService> _logger;
        public UserService(AppDatabaseContext db, ILogger<UserService> logger)
        {
            _db = db;
            _logger = logger;
        }

        public int? GetCurrentUserId(ClaimsPrincipal userClaims)
        {
            try
            {
                //var userId =User.FindFirst(JwtClaimNames.UserId)?.Value;
                var value = userClaims.FindFirst(JwtClaimNames.UserId)?.Value;
                if (int.TryParse(value, out int userId))
                    return userId;

                return null;
            }
            catch (Exception ex)
            {
                // Log the exception
                _logger.LogError(ex,"Error in GetCurrentUserAsync");
                Console.WriteLine($"Error in GetCurrentUserId: {ex.Message}");
                return null;
            }
        }
        public Guid? GetCurrentUserGuid(ClaimsPrincipal userClaims)
        {
            try
            {
               // var value = userClaims.FindFirst("UserGuid")?.Value;
                var value = userClaims.FindFirst(JwtClaimNames.UserGuid)?.Value;
                if (Guid.TryParse(value, out Guid userGuid))
                    return userGuid;

                return null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in GetCurrentUserAsync");
                // Log the exception
                Console.WriteLine($"Error in GetCurrentUserGuid: {ex.Message}");
                return null;
            }
        }
        public string? GetCurrentUserEmail(ClaimsPrincipal userClaims)
        {
            try
            {
                //return userClaims.FindFirst(JwtClaimNames.Email)?.Value
                //   ?? userClaims.FindFirst(JwtClaimNames.FullName)?.Value
                //   ?? userClaims.FindFirst("email")?.Value; ;
                return userClaims.FindFirst(JwtClaimNames.Email)?.Value;
            }
            catch (Exception ex)
            {
                // Log the exception
                _logger.LogError(ex, "Error in GetCurrentUserAsync");
                Console.WriteLine($"Error in GetCurrentUserEmail: {ex.Message}");
                return null;
            }
        }
        public string? GetCurrentUserRole(ClaimsPrincipal userClaims)
        {
            try
            {
                return userClaims.FindFirst(JwtClaimNames.Role)?.Value;
            }
            catch (Exception ex)
            {
                // Log the exception
                _logger.LogError(ex, "Error in GetCurrentUserAsync");
                Console.WriteLine($"Error in GetCurrentUserRole: {ex.Message}");
                return null;
            }
        }
        public async Task<Users?> GetCurrentUserAsync(ClaimsPrincipal userClaims)
        {
            try
            {
                var userId = GetCurrentUserId(userClaims);

                if (!userId.HasValue)
                    return null;

                return await _db.users
                    .FirstOrDefaultAsync(x => x.Id == userId.Value);
            }
            catch (Exception ex)
            {
                // Log the exception
                _logger.LogError(ex, "Error in GetCurrentUserAsync");
                Console.WriteLine($"Error in GetCurrentUserAsync: {ex.Message}");
                return null;
            }
        }
        //GetCurrentUserId()
        //GetCurrentUserGuid()
        //GetCurrentUserEmail()
        //GetCurrentUserRole()
        //GetCurrentUserAsync()
    }    
}
