using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TicketManagement.Server.Models.DTOs
{
    public class UPaymentDTO
    {
        // Accept GUID from client (binds from string GUID)
        public Guid TestGuid { get; set; }

        public string TestName { get; set; } = string.Empty;

        // Card number and CVV as strings to preserve leading zeros and avoid numeric overflow
        public string CardNumber { get; set; } = string.Empty;
        public string Cvv { get; set; } = string.Empty;

        // Optional expiry date (client ISO string will bind to DateTime)
        public DateTime? Expiry { get; set; }

        public string PaymentMethod { get; set; } = string.Empty;

        // Use decimal for money
        public decimal PaymentPrice { get; set; }

        // UPI id can be empty
        public string UpiId { get; set; } = string.Empty;

        public List<SyllabusDt>? Syllabus { get; set; }
    }

    public class SyllabusDt
    {
        // GUID from client; binds from string GUID
        public Guid SyllGuid { get; set; }

        // Keep as string because client sent quoted id ("1") in example
        public string SyllabusID { get; set; } = string.Empty;

        public string SyllabusName { get; set; } = string.Empty;
        public int TotalChapters { get; set; }
        public int TotalQuestions { get; set; }
    }
}
