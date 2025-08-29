using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TicketManagement.Server.DBContexts;
using TicketManagement.Server.Models;

namespace TicketManagement.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EmpUserController : ControllerBase
    {
        private readonly AppDatabaseContext _context;
        public EmpUserController(AppDatabaseContext context) 
        {
            _context = context;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Employee>>> Get()
        {
            
            return await _context.Employees.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Employee>> GetById(int id)
        {
            var getemployee = await _context.Employees.FindAsync(id);
            if (getemployee == null)
            {
                return NotFound();
            }
            return getemployee;
        }

    }
}
