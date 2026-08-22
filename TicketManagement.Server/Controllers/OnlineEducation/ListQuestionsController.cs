using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using TicketManagement.Server.Models.DTOs;
using TicketManagement.Server.Repositorys.OnlineEducation;

namespace TicketManagement.Server.Controllers.OnlineEducation
{
    [ApiController]
    [Route("api/[controller]")]
    public class ListQuestionsController: ControllerBase
    {
        private readonly IListQuestions _listquestionService;
        private readonly ILogger<ListQuestionsController> _logger;

        public ListQuestionsController(IListQuestions listquestionservice, ILogger<ListQuestionsController> logger)
        {
            _listquestionService = listquestionservice;
            _logger = logger;
        }
        
        [HttpGet("questions/{ChapterGuid:guid}")]
        public async Task<IActionResult> GetQuestions(Guid ChapterGuid, [FromQuery] string language = "en")
        {
            try
            {
                language = language?.Trim().ToLower() ?? "en";
                // Allow only en or hi
                if (language != "en" && language != "hi")
                {
                    language = "en";
                }
                // Pass ChapterGuid to service which resolves ChapterId and returns questions
                //var data = await _listquestionService.GetListQuestionsAsync(ChapterGuid);
                var data = await _listquestionService.GetListQuestionsAsync(ChapterGuid, language);
                return Ok(data);
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, "Error fetching questions for ChapterGuid {ChapterGuid}", ChapterGuid);
                return Problem(detail: ex.Message, statusCode: 500);
            }
        }        
    }
}
