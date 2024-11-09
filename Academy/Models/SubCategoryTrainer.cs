using System.ComponentModel.DataAnnotations.Schema;

namespace Academy.Models
{
    public class SubCategoryTrainer
    {
        public int Id { get; set; }
        [ForeignKey("SubCategory")]
        public int? SubCategoryId { get; set; }
        public SubCategory? SubCategory { get; set; }
        [ForeignKey("Trainer")]
        public int? TrainerId { get; set; }
        public Trainer? Trainer { get; set; }
        public bool IsDeleted { get; set; }
    }
}
