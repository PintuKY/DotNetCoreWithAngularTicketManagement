using System;
using System.Collections.Generic;

namespace TicketManagement.Server.Models.DTOs
{
    public class UserTestAttemptedDTO
    {
        public int TestId { get; set; }
        public Guid TestGuid { get; set; }
        public string? TestName { get; set; }
        public int AttemptCount { get; set; }
        public DateTime? LastAttempt { get; set; }

        public List<SyllabusAttemptDTO> SyllabusAttempts { get; set; } = new List<SyllabusAttemptDTO>();
    }

    public class SyllabusAttemptDTO
    {
        public int SyllabusId { get; set; }
        public Guid? SyllabusGuid { get; set; }
        public string? SyllabusName { get; set; }
        public int AttemptCount { get; set; }
        public List<ChapterAttemptDTO> ChapterAttempts { get; set; } = new List<ChapterAttemptDTO>();
    }

    public class ChapterAttemptDTO
    {
        public int ChapterId { get; set; }
        public Guid? ChapterGuid { get; set; }
        public string? ChapterName { get; set; }
        public int AttemptCount { get; set; }
        public DateTime? LastAttempt { get; set; }
    }
}
