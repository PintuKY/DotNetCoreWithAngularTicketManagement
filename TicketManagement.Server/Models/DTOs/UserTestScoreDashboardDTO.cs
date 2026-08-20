namespace TicketManagement.Server.Models.DTOs
{
    public class UserTestScoreDashboardDTO
    {
        // Current user result
        public int TotalScore { get; set; }
        public int Score { get; set; }

        public decimal Accuracy { get; set; }

        // Rank
        public int Rank { get; set; }
        public int TotalUsers { get; set; }

        // Example: 0.67%
        public decimal Percentage { get; set; }

        // Example: 21.43%
        public decimal Percentile { get; set; }

        public int Attempted { get; set; }
        public int CorrectAnswers { get; set; }
        public int WrongAnswers { get; set; }
        public int NotAttempted { get; set; }

        public int TotalQuestions { get; set; }

        public TimeSpan TotalTime { get; set; }

        public decimal AverageTimePerQuestionSeconds { get; set; }

        // Current user
        public UserTestResultsDTO? CurrentUserResult { get; set; }

        // Topper
        public UserTestResultsDTO? TopperResult { get; set; }

        // All users leaderboard
        public List<UserTestResultsDTO> Leaderboard { get; set; } = new List<UserTestResultsDTO>();
    }
}