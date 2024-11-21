using Academy.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace Academy.ViewModels
{
    public class EvaluationDTO
    {
        public int ExamId { get; set; }
        public bool IsDeleted { get; set; }
        public int Score { get; set; }

        public DateOnly? ExamDate { get; set; }
        public string? Review { get; set; }
        public int SubscriptionId { get; set; }
        public Subscription? Subscription { get; set; }
       
        public int? TrainerId { get; set; }
    }
}
