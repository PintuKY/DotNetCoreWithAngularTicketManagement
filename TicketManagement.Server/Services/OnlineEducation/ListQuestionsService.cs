using Microsoft.EntityFrameworkCore;
using TicketManagement.Server.DBContexts;
using TicketManagement.Server.Models.DTOs;
using TicketManagement.Server.Models.OnlineEducation;
using TicketManagement.Server.Repositorys.OnlineEducation;

namespace TicketManagement.Server.Services.OnlineEducation
{
    public class ListQuestionsService : IListQuestions
    {
        private readonly AppDatabaseContext _context;
       
        public ListQuestionsService(AppDatabaseContext context)
        {
            _context = context;
        }

        public async Task<List<QuestionDto>> GetListQuestionsAsync(Guid chapterGuid)
        {
            // Find ChapterId from ChapterGuid
            var chapterId = await _context.chapters
                .AsNoTracking()
                .Where(c => c.ChapterGuid == chapterGuid)
                .Select(c => (int?)c.ChapterId)
                .FirstOrDefaultAsync();

            if (chapterId == null)
            {
                return new List<QuestionDto>();
            }

            var result = await _context.question
                .AsNoTracking()
                .Where(q => q.ChapterID == chapterId.Value)
                .Include(q => q.QuestionOptions)
                .Select(q => new QuestionDto
                {
                    Id = q.Id,
                    QuestionText = q.QuestionText,
                    ChapterID = q.ChapterID,
                    QuestionGuid = q.QuestionGuid,
                    Options = q.QuestionOptions
                        .Select(o => new OptionDto
                        {
                            OptionId = o.OptionId,
                            OptionText = o.OptionText
                        }).ToList()
                })
                .ToListAsync();

            return result;
        }
    }
}
