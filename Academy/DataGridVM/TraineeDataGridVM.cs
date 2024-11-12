namespace Academy.DataGridVM
{
    public class TraineeDataGridVM
    {

          public string  TraineeName { set; get; }
           public string   TraineeAddress { set; get; }
            public string   ParentName { set; get; }
           public string   TraineeEmail { set; get; }
            public string  TraineePhone { set; get; }
            public string   Nationality { set; get; }
            public  DateOnly?    BirthDate { set; get; }
             public string   ResidencyNumber { set; get; }
              public int   TraineeId { set; get; }
        public bool IsActive { set; get; }
    }
}
