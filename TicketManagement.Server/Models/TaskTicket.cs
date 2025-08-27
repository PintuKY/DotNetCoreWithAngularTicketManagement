using System.ComponentModel.DataAnnotations;

namespace TicketManagement.Server.Models
{
    public class TaskTicket
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
}
