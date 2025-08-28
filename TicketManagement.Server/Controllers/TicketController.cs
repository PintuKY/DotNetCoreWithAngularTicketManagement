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

        [HttpGet("{id}")]
        public async Task<ActionResult<TaskTicket>> GetTicket(int id)
        {
            var ticket = await _context.TaskTickets.FindAsync(id);
            if (ticket == null)
            {
                return NotFound();
            }                
            return ticket;
        }
        //Creat ticket
        [HttpPost]
        public async Task<IActionResult> PostTask([FromBody] TaskTicket taskTicket)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }               
            _context.TaskTickets.Add(taskTicket);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(Get), new { id = taskTicket.TicketId }, taskTicket);
        }
      //opdate
        [HttpPost("{id}")]
        public async Task<IActionResult> TicketUpdate(int id, [FromBody] TaskTicket taskticket)
        {
            if (id != taskticket.TicketId)
            {
                return BadRequest("Ticket ID mismatch");
            }

            // check if ticket exists in DB
            var existingTicket = await _context.TaskTickets.FindAsync(id);
            if (existingTicket == null)
            {
                return NotFound("Ticket not found");
            }

            // update fields
            existingTicket.Title = taskticket.Title;
            existingTicket.Description = taskticket.Description;
            existingTicket.Status = taskticket.Status;
            existingTicket.Prioritys = taskticket.Prioritys;
            existingTicket.Categorys = taskticket.Categorys;
            existingTicket.CreatedBy = taskticket.CreatedBy;
            existingTicket.AssignedTo = taskticket.AssignedTo;
            existingTicket.CreatedDate = taskticket.CreatedDate;
            existingTicket.DueDate = taskticket.DueDate;
            existingTicket.AttachmentPath = taskticket.AttachmentPath;

            // save changes
            await _context.SaveChangesAsync();

            return Ok(existingTicket);
        }
        //delete ticket
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTicket(int id)
        {
            var ticket = await _context.TaskTickets.FindAsync(id);
            if (ticket == null)
            {
                return NotFound();
            }

            _context.TaskTickets.Remove(ticket);
            await _context.SaveChangesAsync();

            return NoContent(); // 204 success
        }


    }
}
