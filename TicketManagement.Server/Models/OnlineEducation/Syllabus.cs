namespace TicketManagement.Server.Models.OnlineEducation
{
    public class Syllabus
    {
        public int SyllabusID { get; set; }
        public Guid syllabusGuid { get; set; }
        public string syllabusName { get; set; }
        public bool IsActive {  get; set; } 
        public DateTime CreatedOn {  get; set; }
        public List<Chapter> Chapters { get; set; } = new List<Chapter>(); 
    }
}
