using System.ComponentModel.DataAnnotations.Schema;

namespace TicketManagement.Server.Models.OnlineEducation
{
    [Table("Payment")]
    public class Payment
    {
        public int Id { get; set; }
        public Guid PaymentGuid { get; set; }
        public int UserId { get; set; }
        public int TestId { get; set; }
        public decimal Amount { get; set; }
        public string PaymentStatus { get; set; } = string.Empty;
        public string PaymentMethod { get; set; } = string.Empty;
        public string GatewayOrderId { get; set; } = string.Empty;
        public string GatewayPaymentId { get; set; } = string.Empty;

        // Make nullable to match DB which allows NULLs
        public DateTime? PaymentDate { get; set; }

        // DB shows TransactionId as int — use nullable int to match DB
        public int? TransactionId { get; set; }

        public bool Active { get; set; }
    }
}
