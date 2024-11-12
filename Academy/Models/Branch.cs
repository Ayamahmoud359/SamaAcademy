namespace Academy.Models
{
    public class Branch
    {
        public int BranchId { get; set; }
        public string BranchName { get; set; }
        public string BranchAddress { get; set; }
        public string? Image { get; set; }
        public bool IsActive { get; set; }
        public ICollection<Department> Departments { get; set; } = new List<Department>();
        public bool IsDeleted { get; set; }


    }
}
