using System.ComponentModel.DataAnnotations.Schema;

namespace TicketManagement.Server.Models.OnlineEducation
{
    [Table("UserTestResult")]
    public class UserTestResults
    {
        public int ResultId { get; set; }
        public Guid ResultGuid { get; set; }       
        public int UserId { get; set; }
        public int TestId { get; set; }
        public int TotalQuestions { get; set; }
        public int Attempted { get; set; } = 0;
        public int CorrectAnswers { get; set; } = 0;
        public int WrongAnswers { get; set; } = 0;
        public int NotAttempted { get; set; } = 0;
        public int Score { get; set; } = 0;
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public DateTime CreatedOn { get; set; } = DateTime.Now;
        public int SyllabusId { get; set; }
        public int ChapterId { get; set; }
    }

    //public class UserAnswers 
    //{
    //    public int Id { get; set; }
    //    public int ResultId { get; set; }
    //    public int QuestionId { get; set; }
    //    public int SyllabusId { get; set; }
    //    public int ChaptersId { get; set; }
    //    public char SelectedOption { get; set; }
    //    public char CorrectOption { get; set; }
    //    public bool IsCorrect { get; set; }
    //}
}
