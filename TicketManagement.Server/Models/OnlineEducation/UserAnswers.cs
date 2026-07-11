namespace TicketManagement.Server.Models.OnlineEducation
{
    public class UserAnswers
    {
        public int Id { get; set; }
        public int ResultId { get; set; }
        public int QuestionId { get; set; }
        public int SyllabusId { get; set; }
        public int ChaptersId { get; set; }
        public char SelectedOption { get; set; }
        public char CorrectOption { get; set; }
        public bool IsCorrect { get; set; } = false;
    }
}
