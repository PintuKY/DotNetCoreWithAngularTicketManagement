using System.Collections.Generic;
using System;
using TicketManagement.Server.Models;
using Microsoft.EntityFrameworkCore;
using TicketManagement.Server.Models.OnlineEducation;
namespace TicketManagement.Server.DBContexts
{
    public class AppDatabaseContext : DbContext
    {
        public AppDatabaseContext(DbContextOptions<AppDatabaseContext> options) : base(options) { }    
        public DbSet<Employee> Employees { get; set; }    
        public DbSet<Question> question { get; set; }
        public DbSet<Syllabus> syllabus { get; set; }
        public DbSet<Chapter> chapters { get; set; }
        public DbSet<QuestionOption> questionOptions { get; set; }
        /*
        * Solution 2 — Fluent API Mapping (Professional Way) using => ModelBuilder 
        * This is preferred in enterprise apps because:
        *clean models
        *central mapping
        *easy maintenance 
        */
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);          
            modelBuilder.Entity<Question>().ToTable("Questions");
            modelBuilder.Entity<QuestionOption>().ToTable("QuestionOptions");
            modelBuilder.Entity<Chapter>().ToTable("Chapters");
            modelBuilder.Entity<Syllabus>().ToTable("Syllabus");
            // Primary Key
            modelBuilder.Entity<QuestionOption>().HasKey(q => q.OptionId);          
            modelBuilder.Entity<QuestionOption>()
                .HasOne(o => o.Question)
                .WithMany(q => q.QuestionOptions)
                .HasForeignKey(o => o.QuestionId);           
            modelBuilder.Entity<Chapter>().HasKey(c => c.ChapterId);

            modelBuilder.Entity<Chapter>()
                .HasOne(c => c.Syllabus)
                .WithMany(s => s.Chapters)
                .HasForeignKey(c => c.SyllabusId);

            // Relationship (IMPORTANT) full navigation both side
            //modelBuilder.Entity<QuestionOption>()
            //    .HasOne(q => q.Question)                 // child → parent
            //    .WithMany(q => q.QuestionOptions)        // parent → children
            //    .HasForeignKey(q => q.QuestionId)        // FK column
            //    .OnDelete(DeleteBehavior.Cascade);       // optional  
        }



    }
}
