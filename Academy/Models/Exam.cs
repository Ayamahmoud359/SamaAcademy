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
        public string? ScoreDate  { get; set; }

        public int SubscriptionId { get; set; }
        public Subscription Subscription { get; set; }

    }
}
