using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace TicketManagement.Server.Models
{
    public class Ticket
    {

        [Key]
        public int TicketId { get; set; }

        [Required]
        [MaxLength(200)]
        public string? Title { get; set; }

        [Required]
        public string? Description { get; set; }

        [Required]
        public TicketStatus Status { get; set; } = TicketStatus.Open;

        [Required]
        public Priority Prioritys { get; set; } = Priority.High;  // Low, Medium, High, Critical

        public Category Categorys { get; set; } = Category.Bug;// e.g. Bug, Feature, Support

        [Required]
        [MaxLength(100)]
        public string? CreatedBy { get; set; }

        [MaxLength(100)]
        public string? AssignedTo { get; set; }

        [Required]
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

        public DateTime? UpdatedDate { get; set; }

        public DateTime? DueDate { get; set; }

        [MaxLength(500)]
        public string? AttachmentPath { get; set; }
    }

        public enum TicketStatus
        {
            Open = 0,
            InProgress = 1,
            Hold = 2,
            Closed = 3,
            Rejected = 4
        }
        public enum Category
        {
            Bug=0,
            Feature=1,
            Support=2
        }
        public enum Priority
        {
            Low=0,
            High=1,
            Medium=2,
            Critical=3
        }
}
