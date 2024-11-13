namespace Academy.Models
{
    public class CompetitionTeam
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Image { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
        public int CompetitionDepartmentId { get; set; }
        public CompetitionDepartment CompetitionDepartment { get; set; } = null!; 
        public int TrainerId { get; set; }
        public Trainer Trainer { get; set; } = null!;
        public ICollection<Trainee> Trainees { get; set; } = new List<Trainee>();
    }
}
