using System.ComponentModel.DataAnnotations.Schema;

namespace Academy.Models
{
    public class TraineeCompetitionTeam
    {
        public int Id { get; set; }
        [ForeignKey("Trainee")]
        public int TraineeId { get; set; }
        public Trainee? Trainee { get; set; }
        [ForeignKey("CompetitionTeam")]
        public int CompetitionTeamId { get; set; }
        public CompetitionTeam? CompetitionTeam { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsActive { get; set; }
    }
}
