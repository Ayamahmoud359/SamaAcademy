using Academy.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace Academy.DataGridVM
{
    public class TrainerCategoryDataGridVM
    {
        public int TrainerCategoriesId { get; set; }
        public string BranchName { get; set; }
        public string DepartmentName { get; set; }
        public string CategoryName{ get; set; }
        
       public string TrainerName { get; set; }

     
        public bool IsActive { get; set; }
    }
}
