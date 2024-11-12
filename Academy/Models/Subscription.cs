using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Academy.Models
{
    public class Subscription
    {

        public int SubscriptionId { get; set; }
      
        public DateOnly? StartDate { get; set; }
       
        public DateOnly? EndDate { get; set; }

        public int? Branch { get; set; }

        public int? Department { get; set; }
        //Trainee Id
        [ForeignKey("Trainee")]
        public int TraineeId { get; set; }
        public Trainee? Trainee { get; set; } 

        [ForeignKey("Category")]
        public int CategoryId { get; set; }
        public Category? Category { get; set; } 

        // Logs absences for this subscription
        public ICollection<Absence> Absences { get; set; }=new List<Absence>();

        // Logs exams for this subscription
        public ICollection<Exam> Exams { get; set; }= new List<Exam>();
        public bool IsDeleted { get; set; }
        public bool IsActive { get; set; }


    }
}
