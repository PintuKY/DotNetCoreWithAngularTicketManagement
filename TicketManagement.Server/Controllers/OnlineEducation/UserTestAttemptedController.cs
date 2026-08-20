using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using TicketManagement.Server.Services.OnlineEducation;

namespace TicketManagement.Server.Controllers.OnlineEducation
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserTestAttemptedController : ControllerBase
    {
        private readonly IUserTestAttempted _iusertestattempted;
        private readonly IUserService _userservice;

        public UserTestAttemptedController(IUserTestAttempted userTestAttempted, IUserService iuserservice)
        {
            _iusertestattempted = userTestAttempted;
            _userservice = iuserservice;
        }

        [HttpGet("usertest-attempted")]
        public async Task<IActionResult> Index()
        {
            var userId = _userservice.GetCurrentUserId(User);
            if (!userId.HasValue)
                return Unauthorized(new { message = "User not authenticated" });

            var data = await _iusertestattempted.GetDataUserTestAttempted(User);
            return Ok(data);
        }
    }
}
