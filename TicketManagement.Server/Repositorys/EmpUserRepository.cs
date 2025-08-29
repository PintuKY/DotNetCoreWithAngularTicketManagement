using TicketManagement.Server.DBContexts;
using TicketManagement.Server.Models;
using TicketManagement.Server.Services;

namespace TicketManagement.Server.Repositorys
{
    public class EmpUserRepository : IEmployee
    {
      
        private readonly AppDatabaseContext _DbContexts;
        public EmpUserRepository(AppDatabaseContext dbcontext)
        {
            _DbContexts = dbcontext;
        }
        public Task<IEnumerable<Employee>> GetAllAsync()
        {
            return Task.FromResult<IEnumerable<Employee>>(_DbContexts.Employees);
        }
        public async Task<Employee> GetByIdAsync(int id)
        {
            return await _DbContexts.Employees.FindAsync(id);
        }
    }
}
