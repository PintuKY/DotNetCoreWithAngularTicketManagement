using System.ComponentModel.DataAnnotations;

namespace TicketManagement.Server.Models.OnlineEducation
{
    public class Question
    {
        public int Id { get; set; }
        public Guid QuestionGuid { get; set; }
        public string? QuestionText { get; set; }
        public int ChapterID { get; set; }
        public List<QuestionOption> QuestionOptions { get; set; } = new List<QuestionOption>();
    }
    public class QuestionOption
    {
        [Key]
        public int OptionId { get; set; }
        public string? OptionText { get; set; }
        public int QuestionId { get; set; }
        public Question? Question { get; set; }
    }   
}
