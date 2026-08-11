using System;

namespace TicketManagement.Server.Models.OnlineEducation
{
    [Tags("UserCourses")]
    public class UserTestCourse
    {
        public int ID { get; set; }
        public Guid UserCoursGuid { get; set; }
        public int UserId { get; set; }
        public int TestId { get; set; }
        public int PaymentId { get; set; }

        // These date columns can be NULL in the DB → make them nullable if needed
        public DateTime? PurchaseDate { get; set; }
        public DateTime? ExpiryDate { get; set; }

        // Status column is BIT in DB — use bool (or bool? if nullable)
        public bool Status { get; set; }

        // Make created/updated/lastaccessed nullable to avoid SqlNullValueException
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }

        // DB shows Progress decimal(5,2) — use decimal? to match DB
        public decimal? Progress { get; set; }

        public DateTime? LastAccessed { get; set; }
        public bool Completed { get; set; }
        public bool CertificateIssued { get; set; }
        public bool CertificateExpired { get; set; }
    }
}
