using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TicketManagement.Server.DBContexts;
using TicketManagement.Server.Models;
using TicketManagement.Server.Repositorys;

namespace TicketManagement.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TicketController : ControllerBase
    {
        private readonly AppDatabaseContext _context;
        public TicketController(AppDatabaseContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<TaskTicket>>> Get()
        {
            return await _context.TaskTickets.ToListAsync();
        }
    }
}
