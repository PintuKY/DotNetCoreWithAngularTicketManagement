using TicketManagement.Server.Models;

namespace TicketManagement.Server.Services
{
    public interface ITicket
    {
        Task<IEnumerable<TaskTicket>> GetAllAsync();
        Task<TaskTicket> GetByIdAsync(int id);
        Task AddAsync(TaskTicket ticket);
        Task UpdateAsync(TaskTicket ticket);
        Task<bool> DeleteAsync(int id);
    }
}
