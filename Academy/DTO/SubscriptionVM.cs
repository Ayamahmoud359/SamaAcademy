using System.ComponentModel.DataAnnotations;

namespace Academy.DTO
{
    public class SubscriptionVM
    {
        [DataType(DataType.Date)]
        [Required(ErrorMessage = "Supsciption StartDate is required")]
        [Display(Name = "StartDate")]
        public string StartDate { get; set; }
        [DataType(DataType.Date)]
        [Required(ErrorMessage = "Supsciption EndDate is required")]
        [Display(Name = "EndDate")]
        public string EndDate { get; set; }

        //[Required(ErrorMessage = "Department is required")]
        [Display(Name = "Department")]
        public int? DepartmentId { get; set; }

        //[Required(ErrorMessage = "Category is required")]
        [Display(Name = "Category")]
        public int? CategoryId { get; set; }

    }
}
