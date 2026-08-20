namespace TicketManagement.Server.Models.DTOs
{
    public class UserTestScoreResponseDTO
    {       
        public int TotalUsersAttempted { get; set; }

        public UserTestResultsDTO? CurrentUserResult { get; set; }

        public List<UserTestResultsDTO> UserRankingList { get; set; } = new List<UserTestResultsDTO>();        
    }
}
