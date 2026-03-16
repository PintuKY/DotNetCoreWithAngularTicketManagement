using Microsoft.AspNetCore.Mvc;
using TicketManagement.Server.Models.DTOs;
using TicketManagement.Server.Repositorys.OnlineEducation;

namespace TicketManagement.Server.Controllers.OnlineEducation
{
    [ApiController]
    [Route("api/[controller]")]
    public class ListQuestionsController: ControllerBase
    {
        private readonly IListQuestions _listquestionService;
        public ListQuestionsController(IListQuestions listquestionservice)
        {
            _listquestionService = listquestionservice;
        }
        [HttpGet("questions")]
        public async Task<IActionResult> GetQuestions()
        {
            var data = await _listquestionService.GetListQuestionsAsync();
            return Ok(data);
        }

        
    }
}
