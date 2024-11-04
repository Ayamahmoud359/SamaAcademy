using System.ComponentModel.DataAnnotations.Schema;

namespace Academy.Models
{
    public class CategoryTrainers
    {
        public int CategoryTrainersId { get; set; }
        [ForeignKey("Category")]
        public int CategoryId { get; set; }
        public Category Category { get; set; }
        [ForeignKey("Trainer")]
        public int TrainerId { get; set; }
        public Trainer Trainer { get; set; }
    }
}
