using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Academy.Models
{
    public class Category
    {
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
      
        public string? CategoryDescription{ get; set; }
        public string? image { get; set; }

        public bool IsDeleted { get; set; }
        public bool IsActive { get; set; }
        //Department Id
        [ForeignKey("Department")]
        public int DepartmentId { get; set; }
        public Department? Department { get; set; } 
        ///list of SubCategory
       // public ICollection<SubCategory> SubCategories { get; set; } = new List<SubCategory>();
        public ICollection<TrainerCategories> TrainerCategories { get; set; } = new List<TrainerCategories>();
        // A category can have many trainees (through subscriptions)
        public ICollection<Subscription> Subscriptions { get; set; } = new List<Subscription>();
        public ICollection<CategoryTeamEvaluation> CategoryTeamEvaluations { get; set; } = new List<CategoryTeamEvaluation>();
       
    }
}
