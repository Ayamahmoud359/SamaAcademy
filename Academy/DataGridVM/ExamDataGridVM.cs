using Academy.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Academy.DataGridVM
{
    public class ExamDataGridVM
    {
        public string BranchName { get; set; }
        public string DepartmentName { get; set; }
        public string CategoryName { get; set; }

        public string TrainerName { get; set; }
      

        public int SubscriptionId { get; set; }

        public int? TrainerId { get; set; }
        public DateOnly? StartDate { get; set; }

        public DateOnly? EndDate { get; set; }
        public int ExamId { get; set; }
  
        public int Score { get; set; }
      
        public DateOnly? ExamDate { get; set; }
        public string? Review { get; set; }
 
    }
}
