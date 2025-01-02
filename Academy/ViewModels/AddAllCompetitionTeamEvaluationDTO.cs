

namespace Academy.ViewModels
{
    public class AddAllCompetitionTeamEvaluationDTO
    {
       
      

        public DateOnly? EvaluationDate { get; set; }
       // public IFormFile Image { get; set; } = null!;
        public int CompetitionTeamId { get; set; }
      
        public int TrainerId { get; set; }
    }
}
