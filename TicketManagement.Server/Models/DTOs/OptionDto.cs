namespace TicketManagement.Server.Models.DTOs
{
    public class OptionDto
    {
        public int OptionId { get; set; }
        public string? OptionText { get; set; }
        public int QuestionId { get; set; }
        public bool IsCorrect { get; set; }
    }
}
