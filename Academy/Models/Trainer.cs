using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Academy.Models
{
    public class Trainer
    {
        public int TrainerId { get; set; }
  
        public string TrainerName { get; set; }

       
        public string TrainerEmail { get; set; }
    
        public string TrainerPhone { get; set; }
       
        public string TrainerAddress { get; set; }
        public string? Image { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime SubscriptionDate { get; set; }

        //[ForeignKey("Branch")]
   
        //public int BranchId { get; set; }
        //public Branch Branch { get; set; }
        ///Department Id
        [ForeignKey("Department")]
        
        public int DepartmentId { get; set; }
        public Department Department { get; set; }
        public ICollection<Absence> Absences { get; set; }= new List<Absence>();
        public ICollection<Exam> Exams { get; set; }=new List<Exam>();
        public ICollection<TrainerCategories> CategoryTrainers { get; set; }= new List<TrainerCategories>();
       // public ICollection<SubCategoryTrainer> subCategoryTrainers { get; set; } = new List<SubCategoryTrainer>();
    }
}
