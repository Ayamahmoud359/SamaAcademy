using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Academy.Models
{
    public class Exam
    {
        public int ExamId { get; set; }
        public bool IsDeleted { get; set; }
        public int Score { get; set; }
        [DataType(DataType.Date)]
        public DateOnly? ExamDate  { get; set; }
        public string? Review { get; set; }
        public int SubscriptionId { get; set; }
        public Subscription Subscription { get; set; } = new Subscription();
        //Trainer Id
        [ForeignKey("Trainer")]
        public int? TrainerId { get; set; }
        public Trainer? Trainer { get; set; } 

    }
}
