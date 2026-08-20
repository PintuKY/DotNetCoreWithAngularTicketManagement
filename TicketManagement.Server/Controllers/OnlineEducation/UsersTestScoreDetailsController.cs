using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using TicketManagement.Server.Services.OnlineEducation;

namespace TicketManagement.Server.Controllers.OnlineEducation
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersTestScoreDetailsController : ControllerBase
    {
        private readonly ILogger<UsersTestScoreDetailsController> _logger;
        private readonly IUsersTestScoreDetailsService _usersTSDService;
        private readonly IUserService _userService;

        public UsersTestScoreDetailsController(
            IUserService userService,
            IUsersTestScoreDetailsService usersTSDService,
            ILogger<UsersTestScoreDetailsController> logger)
        {
            _userService = userService;
            _logger = logger;
            _usersTSDService = usersTSDService;
        }

        [HttpPost("usrs-testscore-details")]
        public async Task<IActionResult> PostUserScoreDetails([FromBody] ScoreRequest request)
        {
        

            if (request == null)
            {
                return BadRequest(new { message = "Request payload is required." });
            }
            
            if (!request.testGuid.HasValue || !request.syllabusGuid.HasValue || !request.chapterGuid.HasValue)
            {
                return BadRequest(new
                {
                    message = "testGuid, syllabusGuid and chapterGuid are all required."
                });
            }
            var currentUser = await _userService.GetCurrentUserAsync(User);
            if (currentUser == null)
            {
                _logger.LogWarning("PostUserScoreDetails: unauthenticated call");
                return Unauthorized(new { message = "User not authenticated." });
            }

            // Validate presence / parseability of GUIDs
            if (!request.testGuid.HasValue && !request.syllabusGuid.HasValue && !request.chapterGuid.HasValue)
            {
                return BadRequest(new { message = "At least one of testGuid, syllabusGuid or chapterGuid must be provided and valid GUIDs." });
            }

            try
            {
                var data = await _usersTSDService.GetUserTestScoreDetailsAsync(
                            User,
                            request.testGuid,
                            request.syllabusGuid,
                            request.chapterGuid);

                return Ok(new
                {
                    success = true,
                    data
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while fetching user test score details for user {UserId}", currentUser?.Id);
                return Problem(detail: ex.Message, statusCode: 500);
            }
        }
    }

    public class ScoreRequest
    {
        // make nullable so you can detect missing vs invalid
        public Guid? testGuid { get; set; }
        public Guid? syllabusGuid { get; set; }
        public Guid? chapterGuid { get; set; }
    }
}
