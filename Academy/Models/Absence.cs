using System.ComponentModel.DataAnnotations.Schema;

namespace Academy.Models
{
    public class Absence
    {
        public int AbsenceId { get; set; }
        public bool IsAbsent { get; set; }
        public DateTime? AbsenceDate { get; set; }
        public string? Type { get; set; }
        public int SubscriptionId { get; set; }
        public Subscription Subscription { get; set; } = new Subscription();
        public bool IsDeleted { get; set; }
        //Trainer Id
        [ForeignKey("Trainer")]
        public int TrainerId { get; set; }
        public Trainer? Trainer { get; set; } 
            

    }
}
