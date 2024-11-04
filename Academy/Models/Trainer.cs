using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Academy.Models
{
    public class Trainer
    {
        public int TrainerId { get; set; }
        [Required(ErrorMessage = "Trainer Name is required")]
        public string TrainerName { get; set; }

        [Required(ErrorMessage = "Trainer Email is required")]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string? TrainerEmail { get; set; }
        [Required(ErrorMessage = "Trainer Phone is required")]
        public string TrainerPhone { get; set; }
        [Required(ErrorMessage = "Trainer Address is required")]
        public string TrainerAddress { get; set; }
        public string? Image { get; set; }
        public bool IsActive { get; set; }

        [ForeignKey("Branch")]
        [Required(ErrorMessage = "Branch Id is required")]
        public int BranchId { get; set; }
        public Branch Branch { get; set; }
        ///Department Id
        [ForeignKey("Department")]
        [Required(ErrorMessage = "Department Id is required")]
        public int DepartmentId { get; set; }
        public Department Department { get; set; }
        public ICollection<CategoryTrainers> CategoryTrainers { get; set; }= new List<CategoryTrainers>();
        public ICollection<SubCategoryTrainer> subCategoryTrainers { get; set; } = new List<SubCategoryTrainer>();
    }
}
