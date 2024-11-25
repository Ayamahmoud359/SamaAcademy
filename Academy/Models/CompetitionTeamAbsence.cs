using System.ComponentModel.DataAnnotations.Schema;

namespace Academy.Models
{
    public class CompetitionTeamAbsence
    {
        public int CompetitionTeamAbsenceId { get; set; }
        public bool IsAbsent { get; set; }
        public DateTime? AbsenceDate { get; set; }
        public string? Type { get; set; }
        public int TraineeCompetitionTeamId { get; set; }
        public TraineeCompetitionTeam? TraineeCompetitionTeam { get; set; }
        public bool IsDeleted { get; set; }
        //Trainer Id
        [ForeignKey("Trainer")]
        public int? TrainerId { get; set; }
        public Trainer? Trainer { get; set; } 
            

    }
}
