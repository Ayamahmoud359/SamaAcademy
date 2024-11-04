namespace Academy.Models
{
    public class Branch
    {
        public int BranchId { get; set; }
        public string BranchNameEN { get; set; }
        public string BranchNameAR { get; set; }
        public string BranchAddress { get; set; }
        public string? Image { get; set; }
        public bool IsActive { get; set; }

        ////list of trainers
        public ICollection<Trainer> Trainers { get; set; } = new List<Trainer>();
        ////list of departments
        public ICollection<Department> Departments { get; set; } = new List<Department>();
        ////list of parents
        
        public ICollection<Parent> Parents { get; set; } = new List<Parent>();



    }
}
