using Academy.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Academy.DTO
{
    public class CategoryVM
    {
        public int CategoryId { get; set; }
        [Required(ErrorMessage = "Category Name is Required")]

        [Display(Name = "Category Name")]
        public string CategoryName { get; set; }
        [Required(ErrorMessage = "Category Description is Required")]

        [Display(Name = "Category Description")]
        public string? CategoryDescription { get; set; }

        public bool IsActive { get; set; }
        [Required(ErrorMessage = "Department is Required")]

        [Display(Name = "Department")]
        public int DepartmentId { get; set; }
      
    }
}
