using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Academy.Models
{
    public class Champion
    {
        public int ChampionId { get; set; }
        public string ChampionName { get; set; }
   
        public string? ChampionDescription { get; set; }
     

        public DateOnly? ChampionDate { get; set; }
        public bool IsActive { get; set; }
        public int? ChampionScore { get; set; }
        ///Department Id
        [ForeignKey("Department")]
        public int DepartmentId { get; set; }
        public Department? Department { get; set; }
        ///list of Child
        public ICollection<TraineeChampion> TraineeChampions { get; set; } = new List<TraineeChampion>();
        public bool IsDeleted { get; set; }
    }
}
