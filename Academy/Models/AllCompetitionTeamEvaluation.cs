using System.ComponentModel.DataAnnotations.Schema;

namespace Academy.Models
{
    public class AllCompetitionTeamEvaluation
    {
        public int AllCompetitionTeamEvaluationId { get; set; }
      
       
        public DateOnly? EvaluationDate  { get; set; }
        public string? EvaluationImage { get; set; }
        public bool IsDeleted { get; set; }


        [ForeignKey("CompetitionTeam")]
        public int CompetitionTeamId { get; set; }
        public CompetitionTeam? CompetitionTeam { get; set; }

        //Trainer Id
        [ForeignKey("Trainer")]
        public int? TrainerId { get; set; }
        public Trainer? Trainer { get; set; } 

    }
}
