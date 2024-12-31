

namespace Academy.ViewModels
{
    public class AddCategoryTeamEvaluationDTO
    {
       
      

        public DateOnly? EvaluationDate { get; set; }
       // public IFormFile Image { get; set; } = null!;
        public int CategoryId { get; set; }
      
        public int TrainerId { get; set; }
    }
}
