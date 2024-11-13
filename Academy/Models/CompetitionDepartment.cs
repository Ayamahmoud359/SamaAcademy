namespace Academy.Models
{
    public class CompetitionDepartment
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;

        public string? Image { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
        public ICollection<CompetitionTeam> CompetitionTeams { get; set; } = new List<CompetitionTeam>();
    }
}
