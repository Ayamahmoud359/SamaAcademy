using Academy.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace Academy.DataGridVM
{
    public class ChampionDataGridVM
    {
      

   

        public int ChampionId { get; set; }
        public string ChampionName { get; set; }

        public string? ChampionDescription { get; set; }


        public DateOnly? ChampionDate { get; set; }

       
    }
}
