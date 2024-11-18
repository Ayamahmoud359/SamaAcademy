using Academy.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace Academy.DataGridVM
{
    public class TraineeChampionDataGridVM
    {

        public string BranchName { get; set; }
        public string DepartmentName { get; set; }
        public string ChampionName { get; set; }

        public string TraineeName { get; set; }


        public bool IsActive { get; set; }

        public int TraineeChampionId { get; set; }

        public int ChampionId { get; set; }


        public int TraineeId { get; set; }

   
    }
}
