using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TicketManagement.Server.Models.DTOs;
using TicketManagement.Server.Models.OnlineEducation;

namespace TicketManagement.Server.Services.OnlineEducation
{
    public interface ITestService
    {
        Task<List<TestsDTO>> GetAllAsync();      
        // Updated: return multiple syllabi (List instead of single)
        Task<List<SyllabusCompactDto>> GetSyllabusForTestAsync(Guid testGuid);   
        // New: get chapters by SyllabusGuid (from URL)
        Task<List<ChapterDto>> GetChaptersBySyllabusGuidAsync(Guid syllabusGuid);
    }
}