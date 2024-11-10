using Academy.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Academy.DTO
{
    public class DepartmentVM
    {
        public int DepartmentId { get; set; }
        [Required(ErrorMessage = "Department Name is required")]
        [Display(Name = "Department Name")]
        public string DepartmentName { get; set; }
        [Required(ErrorMessage = "Department Description is required")]
        [Display(Name = "Department Description")]
        public string DepartmentDescription { get; set; }

        public string? Image { get; set; }
        public bool IsActive { get; set; }
        [Required(ErrorMessage = "Branch is required")]
        [Display(Name = "Branch")]
        public int BranchId { get; set; }
      
    }
}
