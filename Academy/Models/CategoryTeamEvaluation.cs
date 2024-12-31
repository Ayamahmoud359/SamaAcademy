using System.ComponentModel.DataAnnotations.Schema;

namespace Academy.Models
{
    public class CategoryTeamEvaluation
    {
        public int CategoryTeamEvaluationId { get; set; }
      
       
        public DateOnly? EvaluationDate  { get; set; }
        public string? EvaluationImage { get; set; }
        public bool IsDeleted { get; set; }


        [ForeignKey("Category")]
        public int CategoryId { get; set; }
        public Category? Category { get; set; }

        //Trainer Id
        [ForeignKey("Trainer")]
        public int? TrainerId { get; set; }
        public Trainer? Trainer { get; set; } 

    }
}
