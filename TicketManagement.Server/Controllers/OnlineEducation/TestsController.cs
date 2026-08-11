using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;
using TicketManagement.Server.Constants;
using TicketManagement.Server.Services.OnlineEducation;

namespace TicketManagement.Server.Controllers.OnlineEducation
{
    [ApiController]
    [Route("api/[controller]")]
    public class TestsController : ControllerBase
    {
        private readonly ITestService _testService;
        private readonly ILogger<TestsController> _logger;
        private readonly IUserService _userService;
        private readonly IUserTestCourse _userTestCourse;

        public TestsController(ITestService testService, ILogger<TestsController> logger, IUserService userService, IUserTestCourse userTestCourse)
        {
            _testService = testService;
            _logger = logger;
            _userService = userService;
            _userTestCourse = userTestCourse;
        }

        // GET: api/tests all Tests Data
        //[Authorize] [Authorize(Roles = Roles.Admin)]
        //[Authorize(Roles = Roles.Student)]
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                // If user is authenticated, return only tests the user purchased (payment success)
                var user = await _userService.GetCurrentUserAsync(User);
                if (user != null)
                {
                    var purchased = await _userTestCourse.GetUserTestCourseAsync(User);
                    var purchasedTestIds = purchased
                        .Where(p => p != null)
                        .Select(p => p.TestId)
                        .Distinct()
                        .ToHashSet();

                    // If user purchased any tests, return them; otherwise fall back to all tests
                    if (purchasedTestIds.Any())
                    {
                        var allTests = await _testService.GetAllAsync();
                        var filtered = allTests.Where(t => purchasedTestIds.Contains(t.Id)).ToList();
                        if (filtered.Any())
                            return Ok(filtered);
                    }

                    // fall through to return all tests when user has no purchases
                }

                // Not authenticated or no purchases -> return all tests
                var tests = await _testService.GetAllAsync();
                GeneralClass.GetTestID = tests.FirstOrDefault()?.Id ?? 0;
                return Ok(tests);
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, "Error fetching tests");
                return Problem(detail: ex.Message, statusCode: 500);
            }
        }

        // GET: api/tests/{testGuid}/syllabus  -> get Testsyllabus (with Syllabus) for a test
        [Authorize(Roles = Roles.Student)]
        [HttpGet("{testGuid:guid}/syllabus")]
        public async Task<IActionResult> GetSyllabusForTest(Guid testGuid)
        {
            try
            {
                var user = await _userService.GetCurrentUserAsync(User);

                if (user == null)
                    return Unauthorized();
                var syllabus = await _testService.GetSyllabusForTestAsync(testGuid);
                if (syllabus == null)
                    return NotFound();
                return Ok(syllabus);
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, "Error fetching syllabus for test {Guid}", testGuid);
                return Problem(detail: ex.Message, statusCode: 500);
            }
        }

        // GET: api/tests/{SyllabusGuid}/chapters get chapter (with Syllabus) for a Syllabus
        [Authorize(Roles = Roles.Student)]
        [HttpGet("{SyllabusGuid:guid}/chapters")]
        public async Task<IActionResult> GetChaptersForTestAndSyllabus(Guid SyllabusGuid)
        {
            try
            {
                var user = await _userService.GetCurrentUserAsync(User);

                if (user == null)
                    return Unauthorized();
                var chapters = await _testService.GetChaptersBySyllabusGuidAsync(SyllabusGuid);
                if (chapters == null || !chapters.Any())
                    return NotFound();
                return Ok(chapters);
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, "Error fetching chapters for syllabus guid {SyllabusGuid}", SyllabusGuid);
                return Problem(detail: ex.Message, statusCode: 500);
            }
        }
    }   
}