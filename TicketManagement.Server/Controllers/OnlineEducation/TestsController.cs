using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using TicketManagement.Server.Services.OnlineEducation;

namespace TicketManagement.Server.Controllers.OnlineEducation
{
    [ApiController]
    [Route("api/[controller]")]
    public class TestsController : ControllerBase
    {
        private readonly ITestService _testService;
        private readonly ILogger<TestsController> _logger;

        public TestsController(ITestService testService, ILogger<TestsController> logger)
        {
            _testService = testService;
            _logger = logger;
        }

        // GET: api/tests all Tests Data
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                var tests = await _testService.GetAllAsync();
                return Ok(tests);
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, "Error fetching tests");
                return Problem(detail: ex.Message, statusCode: 500);
            }
        }

        // GET: api/tests/{testGuid}/syllabus  -> get Testsyllabus (with Syllabus) for a test
        [HttpGet("{testGuid:guid}/syllabus")]
        public async Task<IActionResult> GetSyllabusForTest(Guid testGuid)
        {
            try
            {
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
        [HttpGet("{SyllabusGuid:guid}/chapters")]
        public async Task<IActionResult> GetChaptersForTestAndSyllabus(Guid SyllabusGuid)
        {
            try
            {
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