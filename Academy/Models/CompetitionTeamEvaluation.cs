using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Academy.Models
{
    public class CompetitionTeamEvaluation
    {
        public int CompetitionTeamEvaluationId { get; set; }
        public bool IsDeleted { get; set; }
        public int Score { get; set; }
       
        public DateOnly? Date  { get; set; }
        public string? Review { get; set; }
        public int TraineeCompetitionTeamId { get; set; }
        public TraineeCompetitionTeam? TraineeCompetitionTeam { get; set; } 
        //Trainer Id
        [ForeignKey("Trainer")]
        public int? TrainerId { get; set; }
        public Trainer? Trainer { get; set; } 

    }
}
