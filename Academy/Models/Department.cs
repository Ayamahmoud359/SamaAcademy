using System.ComponentModel.DataAnnotations.Schema;

namespace Academy.Models
{
    public class Department
    {
        public int DepartmentId { get; set; }
        public string DepartmentName { get; set; }
  
        public string? DepartmentDescription { get; set; }

        public string? Image { get; set; }
        public bool IsActive { get; set; }
        [ForeignKey("Branch")]
        public int BranchId { get; set; }
     
        public Branch? Branch { get; set; } 
        ///list of Category
        public ICollection<Category> Categories { get; set; } = new List<Category>();
      
        public bool IsDeleted { get; set; }
    }
}
