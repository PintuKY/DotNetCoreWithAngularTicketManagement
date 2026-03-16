using TicketManagement.Server.Models.DTOs;
using TicketManagement.Server.Models.OnlineEducation;

namespace TicketManagement.Server.Repositorys.OnlineEducation
{
    public interface IListQuestions
    {
        Task<List<QuestionDto>> GetListQuestionsAsync();
    }
}
