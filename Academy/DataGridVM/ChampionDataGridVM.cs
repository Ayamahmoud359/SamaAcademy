using Academy.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace Academy.DataGridVM
{
    public class ChampionDataGridVM
    {
      
        public string DepartmentName { set; get; }
        public string BranchName { set; get; }
        public bool IsActive { set; get; }
   

        public int ChampionId { get; set; }
        public string ChampionName { get; set; }

        public string? ChampionDescription { get; set; }


        public DateOnly? ChampionDate { get; set; }

        public int? ChampionScore { get; set; }
       
    }
}
