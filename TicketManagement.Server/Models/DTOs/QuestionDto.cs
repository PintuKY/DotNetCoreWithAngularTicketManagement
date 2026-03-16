namespace TicketManagement.Server.Models.DTOs
{
    public class QuestionDto
    {
        public int Id { get; set; }
        public Guid QuestionGuid { get; set; }
        public string? QuestionText { get; set; }      
        public int ChapterID { get; set; }
        public List<OptionDto>? Options { get; set; }
    }
}
