using Microsoft.EntityFrameworkCore;
using TicketManagement.Server.DBContexts;
using TicketManagement.Server.Models.DTOs;
using TicketManagement.Server.Models.OnlineEducation;

namespace TicketManagement.Server.Objects
{
    
    public class QuestionsDatas
    {
        private readonly AppDatabaseContext _context;
        private List<Question> questionbychapter = new List<Question>();
        public QuestionsDatas(AppDatabaseContext context)
        {
            _context = context;
        }
        public async Task<List<Question>> GetAllQuestionOfChapter(int chapterId)
        {
            return await _context.question
                .Where(q => q.ChapterID == chapterId)
                .ToListAsync();
        }
        public async Task<List<QuestionDto>> GetAllQuestionOfChapterAns(int chapterId)
        {
            return await _context.question
                .Where(q => q.ChapterID == chapterId)
                .Select(q => new QuestionDto
                {
                    Id = q.Id,
                    Options = q.QuestionOptions
                        .Select(o => new OptionDto
                        {
                            OptionId = o.OptionId,
                            IsCorrect = o.IsCorrect
                        }).ToList()
                })
                .ToListAsync();
        }
        public async Task<List<QuestionDto>> GetAllQuestionOfChapterAnsForReport()
        {
            return await _context.question
                .Select(q => new QuestionDto
                {
                    Id = q.Id,
                    Options = q.QuestionOptions
                        .Select(o => new OptionDto
                        {
                            OptionId = o.OptionId,
                            IsCorrect = o.IsCorrect
                        }).ToList()
                })
                .ToListAsync();
        }
    }
}
