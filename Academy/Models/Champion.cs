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
        public bool IsDeleted { get; set; }
        public string? Image { get; set; }
    }
}
