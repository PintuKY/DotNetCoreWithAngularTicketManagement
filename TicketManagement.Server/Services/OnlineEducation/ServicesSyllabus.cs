using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using TicketManagement.Server.DBContexts;
using TicketManagement.Server.Models.DTOs;
using TicketManagement.Server.Models.OnlineEducation;
using TicketManagement.Server.Repositorys.OnlineEducation;

namespace TicketManagement.Server.Services.OnlineEducation
{
    public class ServicesSyllabus:ISyllabus
    {
        private readonly AppDatabaseContext _context;
        public ServicesSyllabus(AppDatabaseContext context)
        {
            _context = context;
        }
        public async Task<List<SyllabusDto>> GetAsyncSyllabus()
        {
            //var result = await _context.syllabus.Include(s => s.Chapters).ToListAsync();
            //return await _context.syllabus.ToListAsync();
            var result = await _context.syllabus
                                       .AsNoTracking()
                                       .Select(s => new SyllabusDto
                                       {
                                           SyllabusID = s.SyllabusID,
                                           SyllabusGuid=s.syllabusGuid,
                                           SyllabusName = s.syllabusName,
                                           Chapters = s.Chapters.Select(c => new ChapterDto
                                           {
                                               ChapterId = c.ChapterId,
                                               ChapterGuid=c.ChapterGuid,
                                               ChapterName = c.ChapterName,
                                               Questions = c.Questions.Select(q => new QuestionDto
                                               {
                                                   Id = q.Id,
                                                   ChapterID=q.ChapterID,
                                                   QuestionGuid=q.QuestionGuid,
                                                   QuestionText = q.QuestionText,
                                                   Options = q.QuestionOptions.Select(o => new OptionDto
                                                   {
                                                       OptionId = o.OptionId,
                                                       OptionText = o.OptionText
                                                   }).ToList()
                                               }).ToList()
                                           }).ToList()
                                       })
                                       .ToListAsync();
            return result;
        }
    }
}
