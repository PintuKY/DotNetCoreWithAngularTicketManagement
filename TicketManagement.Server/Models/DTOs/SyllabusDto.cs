namespace TicketManagement.Server.Models.DTOs
{
    public class SyllabusDto
    {
        public int SyllabusID { get; set; }
        public string SyllabusName { get; set; }
        public Guid SyllabusGuid { get; set; }
        public bool IsActive { get; set; } = true;
        public List<ChapterDto> Chapters { get; set; }
    }
}
