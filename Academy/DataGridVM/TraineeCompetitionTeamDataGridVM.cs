using Academy.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace Academy.DataGridVM
{
    public class TraineeCompetitionTeamDataGridVM
    {
      
        public string Trainer { get; set; }
        public string CompetitionDepartment { get; set; }
        public string CompetitionTeam { get; set; }

        public string TraineeName { get; set; }
        public bool IsActive { get; set; }

        public int Id { get; set; }

        public int TraineeId { get; set; }
    
        public int CompetitionTeamId { get; set; }

    }
}
