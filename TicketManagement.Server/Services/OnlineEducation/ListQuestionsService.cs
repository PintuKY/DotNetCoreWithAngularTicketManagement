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
        public  async Task<List<QuestionDto>> GetListQuestionsAsync()
        {

            //return await _context.Questions.ToListAsync();
            //var data = await _context.Questions.Include(q => q.QuestionOptions).ToListAsync();
            //var question = await _context.Questions.Include(q => q.QuestionOptions).FirstOrDefaultAsync(x => x.Id == id);

            var result = await _context.question.Include(q => q.QuestionOptions)
                           .Select(q => new QuestionDto
                           {
                               Id = q.Id,
                               QuestionText = q.QuestionText,
                               ChapterID = q.ChapterID,
                               Options = q.QuestionOptions
                               .Select(o => new OptionDto
                               {
                                   OptionId = o.OptionId,
                                   OptionText = o.OptionText
                               }).ToList()
                           }).ToListAsync();

            //var result = await _context.syllabus
            //                            .Select(s => new SyllabusDto
            //                            {
            //                                SyllabusID = s.SyllabusID,
            //                                SyllabusName = s.syllabusName,

            //                                Chapters = s.Chapters.Select(c => new ChapterDto
            //                                {
            //                                    ChapterId = c.ChapterId,
            //                                    ChapterName = c.ChapterName,

            //                                    Questions = c.Questions.Select(q => new QuestionDto
            //                                    {
            //                                        Id = q.Id,
            //                                        QuestionText = q.QuestionText,
            //                                        Options = q.QuestionOptions.Select(o => new OptionDto
            //                                        {
            //                                            OptionId = o.OptionId,
            //                                            OptionText = o.OptionText
            //                                        }).ToList()

            //                                    }).ToList()

            //                                }).ToList()
            //                            })
            //                            .ToListAsync();


            return result;
        }

        
    }
}
