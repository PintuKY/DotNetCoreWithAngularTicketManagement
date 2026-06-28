using System.ComponentModel.DataAnnotations.Schema;

namespace TicketManagement.Server.Models.OnlineEducation
{
    [Table("Tests")]
    public class Test
    {
      public int Id { get; set; }
      public Guid TestGuid { get; set; }
      public string? TestName { get; set; }
      public string? Description {  get; set; }

      // Changed to match DB column type (int)
      public int TotalMarks { get; set; }

      // Changed to match DB column type (int)
      public int DurationMinutes { get; set; }

      // Changed to match DB column type (decimal(10,2))
      public decimal Price { get; set; }

      public int CreatedByUserId {  get; set; }
      public bool IsActive {  get; set; }
      public DateTime CreatedOn {  get; set; }
    
      public int TotalQuestions { get; set; }
      public bool IsPaid { get; set; } = true;
    }
}
