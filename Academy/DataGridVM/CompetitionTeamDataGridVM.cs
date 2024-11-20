namespace Academy.DataGridVM
{
    public class CompetitionTeamDataGridVM
    {


        public string TeamName { get; set; }
        public string CompetitionDepartmentName { get; set; }


        public string TrainerName { get; set; }

        public string Image { get; set; }

        public int Id { get; set; }

        public int? TrainerId { get; set; }

        public int CompetitionDepartmentId { get; set; }



        public bool IsActive { get; set; }

    }
}
