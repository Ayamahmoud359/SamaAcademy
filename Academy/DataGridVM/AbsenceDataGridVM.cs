using Academy.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace Academy.DataGridVM
{
    public class AbsenceDataGridVM
    {

        public string BranchName { get; set; }
        public string DepartmentName { get; set; }
        public string CategoryName { get; set; }
        public string TraineeName { get; set; }
        public string TrainerName { get; set; }
        public int AbsenceId { get; set; }
        public bool IsAbsent { get; set; }
        public DateTime? AbsenceDate { get; set; }
        public string? Type { get; set; }
        public int SubscriptionId { get; set; }

        public int? TrainerId { get; set; }
        public DateOnly? StartDate { get; set; }

        public DateOnly? EndDate { get; set; }

    }
}
