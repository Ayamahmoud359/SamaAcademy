using System.ComponentModel.DataAnnotations.Schema;

namespace Academy.Models
{
    public class SubCategory
    {
        public int SubCategoryId { get; set; }
        public string SubCategoryNameEN { get; set; }
        public string SubCategoryNameAR { get; set; }
        public string SubCategoryDescriptionEN { get; set; }
        public string SubCategoryDescriptionAR { get; set; }
        public bool IsActive { get; set; }
        //Category Id
        [ForeignKey("Category")]
        public int CategoryId { get; set; }
        public Category Category { get; set; }
        public bool IsDeleted { get; set; }
        public ICollection<SubCategoryTrainer> SubCategoryTrainers { get; set; } = new List<SubCategoryTrainer>();

    }
}
