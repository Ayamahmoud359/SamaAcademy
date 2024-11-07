using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Academy.Models
{
    public class Subscription
    {
        public int SubscriptionId { get; set; }
        [DataType(DataType.Date)]
        public string? StartDate { get; set; }
        [DataType(DataType.Date)]
        public string? EndDate { get; set; }
        //Trainee Id
        [ForeignKey("Trainee")]
        public int TraineeId { get; set; }
        public Trainee Trainee { get; set; }
        //Department Id
        [ForeignKey("Department")]
        public int ? DepartmentId { get; set; }
        public Department ?Department { get; set; }
        //Trainer Id
        [ForeignKey("Trainer")]
        public int? TrainerId { get; set; }
        public Trainer? Trainer { get; set; }

        [ForeignKey("Category")]
        public int? CategoryId { get; set; }
        public Category? Category { get; set; }
        // Logs absences for this subscription
        public ICollection<Absence> Absences { get; set; }=new List<Absence>();

        // Logs exams for this subscription
        public ICollection<Exam> Exams { get; set; }= new List<Exam>();



    }
}
