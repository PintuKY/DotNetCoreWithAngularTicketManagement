using TicketManagement.Server.Models.DTOs;
using TicketManagement.Server.Models.OnlineEducation;

namespace TicketManagement.Server.Repositorys.OnlineEducation
{
    public interface IListQuestions
    {
        // Updated to accept ChapterGuid (from URL) and return questions for that chapter
        Task<List<QuestionDto>> GetListQuestionsAsync(Guid chapterGuid);
    }
}
