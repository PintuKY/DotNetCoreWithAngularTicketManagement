
namespace TicketManagement.Server.Models.DTOs
{
    public class UserTestCourseDTO
    {
        public int ID { get; set; }
        public Guid UserCoursGuid { get; set; }
        public int UserId { get; set; }
        public int TestId { get; set; }     
        public List<TestsCourseDTO>? testData { get; set; }
        public List<SyllabusDTO>? syllabusData { get; set; }            
        public List<PaymentDTO>? paymentData { get; set; }
    }
    public class SyllabusDTO
    {
        public int SyllabusID { get; set; }
        public string SyllabusName { get; set; }
        public Guid SyllabusGuid { get; set; }
        public List<ChapterDTO> chapters { get; set; }
    }
    public class ChapterDTO
    {
        public int ChapterId { get; set; }
        public Guid ChapterGuid { get; set; }
        public string ChapterName { get; set; }

        //public List<QuestionDto> Questions { get; set; }
    }
    public class TestsCourseDTO
    {
        public int Id { get; set; }
        public Guid TestGuid { get; set; }
        public string? TestName { get; set; }
        public string? Description { get; set; }       
        public bool IsActive { get; set; }       
        public bool IsPaid { get; set; }
    }
    public class PaymentDTO
    {
        public decimal Amount { get; set; }
        public string PaymentStatus { get; set; } = string.Empty;
    }
}
