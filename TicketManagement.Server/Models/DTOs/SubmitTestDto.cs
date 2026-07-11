namespace TicketManagement.Server.Models.DTOs
{
    public class TestSubmissionDto
    {
        public int ChapterId { get; set; }
        public int SyllabusID { get; set; }
        public Dictionary<int, int> AnswersWithOptionIds { get; set; }
        public Dictionary<int, string> Answers { get; set; }

        public DateTime StartDateTime { get; set; }

        public DateTime EndDateTime { get; set; }        

        public int QuestionsAnswered { get; set; }

        public int QuestionsAttempted { get; set; }

        public int QuestionsNotAnswered { get; set; }

        public int QuestionsSkipped { get; set; }

        public int TotalTimeSpentSeconds { get; set; }
        public SummaryDto Summary { get; set; }

        public List<QuestionMetadataDto> QuestionMetadata { get; set; }

    }
    public class SummaryDto
    {
        public int TotalQuestions { get; set; }

        public int Answered { get; set; }

        public int Attempted { get; set; }

        public int Skipped { get; set; }

        public int NotAnswered { get; set; }
    }

    public class QuestionMetadataDto
    {
        public int QuestionId { get; set; }

        public string QuestionText { get; set; }

        // Accept string so values like "A", "B", "C" bind successfully.
        public string SelectedAnswer { get; set; }

        public bool IsAnswered { get; set; }

        public bool IsSkipped { get; set; }
    }

}
