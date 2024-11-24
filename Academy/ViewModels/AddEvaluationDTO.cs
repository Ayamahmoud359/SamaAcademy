using Academy.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace Academy.ViewModels
{
    public class AddEvaluationDTO
    {
       
        public int Score { get; set; }

        public DateOnly? EvaluationDate { get; set; }
        public string? Review { get; set; }
        public int SubscriptionId { get; set; }
      
        public int TrainerId { get; set; }
    }
}
