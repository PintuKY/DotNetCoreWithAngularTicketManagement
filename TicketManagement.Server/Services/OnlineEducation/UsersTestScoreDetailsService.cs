using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using TicketManagement.Server.DBContexts;
using TicketManagement.Server.Models.DTOs;

namespace TicketManagement.Server.Services.OnlineEducation
{
    public class UsersTestScoreDetailsService : IUsersTestScoreDetailsService
    {
        private readonly IUserService _userService;
        private readonly ILogger<UsersTestScoreDetailsService> _logger;
        private readonly AppDatabaseContext _db;

        public UsersTestScoreDetailsService(
            IUserService userService,
            ILogger<UsersTestScoreDetailsService> logger,
            AppDatabaseContext db)
        {
            _userService = userService;
            _logger = logger;
            _db = db;
        }


        public async Task<UserTestScoreDashboardDTO>
            GetUserTestScoreDetailsAsync(
                ClaimsPrincipal userClaims,
                Guid? testGuid,
                Guid? syllabusGuid,
                Guid? chapterGuid)
        {
            try
            {
                // =============================================
                // 1. GET CURRENT LOGIN USER
                // =============================================
                var currentUser =
                    await _userService.GetCurrentUserAsync(userClaims);

                if (currentUser == null)
                {
                    throw new UnauthorizedAccessException(
                        "User not authenticated.");
                }


                // =============================================
                // 2. GET TEST
                // testGuid -> TestId
                // =============================================
                var test = await _db.Tests
                    .AsNoTracking()
                    .FirstOrDefaultAsync(x =>
                        x.TestGuid == testGuid.Value);

                if (test == null)
                {
                    throw new KeyNotFoundException(
                        "Test not found.");
                }

                int testId = test.Id;
                int totalMarks = test.TotalMarks;


                // =============================================
                // 3. GET SYLLABUS
                // syllabusGuid -> SyllabusID
                // =============================================
                var syllabus = await _db.syllabus
                    .AsNoTracking()
                    .FirstOrDefaultAsync(x =>
                        x.syllabusGuid == syllabusGuid.Value);

                if (syllabus == null)
                {
                    throw new KeyNotFoundException(
                        "Syllabus not found.");
                }

                int syllabusId = syllabus.SyllabusID;


                // =============================================
                // 4. GET CHAPTER
                // chapterGuid -> ChapterId
                // =============================================
                var chapter = await _db.chapters
                    .AsNoTracking()
                    .FirstOrDefaultAsync(x =>
                        x.ChapterGuid == chapterGuid.Value);

                if (chapter == null)
                {
                    throw new KeyNotFoundException(
                        "Chapter not found.");
                }

                int chapterId = chapter.ChapterId;


                // Validate chapter belongs to syllabus
                if (chapter.SyllabusId != syllabusId)
                {
                    throw new InvalidOperationException(
                        "Chapter does not belong to selected syllabus.");
                }


                // =============================================
                // 5. GET ALL USERS FOR SAME TEST
                // + SAME SYLLABUS
                // + SAME CHAPTER
                //
                // JOIN WITH USERS TABLE FOR USER NAME
                // =============================================
                var allResultsQuery =
                    from r in _db.userTestResults.AsNoTracking()

                    join u in _db.users.AsNoTracking()
                        on r.UserId equals u.Id

                    where r.TestId == testId
                       && r.SyllabusId == syllabusId
                       && r.ChapterId == chapterId

                    select new
                    {
                        r.ResultId,
                        r.ResultGuid,

                        r.UserId,

                        // IMPORTANT:
                        // Replace u.UserName below with your
                        // actual Users model name property
                        UserName = u.FullName,

                        r.TestId,
                        r.TotalQuestions,
                        r.Attempted,
                        r.CorrectAnswers,
                        r.WrongAnswers,
                        r.NotAttempted,
                        r.Score,
                        r.StartTime,
                        r.EndTime,
                        r.CreatedOn,
                        r.SyllabusId,
                        r.ChapterId
                    };


                // =============================================
                // 6. GET ALL RESULTS
                // ORDER HIGHEST SCORE FIRST
                // =============================================
                var rawResults = await allResultsQuery
                    .OrderByDescending(x => x.Score)
                    .ThenBy(x => x.EndTime)
                    .ToListAsync();


                int totalUsers = rawResults.Count;


                // No users attempted
                if (totalUsers == 0)
                {
                    return new UserTestScoreDashboardDTO
                    {
                        TotalScore = totalMarks,
                        TotalUsers = 0
                    };
                }


                // =============================================
                // 7. CREATE LEADERBOARD
                // =============================================
                var leaderboard =
                    new List<UserTestResultsDTO>();


                int rank = 0;
                int? previousScore = null;


                for (int i = 0; i < rawResults.Count; i++)
                {
                    var item = rawResults[i];


                    // -----------------------------------------
                    // RANK CALCULATION
                    //
                    // 70 = Rank 1
                    // 67 = Rank 2
                    // 65 = Rank 3
                    // 65 = Rank 3
                    // 64 = Rank 5
                    // -----------------------------------------
                    if (previousScore == null ||
                        item.Score != previousScore.Value)
                    {
                        rank = i + 1;
                    }


                    // -----------------------------------------
                    // ACCURACY
                    //
                    // Correct / Attempted * 100
                    //
                    // Example:
                    // 1 correct / 2 attempted
                    // = 50%
                    // -----------------------------------------
                    decimal accuracy = item.Attempted > 0
                        ? Math.Round(
                            ((decimal)item.CorrectAnswers /
                             item.Attempted) * 100,
                            2)
                        : 0;


                    // -----------------------------------------
                    // PERCENTAGE
                    //
                    // Score / TotalMarks * 100
                    //
                    // Example:
                    // 1 / 150 = 0.67%
                    // -----------------------------------------
                    decimal percentage = totalMarks > 0
                        ? Math.Round(
                            ((decimal)item.Score /
                             totalMarks) * 100,
                            2)
                        : 0;


                    // -----------------------------------------
                    // PERCENTILE
                    //
                    // Example:
                    // Rank = 11
                    // Total Users = 14
                    //
                    // Users below = 14 - 11 = 3
                    // 3 / 14 * 100 = 21.43%
                    //
                    // This matches your screenshot style.
                    // -----------------------------------------
                    decimal percentile = Math.Round(
                        ((decimal)(totalUsers - rank) /
                         totalUsers) * 100,
                        2);


                    // -----------------------------------------
                    // TOTAL TIME
                    // -----------------------------------------
                    TimeSpan totalTime =
                        item.EndTime - item.StartTime;


                    // Safety
                    if (totalTime.TotalSeconds < 0)
                    {
                        totalTime = TimeSpan.Zero;
                    }


                    // -----------------------------------------
                    // AVERAGE TIME PER QUESTION
                    //
                    // Total Seconds / Attempted Questions
                    // -----------------------------------------
                    decimal averageTimePerQuestionSeconds =
                        item.Attempted > 0
                        ? Math.Round(
                            (decimal)totalTime.TotalSeconds /
                            item.Attempted,
                            2)
                        : 0;


                    leaderboard.Add(
                        new UserTestResultsDTO
                        {
                            ResultId = item.ResultId,
                            ResultGuid = item.ResultGuid,

                            UserId = item.UserId,
                            UserName = item.UserName,

                            TestId = item.TestId,

                            TotalQuestions = item.TotalQuestions,
                            Attempted = item.Attempted,
                            CorrectAnswers = item.CorrectAnswers,
                            WrongAnswers = item.WrongAnswers,
                            NotAttempted = item.NotAttempted,
                            Score = item.Score,

                            TotalMarks = totalMarks,
                            Accuracy = accuracy,
                            Percentage = percentage,
                            Percentile = percentile,
                            Rank = rank,

                            TotalTime = totalTime,
                            AverageTimePerQuestionSeconds =
                                averageTimePerQuestionSeconds,

                            StartTime = item.StartTime,
                            EndTime = item.EndTime,
                            CreatedOn = item.CreatedOn,

                            SyllabusId = item.SyllabusId,
                            ChapterId = item.ChapterId
                        });


                    previousScore = item.Score;
                }


                // =============================================
                // 8. GET CURRENT USER RESULT
                // =============================================
                var currentUserResult =
                    leaderboard.FirstOrDefault(x =>
                        x.UserId == currentUser.Id);


                // =============================================
                // 9. GET TOPPER
                // =============================================
                var topperResult =
                    leaderboard.FirstOrDefault();


                // =============================================
                // 10. RETURN DASHBOARD DATA
                // =============================================
                return new UserTestScoreDashboardDTO
                {
                    // Current user dashboard
                    TotalScore = totalMarks,

                    Score = currentUserResult?.Score ?? 0,

                    Accuracy =
                        currentUserResult?.Accuracy ?? 0,

                    Rank =
                        currentUserResult?.Rank ?? 0,

                    TotalUsers = totalUsers,

                    Percentage =
                        currentUserResult?.Percentage ?? 0,

                    Percentile =
                        currentUserResult?.Percentile ?? 0,

                    Attempted =
                        currentUserResult?.Attempted ?? 0,

                    CorrectAnswers =
                        currentUserResult?.CorrectAnswers ?? 0,

                    WrongAnswers =
                        currentUserResult?.WrongAnswers ?? 0,

                    NotAttempted =
                        currentUserResult?.NotAttempted ?? 0,

                    TotalQuestions =
                        currentUserResult?.TotalQuestions ?? 0,

                    TotalTime =
                        currentUserResult?.TotalTime ??
                        TimeSpan.Zero,

                    AverageTimePerQuestionSeconds =
                        currentUserResult
                            ?.AverageTimePerQuestionSeconds ?? 0,

                    // Current User
                    CurrentUserResult = currentUserResult,

                    // Topper
                    TopperResult = topperResult,

                    // Leaderboard
                    Leaderboard = leaderboard
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Error getting test score dashboard");

                throw;
            }
        }
    }
}