using Microsoft.AspNetCore.Mvc;
using TicketManagement.Server.Repositorys.OnlineEducation;

namespace TicketManagement.Server.Controllers.OnlineEducation
{
    [ApiController]    
    [Route("api/[controller]")]
    public class SyllabusController : ControllerBase
    {
        private readonly ISyllabus _isyllabus;
        public SyllabusController(ISyllabus isyllabus)
        {
            _isyllabus = isyllabus;
        }

        [HttpGet("syllabusname")]       
        public async Task<IActionResult>GetSyllabus()
        {
            var data = await _isyllabus.GetAsyncSyllabus();
            return Ok(data);

        }
    }
}
