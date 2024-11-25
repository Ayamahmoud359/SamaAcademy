using Academy.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace Academy.ViewModels
{
    public class AddCompetitionTeamEvaluationDTO
    {
       
        public int Score { get; set; }

        public DateOnly? EvaluationDate { get; set; }
        public string? Review { get; set; }
        public int TraineeCompetitionTeamId { get; set; }
     
        public int TrainerId { get; set; }
    }
}
