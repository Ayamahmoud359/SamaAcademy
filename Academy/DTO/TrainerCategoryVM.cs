using System.ComponentModel.DataAnnotations;

namespace Academy.DTO
{
    public class TrainerCategoryVM
    {

        [Required(ErrorMessage = "Trainer is required")]
        [Display(Name = "Trainer")]
        public int TrainerId { get; set; }

        [Required(ErrorMessage = "Branch is required")]
        [Display(Name = "Branch")]
        public int BranchId { get; set; }

        [Required(ErrorMessage = "Department is required")]
        [Display(Name = "Department")]
        public int DepartmentId { get; set; }

        [Required(ErrorMessage = "Please select at least one category.")]
        public List<int> SelectedCategories { get; set; } = new List<int>();

    }
}
