using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace TicketManagement.Server.Models.OnlineEducation
{
    [Table("Users")]
    public class Users
    {
        public int Id { get; set; }

        public Guid? UserGuid { get; set; }

        public string? FullName { get; set; }

        public string? Email { get; set; }

        public string? Mobile { get; set; }

        // Store hashed password as string
        public string? PasswordHash { get; set; }

        // Example: "Admin", "Student", "Teacher"
        public string? Role { get; set; }

        public bool IsActive { get; set; }

        public DateTime? CreatedOn { get; set; }

        public DateTime? LastLoginDate { get; set; }

        public bool EmailVerified { get; set; }

        public DateTime? UpdatedOn { get; set; }
    }
}
