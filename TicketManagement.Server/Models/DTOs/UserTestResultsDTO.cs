namespace TicketManagement.Server.Models.DTOs
{
    public class UserTestResultsDTO
    {
        public int ResultId { get; set; }
        public Guid ResultGuid { get; set; }

        public int UserId { get; set; }

        // NEW
        public string? UserName { get; set; }

        public int TestId { get; set; }

        public int TotalQuestions { get; set; }
        public int Attempted { get; set; }
        public int CorrectAnswers { get; set; }
        public int WrongAnswers { get; set; }
        public int NotAttempted { get; set; }
        public int Score { get; set; }

        // NEW
        public int TotalMarks { get; set; }
        public decimal Accuracy { get; set; }
        public decimal Percentage { get; set; }
        public decimal Percentile { get; set; }
        public int Rank { get; set; }

        // NEW
        public TimeSpan TotalTime { get; set; }
        public decimal AverageTimePerQuestionSeconds { get; set; }

        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public DateTime CreatedOn { get; set; }

        public int SyllabusId { get; set; }
        public int ChapterId { get; set; }
    }
}