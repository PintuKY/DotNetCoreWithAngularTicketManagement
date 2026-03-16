namespace TicketManagement.Server.Models.DTOs
{
    public class ChapterDto
    {
        public int ChapterId { get; set; }
        public Guid ChapterGuid { get; set; }
        public string ChapterName { get; set; }

        public List<QuestionDto> Questions { get; set; }
    }
}
