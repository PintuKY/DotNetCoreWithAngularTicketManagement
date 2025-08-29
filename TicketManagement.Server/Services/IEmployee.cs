using TicketManagement.Server.Models;

namespace TicketManagement.Server.Services
{
    public interface IEmployee
    {
        Task<IEnumerable<Employee>> GetAllAsync();
        Task<Employee> GetByIdAsync(int id);
    }
}
