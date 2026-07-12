using System.ComponentModel.DataAnnotations.Schema;

namespace TicketManagement.Server.Models.OnlineEducation
{
    [Table("UserProfiles")]
    public class UserProfiles
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string? ProfileImageUrl { get; set; }
        public string? Address { get; set; }
        public string? City { get; set; }
        public string? State { get; set; }
        public DateOnly DOB { get; set; }
    }
}
