namespace Academy.DataGridVM
{
    public class TrainerDataGridVM
    {
         public string  TrainerName { set; get; }
          public int TrainerId { set; get; }
        public string TrainerPhone { set; get; }
        public DateOnly?   HiringDate  { set; get; }
        public string TrainerAddress { set; get; }
        public string           TrainerEmail { set; get; }
         public string        CurrentBranchName { set; get; }
         public string      CurrentDepartmentName { set; get; }
           public bool     IsActive { set; get; }
    }
}
