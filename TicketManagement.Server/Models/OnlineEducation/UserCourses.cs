using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace TicketManagement.Server.Models.OnlineEducation
{
    [Table("UserCourses")]
    public class UserCourses
    {
        public int Id { get; set; }

        // DB allows NULL for UserCoursGuid
        public Guid? UserCoursGuid { get; set; }

        public int UserId { get; set; }

        public int TestId { get; set; }

        // PaymentId is nullable in DB
        public int? PaymentId { get; set; }

        // PurchaseDate non-nullable in DB
        public DateTime PurchaseDate { get; set; }

        // ExpiryDate nullable in DB
        public DateTime? ExpiryDate { get; set; }

        // Status is BIT and appears NOT NULL in DB → keep non-nullable bool
        public bool Status { get; set; }

        // CreatedDate non-nullable
        public DateTime CreatedDate { get; set; }

        // UpdatedDate nullable
        public DateTime? UpdatedDate { get; set; }

        // Progress decimal(5,2) nullable
        public decimal? Progress { get; set; }

        // LastAccessed nullable
        public DateTime? LastAccessed { get; set; }

        // Completed / CertificateIssued / CertificateExpired are BIT columns and appear nullable in DB snapshot
        public bool? Completed { get; set; }
        public bool? CertificateIssued { get; set; }
        public bool? CertificateExpired { get; set; }
    }
}
