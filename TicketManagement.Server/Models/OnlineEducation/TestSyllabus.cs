using System.ComponentModel.DataAnnotations.Schema;

namespace TicketManagement.Server.Models.OnlineEducation
{
    [Table("TestSyllabus")]
    public class TestSyllabus
    {
        public int Id { get; set; }
        public Guid TestSyllabusGuid { get; set; }
        public int TestId { get; set; }
        public int SyllabusId { get; set; }
        public bool IsActive { get; set; }
     
    }
}
