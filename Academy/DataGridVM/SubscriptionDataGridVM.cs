using Academy.Models;

namespace Academy.DataGridVM
{
    public class SubscriptionDataGridVM
    {
        public int SubscriptionId { set; get; }
        public DateOnly? StartDate { set; get; }
        public DateOnly? EndDate { set; get; }
        public int? Branch { set; get; }
        public int? Department { set; get; }
        public string TraineeName { set; get; }
        public string CategoryName { set; get; }
        public string DepartmentName { set; get; }
        public string BranchName { set; get; }
        public bool IsActive { set; get; }
    }
}
