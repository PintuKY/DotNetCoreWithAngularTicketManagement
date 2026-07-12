    using Microsoft.AspNetCore.Mvc;
    using System.Threading.Tasks;
    using TicketManagement.Server.Models.DTOs;
    using TicketManagement.Server.Services.OnlineEducation;

    namespace TicketManagement.Server.Controllers.OnlineEducation
    {
        [ApiController]
        [Route("api/[controller]")]
        public class UserProfileController : ControllerBase
        {
            private readonly IUserProfilesService _iuserprofile;
            private readonly ILogger<UserProfileController> _ilogger;

            public UserProfileController(ILogger<UserProfileController> ilogger, IUserProfilesService iuserprofile)
            {
                _ilogger = ilogger;
                _iuserprofile = iuserprofile;
            }

            [HttpPost("updateuserprofile")]
            public async Task<IActionResult> UpdateUserProfile([FromBody] UserProfilesDTO data)
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var success = await _iuserprofile.SaveUserprofileAsync(data, User);
                if (!success)
                {
                    _ilogger.LogWarning("UpdateUserProfile failed for current user.");
                    return BadRequest(new { message = "Unable to update profile. Check data or authentication." });
                }

                return Ok(new { message = "Profile updated successfully." });
            }

            [HttpGet("getmeprofile")]
            public async Task<IActionResult> GetMyProfile()
            {
                var profile = await _iuserprofile.GetUserProfileAsync(User);
                if (profile == null)
                    return NotFound(new { message = "Profile not found or not authenticated." });

                return Ok(profile);
            }

            [HttpPost("changepassword")]
            public async Task<IActionResult> ChangePassword([FromBody] PasswordUpdateDTO model)
            {
                if (model == null || string.IsNullOrWhiteSpace(model.CurrentPassword) || string.IsNullOrWhiteSpace(model.NewPassword))
                    return BadRequest(new { message = "Invalid password payload." });

                if (model.NewPassword != model.ConfirmPassword)
                    return BadRequest(new { message = "New password and confirm password do not match." });
            if (!GeneralClass.IsStrongPassword(model.NewPassword))
            {
                return BadRequest(new
                {
                    message = "Password must contain uppercase, lowercase, number and special character."
                });
            }
            var changed = await _iuserprofile.ChangePasswordAsync(model, User);
                if (!changed)
                    return BadRequest(new { message = "Password change failed. Verify current password and policy." });

                return Ok(new { message = "Password changed successfully." });
            }
        }
    }
