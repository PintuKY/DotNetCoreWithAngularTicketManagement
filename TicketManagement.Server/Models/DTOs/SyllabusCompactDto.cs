using TicketManagement.Server.Models.OnlineEducation;

namespace TicketManagement.Server.Models.DTOs
{
    public class SyllabusCompactDto
    {        
        public int SyllabusID { get; set; }
        public Guid SyllabusGuid { get; set; }
        public string SyllabusName { get; set; }
        public int TotalChapters { get; set; }
        public int TotalQuestions { get; set; }
         
    }

}
