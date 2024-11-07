using System.ComponentModel.DataAnnotations.Schema;

namespace Academy.Models
{
    public class TrainerCategories
    {
        public int TrainerCategoriesId { get; set; }
        [ForeignKey("Category")]
        public int CategoryId { get; set; }
        public Category Category { get; set; }
        [ForeignKey("Trainer")]
        public int TrainerId { get; set; }
        public Trainer Trainer { get; set; }
    }
}
