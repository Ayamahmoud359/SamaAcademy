using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Academy.Models
{
    public class Subscriptions
    {
        public int SubscriptionsId { get; set; }
        [DataType(DataType.Date)]
        public string? SubscriptionDate { get; set; }
        //Child Id
        [ForeignKey("Child")]
        public int ChildId { get; set; }
        public Child Child { get; set; }
        //Department Id
        [ForeignKey("Department")]
        public int? DepartmentId { get; set; }
        public Department? Department { get; set; }
        //Trainer Id
        [ForeignKey("Trainer")]
        public int TrainerId { get; set; }
        public Trainer Trainer { get; set; }

        [ForeignKey("Category")]
        public int? CategoryId { get; set; }
        public Category? Category { get; set; }
        ///SubCategory Id

        [ForeignKey("SubCategory")]
        public int? SubCategoryId { get; set; }
        public SubCategory? SubCategory { get; set; }

    }
}
