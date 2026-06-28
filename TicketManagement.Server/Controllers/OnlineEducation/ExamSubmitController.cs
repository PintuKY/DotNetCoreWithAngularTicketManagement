using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using TicketManagement.Server.Models.DTOs;
using TicketManagement.Server.Models.OnlineEducation;
using TicketManagement.Server.Objects;
using TicketManagement.Server.Repositorys.OnlineEducation;
using TicketManagement.Server.DBContexts;

namespace TicketManagement.Server.Controllers.OnlineEducation
{
    [ApiController]
    [Route("api/[controller]")]
    public class ExamSubmitController : ControllerBase
    {
        private readonly QuestionsDatas _questionsDatas;
        private readonly AppDatabaseContext _dbContext;
        private readonly ILogger<ExamSubmitController> _logger;

        public ExamSubmitController(QuestionsDatas questionsDatas, AppDatabaseContext dbContext, ILogger<ExamSubmitController> logger)
        {
            _questionsDatas = questionsDatas;
            _dbContext = dbContext;
            _logger = logger;
        }

        [HttpPost("examsubmits")]
        public async Task<IActionResult> ExamSubmite([FromBody] TestSubmissionDto data)
        {
            if (data == null)
            {
                return BadRequest("Request body is required.");
            }

            // Normalize incoming collections / values to avoid NullReferenceException
            var answers = data.Answers ?? new Dictionary<int, int>();
            var chapterId = data.ChapterId;
            var syllabuid = data.SyllabusID;

            // Load the questions for the chapter so we can compute correctness
            var question_of_chapter = await _questionsDatas.GetAllQuestionOfChapterAns(chapterId) ?? new List<QuestionDto>();

            // Prefer client summary if present; otherwise fall back to DB-derived count
            int totalQuestion = data.Summary?.TotalQuestions ?? question_of_chapter.Count;

            // Attempted: prefer explicit answers count or fallback to DTO numeric field
            int attempted = answers.Count > 0 ? answers.Count : (data.QuestionsAttempted > 0 ? data.QuestionsAttempted : 0);
            int notAttempted = Math.Max(0, totalQuestion - attempted);

            int correctAnswers = 0;
            int wrongAnswers = 0;

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

            //var testEntity = await _dbContext.Tests
            //    .AsNoTracking()
            //    .FirstOrDefaultAsync(t => t.TestGuid == syllabuid);

            //if (testEntity == null)
            //{
            //    // If client can provide TestId or TestGuid, you may try to resolve using that instead.
            //    _logger.LogWarning("No test found for ChapterId {ChapterId} while attempting to submit exam", chapterId);
            //    return BadRequest($"No test found for ChapterId {chapterId}.");
            //}

            var userTestResults = new UserTestResults
            {
                ResultGuid = Guid.NewGuid(),
                UserId = 1,                    // replace with authenticated user id
                //TestId = testEntity.Id,        // <- use the Tests.Id, not the ChapterId
                SyllabusId = syllabuid,
                ChapterId = chapterId,
                TotalQuestions = totalQuestion,
                Attempted = attempted,
                CorrectAnswers = correctAnswers,
                WrongAnswers = wrongAnswers,
                NotAttempted = notAttempted,
                Score = score,
                StartTime = data.StartDateTime,
                EndTime = data.EndDateTime,
                CreatedOn = DateTime.Now
            };

            // Persist safely
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

            return Ok(new
            {
                status = true,
                message = "Test result saved.",
                resultId = userTestResults.ResultId,
                resultGuid = userTestResults.ResultGuid,
                totalQuestions = totalQuestion,
                attempted,
                correctAnswers,
                wrongAnswers,
                notAttempted,
                score
            });
        }
        // Accept DTO in body, make async so we can await DB calls and compute report.
        // Route is relative: "api/ExamSubmit/performance-report"
        
    }
}
