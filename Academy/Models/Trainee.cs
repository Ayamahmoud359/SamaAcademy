﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Academy.Models
{
    public class Trainee
    {
        public int TraineeId { get; set; }
        public string TraineeName { get; set; }
        ///Put the child age in date attribute <summary>
        [DataType(DataType.Date)]
        public string BirthDate { get; set; }
        public string? Image { get; set; }
        public bool IsActive { get; set; }
        ///Parent Id
        [ForeignKey("Parent")]
    
        public int ParentId { get; set; }
        public Parent Parent { get; set; }
         /// List of Champion
          public ICollection<TraineeChampion> ChampionChildren { get; set; } = new List<TraineeChampion>();
        // A trainee can have many subscriptions (to multiple departments and categories)
        public ICollection<Subscription> Subscriptions { get; set; }

        // Absence logs for this trainee
        public ICollection<Absence> Absences { get; set; }

        // Exams taken by this trainee
        public ICollection<Exam> Exams { get; set; }
        public bool IsDeleted { get; set; }
    }
}
