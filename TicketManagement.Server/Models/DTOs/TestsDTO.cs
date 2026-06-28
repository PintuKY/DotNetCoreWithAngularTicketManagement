namespace TicketManagement.Server.Models.DTOs
{
    public class TestsDTO
    {
        public int Id { get; set; }
        public Guid TestGuid { get; set; }
        public string? TestName { get; set; }
        public string? Description { get; set; }        
        public decimal Price { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedOn { get; set; }
        public int TotalSyllabus { get; set; }
        public bool IsPaid { get; set; }
    }
}
