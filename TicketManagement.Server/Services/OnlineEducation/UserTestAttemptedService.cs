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

        public async Task<List<UserTestAttemptedDTO>> GetDataUserTestAttempted(ClaimsPrincipal userClaims)
        {
            var resultDto = new List<UserTestAttemptedDTO>();

            var userId = _userService.GetCurrentUserId(userClaims);
            if (!userId.HasValue)
            {
                _logger.LogWarning("GetDataUserTestAttempted: no current user in claims");
                return resultDto;
            }
            var uid = userId.Value;

            // Load all user test results for this user
            var attempts = await _db.userTestResults
                                    .AsNoTracking()
                                    .Where(r => r.UserId == uid)
                                    .ToListAsync();

            if (attempts == null || attempts.Count == 0)
                return resultDto;

            // Preload lookups to avoid repeated DB hits
            var tests = await _db.Tests.AsNoTracking().ToListAsync();
            var syllabi = await _db.syllabus.AsNoTracking().ToListAsync();
            var chapters = await _db.chapters.AsNoTracking().ToListAsync();

            // Group by TestId
            var byTest = attempts.GroupBy(r => r.TestId);

            foreach (var testGroup in byTest)
            {
                var testId = testGroup.Key;
                var testEntity = tests.FirstOrDefault(t => t.Id == testId);

                var testDto = new UserTestAttemptedDTO
                {
                    TestId = testId,
                    TestGuid = testEntity?.TestGuid ?? Guid.Empty,
                    TestName = testEntity?.TestName,
                    AttemptCount = testGroup.Count(),
                    LastAttempt = testGroup.Max(x => x.CreatedOn)
                };

                // Group testGroup by SyllabusId
                var bySyllabus = testGroup
                                    .Where(r => r.SyllabusId != 0)
                                    .GroupBy(r => r.SyllabusId);

                foreach (var syllabusGroup in bySyllabus)
                {
                    var syllId = syllabusGroup.Key;
                    var syllEntity = syllabi.FirstOrDefault(s => s.SyllabusID == syllId);

                    var syllabusDto = new SyllabusAttemptDTO
                    {
                        SyllabusId = syllId,
                        SyllabusName = syllEntity?.syllabusName,
                        SyllabusGuid = syllEntity?.syllabusGuid,
                        AttemptCount = syllabusGroup.Count()
                    };

                    // Group per chapter inside this syllabus group
                    var byChapter = syllabusGroup
                                        .Where(r => r.ChapterId != 0)
                                        .GroupBy(r => r.ChapterId);

                    foreach (var chapterGroup in byChapter)
                    {
                        var chapId = chapterGroup.Key;
                        var chapEntity = chapters.FirstOrDefault(c => c.ChapterId == chapId);

                        var chapterDto = new ChapterAttemptDTO
                        {
                            ChapterId = chapId,
                            ChapterName = chapEntity?.ChapterName,
                            AttemptCount = chapterGroup.Count(),
                            ChapterGuid = chapEntity?.ChapterGuid,
                            LastAttempt = chapterGroup.Max(x => x.CreatedOn)
                        };

                        syllabusDto.ChapterAttempts.Add(chapterDto);
                    }

                    testDto.SyllabusAttempts.Add(syllabusDto);
                }

                // Also include attempts that had no syllabus/chapter specified (aggregate under SyllabusId = 0)
                var orphanAttempts = testGroup.Where(r => (r.SyllabusId == 0 || r.SyllabusId == null) && (r.ChapterId == 0 || r.ChapterId == null)).ToList();
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

                resultDto.Add(testDto);
            }

            return resultDto;
        }
    }
}
