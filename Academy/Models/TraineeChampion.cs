using System.ComponentModel.DataAnnotations.Schema;

namespace Academy.Models
{
    public class TraineeChampion
    {
        public int TraineeChampionId { get; set; }
        [ForeignKey("Champion")]
        public int ChampionId { get; set; }
        public Champion Champion { get; set; }
        [ForeignKey("Trainee")]
        public int TraineeId { get; set; }
        public Trainee Trainee{ get; set; }
    }
}
