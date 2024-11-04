using System.ComponentModel.DataAnnotations.Schema;

namespace Academy.Models
{
    public class Department
    {
        public int DepartmentId { get; set; }
        public string DepartmentNameEN { get; set; }
        public string DepartmentNameAR { get; set; }
        public string DepartmentDescriptionEN { get; set; }
        public string DepartmentDescriptionAR { get; set; }
        public string? Image { get; set; }
        public bool IsActive { get; set; }
        [ForeignKey("Branch")]
        public int BranchId { get; set; }
        public Branch Branch { get; set; }
        ///list of Category
        public ICollection<Category> Categories { get; set; } = new List<Category>();
    }
}
