using Academy.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace Academy.DTO
{
    public class DepartmentVM
    {
        public int DepartmentId { get; set; }
        public string DepartmentName { get; set; }

        public string DepartmentDescription { get; set; }

        public string? Image { get; set; }
        public bool IsActive { get; set; }

        public int BranchId { get; set; }
      
    }
}
