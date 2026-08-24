using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using TicketManagement.Server.DBContexts;
using TicketManagement.Server.Models.DTOs;

namespace TicketManagement.Server.Services.OnlineEducation
{
    public class UserTestAttemptedService : IUserTestAttempted
    {
        private readonly AppDatabaseContext _db;
        private readonly ILogger<UserTestAttemptedService> _logger;
        private readonly IUserService _userService;

        public UserTestAttemptedService(AppDatabaseContext db, ILogger<UserTestAttemptedService> logger, IUserService userService)
        {
            _db = db;
            _logger = logger;
            _userService = userService;
        }

        public async Task<List<UserTestAttemptedDTO>> GetDataUserTestAttempted(
    ClaimsPrincipal userClaims)
        {
            var resultDto = new List<UserTestAttemptedDTO>();

            var userId = _userService.GetCurrentUserId(userClaims);

            if (!userId.HasValue)
            {
                _logger.LogWarning(
                    "GetDataUserTestAttempted: no current user in claims");

                return resultDto;
            }

            var uid = userId.Value;

            // =========================================================
            // 1. Get user test attempts
            // =========================================================

            var attempts = await _db.userTestResults
                .AsNoTracking()
                .Where(r => r.UserId == uid)
                .ToListAsync();

            if (attempts.Count == 0)
                return resultDto;


            // =========================================================
            // 2. Load ONLY required columns from Tests
            // =========================================================

            var tests = await _db.Tests
                .AsNoTracking()
                .Select(t => new
                {
                    t.Id,
                    t.TestGuid,
                    TestName = t.TestName ?? ""
                })
                .ToListAsync();


            // =========================================================
            // 3. Load ONLY required columns from Syllabus
            // =========================================================

            var syllabi = await _db.syllabus
                .AsNoTracking()
                .Select(s => new
                {
                    s.SyllabusID,
                    s.syllabusGuid,
                    SyllabusName = s.syllabusName ?? ""
                })
                .ToListAsync();


            // =========================================================
            // 4. Load ONLY required columns from Chapters
            // =========================================================

            var chapters = await _db.chapters
                .AsNoTracking()
                .Select(c => new
                {
                    c.ChapterId,
                    c.ChapterGuid,
                    ChapterName = c.ChapterName ?? ""
                })
                .ToListAsync();


            // =========================================================
            // 5. Group by Test
            // =========================================================

            var byTest = attempts.GroupBy(r => r.TestId);

            foreach (var testGroup in byTest)
            {
                var testId = testGroup.Key;

                var testEntity = tests
                    .FirstOrDefault(t => t.Id == testId);


                var testDto = new UserTestAttemptedDTO
                {
                    TestId = testId,

                    TestGuid = testEntity?.TestGuid ?? Guid.Empty,

                    TestName = testEntity?.TestName,

                    AttemptCount = testGroup.Count(),

                    LastAttempt = testGroup.Max(x => x.CreatedOn)
                };


                // =====================================================
                // 6. Group by Syllabus
                // =====================================================

                var bySyllabus = testGroup
                    .Where(r => r.SyllabusId != 0)
                    .GroupBy(r => r.SyllabusId);


                foreach (var syllabusGroup in bySyllabus)
                {
                    var syllId = syllabusGroup.Key;

                    var syllEntity = syllabi
                        .FirstOrDefault(s => s.SyllabusID == syllId);


                    var syllabusDto = new SyllabusAttemptDTO
                    {
                        SyllabusId = syllId,

                        SyllabusName = syllEntity?.SyllabusName,

                        SyllabusGuid = syllEntity?.syllabusGuid,

                        AttemptCount = syllabusGroup.Count()
                    };


                    // =================================================
                    // 7. Group by Chapter
                    // =================================================

                    var byChapter = syllabusGroup
                        .Where(r => r.ChapterId != 0)
                        .GroupBy(r => r.ChapterId);


                    foreach (var chapterGroup in byChapter)
                    {
                        var chapId = chapterGroup.Key;

                        var chapEntity = chapters
                            .FirstOrDefault(c => c.ChapterId == chapId);


                        var chapterDto = new ChapterAttemptDTO
                        {
                            ChapterId = chapId,

                            ChapterName = chapEntity?.ChapterName,

                            AttemptCount = chapterGroup.Count(),

                            ChapterGuid = chapEntity?.ChapterGuid,

                            LastAttempt = chapterGroup.Max(
                                x => x.CreatedOn)
                        };


                        syllabusDto.ChapterAttempts.Add(chapterDto);
                    }


                    testDto.SyllabusAttempts.Add(syllabusDto);
                }


                // =====================================================
                // 8. Orphan attempts
                // =====================================================

                var orphanAttempts = testGroup
                    .Where(r =>
                        (r.SyllabusId == 0 || r.SyllabusId == null) &&
                        (r.ChapterId == 0 || r.ChapterId == null))
                    .ToList();


                if (orphanAttempts.Any())
                {
                    var orphanSyll = new SyllabusAttemptDTO
                    {
                        SyllabusId = 0,

                        SyllabusName = "General",

                        SyllabusGuid = null,

                        AttemptCount = orphanAttempts.Count
                    };


                    testDto.SyllabusAttempts.Add(orphanSyll);
                }


                // =====================================================
                // 9. Add test result
                // =====================================================

                resultDto.Add(testDto);
            }


            return resultDto;
        }

        
    }
}
