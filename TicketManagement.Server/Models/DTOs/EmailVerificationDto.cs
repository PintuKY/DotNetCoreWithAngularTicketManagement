using System.ComponentModel.DataAnnotations;

namespace TicketManagement.Server.Models.DTOs
{
    public class EmailVerificationDto
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

       public string? Otp { get; set; }
    }
}
