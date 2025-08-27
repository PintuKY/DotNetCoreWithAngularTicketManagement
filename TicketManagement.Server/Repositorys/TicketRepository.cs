using TicketManagement.Server.Models;
using TicketManagement.Server.Services;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using TicketManagement.Server.DBContexts;
namespace TicketManagement.Server.Repositorys
{
    public class TicketRepository : ITicket
    {
       private readonly AppDatabaseContext _DbContexts;
        public TicketRepository(AppDatabaseContext dbcontext)
        {
            _DbContexts = dbcontext;
        }
        public Task<IEnumerable<TaskTicket>> GetAllAsync()
        {
            return Task.FromResult<IEnumerable<TaskTicket>>(_DbContexts.TaskTickets);
        }
        public async Task<TaskTicket> GetByIdAsync(int id)
        {
            return await _DbContexts.TaskTickets.FindAsync(id);
        }
        public async Task AddAsync(TaskTicket ticket)
        {
            _DbContexts.TaskTickets.Add(ticket);
            await _DbContexts.SaveChangesAsync();
            
        }
        public async Task UpdateAsync(TaskTicket ticket)
        {
            _DbContexts.TaskTickets.Update(ticket);
            await _DbContexts.SaveChangesAsync();
            
        }
        public async Task<bool> DeleteAsync(int id)
        {
            var ticket = await _DbContexts.TaskTickets.FindAsync(id);
            if (ticket == null)
                return false;

            _DbContexts.TaskTickets.Remove(ticket);
            await _DbContexts.SaveChangesAsync();
            return true;
        }
    }
}
