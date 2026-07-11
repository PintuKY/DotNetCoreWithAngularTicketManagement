using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using TicketManagement.Server.Models.DTOs;
using TicketManagement.Server.Models.OnlineEducation;
using TicketManagement.Server.Objects;
using TicketManagement.Server.Repositorys.OnlineEducation;
using TicketManagement.Server.DBContexts;
using TicketManagement.Server.Services.OnlineEducation;

namespace TicketManagement.Server.Controllers.OnlineEducation
{
    [ApiController]
    [Route("api/[controller]")]
    public class ExamSubmitController : ControllerBase
    {
        private readonly QuestionsDatas _questionsDatas;
        private readonly AppDatabaseContext _dbContext;
        private readonly ILogger<ExamSubmitController> _logger;
        private readonly IUserService _userService;
        public ExamSubmitController(QuestionsDatas questionsDatas, AppDatabaseContext dbContext, ILogger<ExamSubmitController> logger, IUserService userService)
        {
            _questionsDatas = questionsDatas;
            _dbContext = dbContext;
            _logger = logger;
            _userService = userService;
        }

        [HttpPost("examsubmits")]
        public async Task<IActionResult> ExamSubmite([FromBody] TestSubmissionDto data)
        {

            var user = await _userService.GetCurrentUserAsync(User);
            if (user == null)
                return Unauthorized();

            if (data == null)
            {
                return BadRequest("Request body is required.");
            }

            // Normalize incoming collections / values to avoid NullReferenceException
            var answers = data.AnswersWithOptionIds ?? new Dictionary<int, int>();
            var chapterId = data.ChapterId;
            var syllabuid = data.SyllabusID;
            int totaltimeinsecond = data.TotalTimeSpentSeconds;

            // Load the questions for the chapter so we can compute correctness
            var question_of_chapter = await _questionsDatas.GetAllQuestionOfChapterAns(chapterId) ?? new List<QuestionDto>();

            // Prefer client summary if present; otherwise fall back to DB-derived count
            int totalQuestion = data.Summary?.TotalQuestions ?? question_of_chapter.Count;

            // Attempted: prefer explicit answers count or fallback to DTO numeric field
            int Totalattempted = answers.Count > 0 ? answers.Count : (data.QuestionsAttempted > 0 ? data.QuestionsAttempted : 0);
            int notAttempted = Math.Max(0, totalQuestion - Totalattempted);

            int correctAnswers = 0;
            int wrongAnswers = 0;

            // Compute totals by comparing selected option id vs correct option id (per question)
            foreach (var q in question_of_chapter)
            {
                var correctOption = q.Options?.FirstOrDefault(o => o.IsCorrect);
                if (answers.ContainsKey(q.Id))
                {
                    var selected = answers[q.Id];
                    if (correctOption != null && selected == correctOption.OptionId)
                    {
                        correctAnswers++;
                    }
                    else
                    {
                        wrongAnswers++;
                    }
                }
            }

            // Simple scoring — adjust if your system uses different weights/negative marking
            int score = correctAnswers;

            // Resolve Test.Id for the chapter (preferred) or fail early.
            var testEntity = await _dbContext.Tests
                .AsNoTracking()
                .FirstOrDefaultAsync(t => t.Id == GeneralClass.GetTestID);

            if (testEntity == null)
            {
                _logger.LogWarning("No test found for ChapterId {ChapterId} while attempting to submit exam", chapterId);
                return BadRequest($"No test found for ChapterId {chapterId}.");
            }

            var userTestResults = new UserTestResults
            {
                ResultGuid = Guid.NewGuid(),
                UserId = user.Id,
                TestId = testEntity.Id,
                SyllabusId = syllabuid,
                ChapterId = chapterId,
                TotalQuestions = totalQuestion,
                Attempted = Totalattempted,
                CorrectAnswers = correctAnswers,
                WrongAnswers = wrongAnswers,
                NotAttempted = notAttempted,
                Score = score,
                StartTime = data.StartDateTime,
                EndTime = data.EndDateTime,
                CreatedOn = DateTime.Now
            };

            // Persist user test results first so we have ResultId available for answers
            try
            {
                await _dbContext.AddAsync(userTestResults);
                await _dbContext.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "DB error saving UserTestResults for chapter {ChapterId}", chapterId);
                return Problem(detail: ex.InnerException?.Message ?? ex.Message, statusCode: 500);
            }

            // Build and persist UserAnswers records (one per answered question)
            try
            {
                // Map questions by id for quick lookup
                var questionMap = question_of_chapter.ToDictionary(q => q.Id, q => q);

                var userAnswersList = new List<UserAnswers>();

                foreach (var kvp in answers)
                {
                    var questionId = kvp.Key;
                    var selectedOptionId = kvp.Value;

                    if (!questionMap.TryGetValue(questionId, out var qdto))
                    {
                        // If question metadata not available from cached list, skip or optionally log.
                        _logger.LogDebug("Answered question id {QuestionId} not found in loaded chapter questions; skipping persisting answer.", questionId);
                        continue;
                    }

                    var correctOption = qdto.Options?.FirstOrDefault(o => o.IsCorrect);
                    bool isCorrect = correctOption != null && selectedOptionId == correctOption.OptionId;

                    // Convert option index to letter (A, B, C, ...) for storage in char fields.
                    char selectedChar = ' ';
                    char correctChar = ' ';

                    if (qdto.Options != null && qdto.Options.Count > 0)
                    {
                        var selIndex = qdto.Options.FindIndex(o => o.OptionId == selectedOptionId);
                        if (selIndex >= 0)
                            selectedChar = (char)('A' + selIndex);

                        var corIndex = qdto.Options.FindIndex(o => o.IsCorrect);
                        if (corIndex >= 0)
                            correctChar = (char)('A' + corIndex);
                    }

                    var userAnswer = new UserAnswers
                    {
                        ResultId = userTestResults.ResultId,
                        QuestionId = questionId,
                        SyllabusId = syllabuid,
                        ChaptersId = chapterId,
                        SelectedOption = selectedChar,
                        CorrectOption = correctChar,
                        IsCorrect = isCorrect
                    };

                    userAnswersList.Add(userAnswer);
                }

                if (userAnswersList.Count > 0)
                {
                    await _dbContext.AddRangeAsync(userAnswersList);
                    await _dbContext.SaveChangesAsync();
                }
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "DB error saving UserAnswers for ResultId {ResultId}", userTestResults.ResultId);
                // Do not fail the whole request if answers fail to persist; return partial info or error as appropriate.
                return Problem(detail: ex.InnerException?.Message ?? ex.Message, statusCode: 500);
            }

            return Ok(new
            {
                status = true,
                message = "Test result saved.",
                resultId = userTestResults.ResultId,
                resultGuid = userTestResults.ResultGuid,
                totalQuestions = totalQuestion,
                Totalattempted,
                correctAnswers,
                wrongAnswers,
                notAttempted,
                score,
                state = "userprofile"
            });
        }
        // Accept DTO in body, make async so we can await DB calls and compute report.
        // Route is relative: "api/ExamSubmit/performance-report"

    }
}
