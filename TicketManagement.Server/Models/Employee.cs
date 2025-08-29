using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TicketManagement.Server.Models
{
    public class Employee
    {
        [Key]
        public int EmpId { get; set; }
        [Required]
        [MaxLength(100)] 
        public string FullName { get; set; } = string.Empty;
        [ForeignKey("TaskTicket")]
        public int TicketId { get; set; } // FK

        [Required]
        [MaxLength(100)]
        public string Department { get; set; } = string.Empty;

        [MaxLength(100)]
        public string Designation { get; set; } = string.Empty;
       
    }
}
