using System.Collections.Generic;
using System;
using TicketManagement.Server.Models;
using Microsoft.EntityFrameworkCore;
namespace TicketManagement.Server.DBContexts
{
    public class AppDatabaseContext : DbContext
    {
        public AppDatabaseContext(DbContextOptions<AppDatabaseContext> options) : base(options) { }
        public DbSet<Ticket> Tickets { get; set; }
        public DbSet<TaskTicket> TaskTickets {  get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            // any additional configuration
        }

    }
}
