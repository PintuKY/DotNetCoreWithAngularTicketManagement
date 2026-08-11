using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TicketManagement.Server.DBContexts;
using TicketManagement.Server.Models.DTOs;
using TicketManagement.Server.Models.OnlineEducation;

namespace TicketManagement.Server.Services.OnlineEducation
{
    public class UserTestCourseService : IUserTestCourse
    {
        private readonly AppDatabaseContext _db;
        private readonly ILogger<UserTestCourseService> _logger;
        private readonly IUserService _userService;
        public UserTestCourseService(AppDatabaseContext db, ILogger<UserTestCourseService> logger, IUserService userService)
        {
            _db = db;
            _logger = logger;
            _userService = userService;
        }

        public async Task<List<UserTestCourseDTO>> GetUserTestCourseAsync(System.Security.Claims.ClaimsPrincipal userClaims)
        {
            try
            {
                var userId = _userService.GetCurrentUserId(userClaims);
                if (!userId.HasValue)
                {
                    _logger.LogWarning("GetUserTestCourseAsync: no user id in claims");
                    return new List<UserTestCourseDTO>();
                }

                var uid = userId.Value;

                // Join UserTestCourse with Payment where payment status indicates completed purchase.
                var joined = await (from uc in _db.Set<UserTestCourse>().AsNoTracking()
                                    join p in _db.Set<Payment>().AsNoTracking() on uc.PaymentId equals p.Id
                                    where uc.UserId == uid && (p.PaymentStatus == "Success" || p.PaymentStatus == "Completed" || p.PaymentStatus == "Paid")
                                    select new { uc, p })
                                   .ToListAsync();

                var result = new List<UserTestCourseDTO>(capacity: joined.Count);

                foreach (var item in joined)
                {
                    // load test info
                    var test = await _db.Tests.AsNoTracking().FirstOrDefaultAsync(t => t.Id == item.uc.TestId);

                    var testDto = test == null ? null : new TestsCourseDTO
                    {
                        Id = test.Id,
                        TestGuid = test.TestGuid,
                        TestName = test.TestName,
                        Description = test.Description,
                        IsActive = test.IsActive,
                        IsPaid = test.IsPaid
                    };

                    // Populate syllabusData: find active TestSyllabus entries for this test, then load Syllabus + Chapters
                    List<SyllabusDTO> syllabusDtos = new List<SyllabusDTO>();
                    if (test != null)
                    {
                        var syllabusIds = await _db.Set<TestSyllabus>()
                                                   .AsNoTracking()
                                                   .Where(ts => ts.TestId == test.Id && ts.IsActive)
                                                   .Select(ts => ts.SyllabusId)
                                                   .Distinct()
                                                   .ToListAsync();

                        if (syllabusIds.Any())
                        {
                            syllabusDtos = await _db.syllabus
                                .AsNoTracking()
                                .Where(s => syllabusIds.Contains(s.SyllabusID))
                                .Select(s => new SyllabusDTO
                                {
                                    SyllabusID = s.SyllabusID,
                                    SyllabusName = s.syllabusName,
                                    SyllabusGuid = s.syllabusGuid,
                                    chapters = s.Chapters.Select(c => new ChapterDTO
                                    {
                                        ChapterId = c.ChapterId,
                                        ChapterGuid = c.ChapterGuid,
                                        ChapterName = c.ChapterName
                                    }).ToList()
                                })
                                .ToListAsync();
                        }
                    }

                    var dto = new UserTestCourseDTO
                    {
                        ID = item.uc.ID,
                        UserCoursGuid = item.uc.UserCoursGuid,
                        UserId = item.uc.UserId,
                        TestId = item.uc.TestId,
                        testData = testDto != null ? new List<TestsCourseDTO> { testDto } : new List<TestsCourseDTO>(),
                        syllabusData = syllabusDtos,
                        paymentData = new List<PaymentDTO>
                            {
                                new PaymentDTO
                                {
                                    Amount = item.p.Amount,
                                    PaymentStatus = item.p.PaymentStatus
                                }
                            }
                    };

                    result.Add(dto);
                }

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in GetUserTestCourseAsync");
                return new List<UserTestCourseDTO>();
            }
        }
    }
}
