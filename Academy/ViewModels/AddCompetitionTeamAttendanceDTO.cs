using Academy.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace Academy.ViewModels
{
    public class AddCompetitionTeamAttendanceDTO
    {
       
        public bool IsAbsent { get; set; }
        public DateTime? Date { get; set; }
        public string? Type { get; set; }
        public int TraineeCompetitionTeamId { get; set; }
        public int? TrainerId { get; set; }
    }
}
