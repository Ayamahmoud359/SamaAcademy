using System.ComponentModel.DataAnnotations;

namespace Academy.DTO
{
    public class SubscriptionVM
    {

        [Required(ErrorMessage = "Trainee is required")]
        [Display(Name = "Trainee")]
        public int TraineeId { get; set; }

        [Required(ErrorMessage = "Branch is required")]
        [Display(Name = "Branch")]
        public int BranchId { get; set; }

        
        [Required(ErrorMessage = "Supsciption StartDate is required")]
        [Display(Name = "StartDate")]
        public DateOnly? StartDate { get; set; }


        [Required(ErrorMessage = "Supsciption EndDate is required")]
        [Display(Name = "EndDate")]
        public DateOnly? EndDate { get; set; }

        [Required(ErrorMessage = "Department is required")]
        [Display(Name = "Department")]
        public int DepartmentId { get; set; }

        [Required(ErrorMessage = "Category is required")]
        [Display(Name = "Category")]
        public int CategoryId { get; set; }

    }
}
