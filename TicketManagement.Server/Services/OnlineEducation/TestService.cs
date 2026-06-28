using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using TicketManagement.Server.DBContexts;
using TicketManagement.Server.Models.DTOs;
using TicketManagement.Server.Models.OnlineEducation;

namespace TicketManagement.Server.Services.OnlineEducation
{
    public class TestService : ITestService
    {
        private readonly AppDatabaseContext _db;

        public TestService(AppDatabaseContext db)
        {
            _db = db;
        }

        public async Task<List<TestsDTO>> GetAllAsync()
        {
            var result = new List<TestsDTO>();
            result  = await _db.Tests.Select(x=> new TestsDTO
            {
                Id = x.Id,
                TestGuid = x.TestGuid,
                TestName = x.TestName,
                Description = x.Description,
                Price = x.Price,
                IsActive = x.IsActive,
                CreatedOn = x.CreatedOn,
                TotalSyllabus = _db.Set<TestSyllabus>().Count(ts => ts.TestId == x.Id && ts.IsActive),
                IsPaid = x.IsPaid == true
            }).AsNoTracking().ToListAsync();

            return result;
        }       
           
        public async Task<List<SyllabusCompactDto>> GetSyllabusForTestAsync(Guid testGuid)
        {
            var result = new List<SyllabusCompactDto>();

            // 1) Find Test by TestGuid
            var testId = await _db.Tests
                .AsNoTracking()
                .Where(t => t.TestGuid == testGuid)
                .Select(t => (int?)t.Id)
                .FirstOrDefaultAsync();

            if (testId.HasValue)
            {
                // 1.a) Find ALL active TestSyllabus records by TestId
                var syllabusIds = await _db.Set<TestSyllabus>()
                    .AsNoTracking()
                    .Where(ts => ts.TestId == testId.Value && ts.IsActive)
                    .Select(ts => ts.SyllabusId)
                    .Distinct()
                    .ToListAsync();

                if (syllabusIds.Any())
                {
                    // Fetch all corresponding Syllabus records
                    var syllabi = await _db.syllabus
                        .AsNoTracking()
                        .Where(s => syllabusIds.Contains(s.SyllabusID))
                        .Select(s => new SyllabusCompactDto
                        {
                            SyllabusID = s.SyllabusID,
                            SyllabusGuid = s.syllabusGuid,
                            SyllabusName = s.syllabusName,
                            TotalChapters = s.Chapters.Count(),
                            TotalQuestions = s.Chapters.SelectMany(c => c.Questions).Count()
                        })
                        .ToListAsync();

                    result.AddRange(syllabi);
                }
            }

            // If found by TestGuid, return early
            if (result.Any())
                return result;

            // 2) Fallback: incoming GUID might be a TestSyllabus.TestSyllabusGuid
            var syllabusIdByTestSyllabusGuid = await _db.Set<TestSyllabus>()
                .AsNoTracking()
                .Where(ts => ts.TestSyllabusGuid == testGuid && ts.IsActive)
                .Select(ts => ts.SyllabusId)
                .FirstOrDefaultAsync();

            if (syllabusIdByTestSyllabusGuid != 0)
            {
                var syllabusDto = await _db.syllabus
                    .AsNoTracking()
                    .Where(s => s.SyllabusID == syllabusIdByTestSyllabusGuid)
                    .Select(s => new SyllabusCompactDto
                    {
                        SyllabusID = s.SyllabusID,
                        SyllabusGuid = s.syllabusGuid,
                        SyllabusName = s.syllabusName,
                        TotalChapters = s.Chapters.Count(),
                        TotalQuestions = s.Chapters.SelectMany(c => c.Questions).Count()
                    })
                    .FirstOrDefaultAsync();

                if (syllabusDto != null)
                    result.Add(syllabusDto);
            }

            // If found by TestSyllabusGuid, return
            if (result.Any())
                return result;

            // 3) Last fallback: treat GUID as Syllabus.syllabusGuid
            var syllabusByGuid = await _db.syllabus
                .AsNoTracking()
                .Where(s => s.syllabusGuid == testGuid)
                .Select(s => new SyllabusCompactDto
                {
                    SyllabusID = s.SyllabusID,
                    SyllabusGuid = s.syllabusGuid,
                    SyllabusName = s.syllabusName,
                    TotalChapters = s.Chapters.Count(),
                    TotalQuestions = s.Chapters.SelectMany(c => c.Questions).Count()
                })
                .FirstOrDefaultAsync();

            if (syllabusByGuid != null)
                result.Add(syllabusByGuid);

            return result;
        }      

        public async Task<List<ChapterDto>> GetChaptersBySyllabusGuidAsync(Guid syllabusGuid)
        {
            var syllabusId = await _db.syllabus
                .AsNoTracking()
                .Where(s => s.syllabusGuid == syllabusGuid)
                .Select(s => (int?)s.SyllabusID)
                .FirstOrDefaultAsync();

            if (syllabusId == null)
                return new List<ChapterDto>();

            return await _db.chapters
                .AsNoTracking()
                .Where(c => c.SyllabusId == syllabusId.Value)
                .Select(c => new ChapterDto
                {
                    ChapterId = c.ChapterId,
                    ChapterGuid = c.ChapterGuid,
                    ChapterName = c.ChapterName,
                    Questions = new List<QuestionDto>()
                })
                .ToListAsync();
        }
    }
}
