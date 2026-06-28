using System.ComponentModel.DataAnnotations.Schema;

namespace TicketManagement.Server.Models.OnlineEducation
{
    [Table("EmailOTP")]
    public class EmailOTPs
    {
        public int Id { get; set; }
        public string? Email { get; set; }
        public string? OTP { get; set; }
        public bool IsVerified { get; set; }
        public DateTime ExpiresOn { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
