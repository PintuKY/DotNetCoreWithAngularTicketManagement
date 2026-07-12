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
        public DbSet<TestSyllabus> testSyllabus { get; set; }
        public DbSet<Users> users { get; set; }
        public DbSet<EmailOTPs> emailOtp { get; set; }

        // Add DbSet for UserTestResults so EF knows about the entity
        public DbSet<UserTestResults> userTestResults { get; set; }

        // Add DbSet for Test entity (maps to DB table "tests")
        public DbSet<Test> Tests { get; set; }

        // Add DbSet for UserAnswers so EF can track/persist answers
        public DbSet<UserAnswers> userAnswers { get; set; }
        public DbSet<UserProfiles> userProfiles { get; set; }

        /*
        * Solution 2 — Fluent API Mapping (Professional Way) using => ModelBuilder 
        * This is preferred in enterprise apps because:
        * clean models
        * central mapping
        * easy maintenance 
        */
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);          
            modelBuilder.Entity<Question>().ToTable("Questions");
            modelBuilder.Entity<QuestionOption>().ToTable("QuestionOptions");
            modelBuilder.Entity<Chapter>().ToTable("Chapters");
            modelBuilder.Entity<Syllabus>().ToTable("Syllabus");
            modelBuilder.Entity<Users>().ToTable("Users");
            modelBuilder.Entity<Users>().HasKey(u => u.Id);
            modelBuilder.Entity<Users>()
                .HasIndex(u => u.Email)
                .IsUnique();

            // Map UserTestResults explicitly and set key
            modelBuilder.Entity<UserTestResults>().ToTable("UserTestResult");   
            modelBuilder.Entity<UserTestResults>().HasKey(u => u.ResultId);

            // Map Test entity to "tests" table
            modelBuilder.Entity<Test>().ToTable("tests");
            modelBuilder.Entity<Test>().HasKey(t => t.Id);

            // Map UserAnswers so EF Core recognizes and maps columns correctly
            modelBuilder.Entity<UserAnswers>().ToTable("UserAnswers");
            modelBuilder.Entity<UserAnswers>().HasKey(u => u.Id);
            // ensure char columns map to char(1) in DB to match existing schema
            modelBuilder.Entity<UserAnswers>()
                .Property(u => u.SelectedOption)
                .HasColumnType("char(1)")
                .IsFixedLength();
            modelBuilder.Entity<UserAnswers>()
                .Property(u => u.CorrectOption)
                .HasColumnType("char(1)")
                .IsFixedLength();
            // optional: ensure IsCorrect has default
            modelBuilder.Entity<UserAnswers>()
                .Property(u => u.IsCorrect)
                .HasDefaultValue(false);

            // Primary Key for question options
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
