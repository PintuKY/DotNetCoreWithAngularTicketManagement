using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TicketManagement.Server.Models.OnlineEducation
{
    /*
     * 
     * Solution 1 (Best) — Map Table Name Using Attribute and used in service or controller
     * //used for tablle miishmatch, [first method for if table name or class name miss match]
     * 
     */
    //[Table("Questions")] 
    public class ListQuestionsModel
    {
        [Key]
        public int Id { get; set; }
        public Guid? QuestionGuid { get; set;}
        public int? TestId {get;set;}
        public string? QuestionText {get; set;}
        public char CorrectOption {get; set;}
        public string? Syllabus {get; set;}
        public string? Chapter {get; set;}
        public string? Module {get; set;}
        public string? Topic {get; set;}
        public string? Resources {get; set;}
        public string? DifficultyLevel {get; set;}
        public int? CreatedByUserId {get; set;}
        public int? Marks {get; set;}
        public bool IsActive {get; set;}
        public DateTime? CreatedOn {get; set;}
        public string  Status {get; set;}
        // public List<QuestionOption> QuestionOptions { get; set; }
        public List<QuestionOption1> QuestionOptions { get; set; }   = new List<QuestionOption1>();
    }
    public class QuestionOption1
    {

        public int OptionId { get; set; }
        public string? OptionText { get; set; }
        public bool IsCorrect { get; set; }

        public int QuestionId { get; set; }
        public ListQuestionsModel Question { get; set; }

    }
}
