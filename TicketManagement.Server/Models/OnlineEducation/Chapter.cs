namespace TicketManagement.Server.Models.OnlineEducation
{
    public class Chapter
    {
        public int ChapterId { get; set; }
        public Guid ChapterGuid { get; set; }
        public int SyllabusId {  get; set; }
        public string ChapterName { get; set; }
        public string Module { get; set; }
        public string Topic { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedOn { get; set; }
        public Syllabus Syllabus { get; set; }  // ⭐ REQUIRED navigation property
        public List<Question> Questions {  get; set; } = new List<Question>();

    }
}
