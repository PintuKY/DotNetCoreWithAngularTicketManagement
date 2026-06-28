namespace TicketManagement.Server.Models.OnlineEducation
{
    public class TestAttempts
    {
        public int Id;
        public int TestId { get; set; }
        public int UserId { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public int TotalScore { get; set; }
    }
}
