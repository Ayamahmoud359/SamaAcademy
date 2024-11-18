using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Academy.Models
{
    public class Trainee
    {
        public int TraineeId { get; set; }
        public string? TraineeEmail { get; set; }
        public string UserName { get; set; }
        public string TraineePhone { get; set; }
        public string? TraineeAddress { get; set; }
        public string TraineeName { get; set; }
        public DateOnly? BirthDate { get; set; }
        public string? Image { get; set; }
        public string? Nationality { get; set; }
        public string? ResidencyNumber { get; set; }
        public bool IsActive { get; set; }
        ///Parent Id
        [ForeignKey("Parent")]
    
        public int? ParentId { get; set; }
        public Parent? Parent { get; set; }
         /// List of Champion
          public ICollection<TraineeChampion> TraineeChampions{ get; set; } = new List<TraineeChampion>();
        // A trainee can have many subscriptions (to multiple departments and categories)
        public ICollection<Subscription> Subscriptions { get; set; }= new List<Subscription> 
            ();
      
        public bool IsDeleted { get; set; }
     
    }
}
