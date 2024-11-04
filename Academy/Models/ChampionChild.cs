using System.ComponentModel.DataAnnotations.Schema;

namespace Academy.Models
{
    public class ChampionChild
    {
        public int ChampionChildId { get; set; }
        [ForeignKey("Champions")]
        public int ChampionsId { get; set; }
        public Champions Champions { get; set; }
        [ForeignKey("Child")]
        public int ChildId { get; set; }
        public Child Child { get; set; }
    }
}
