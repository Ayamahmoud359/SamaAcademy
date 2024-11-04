using System.ComponentModel.DataAnnotations.Schema;

namespace Academy.Models
{
    public class Category
    {
        public int CategoryId { get; set; }
        public string CategoryNameEN { get; set; }
        public string CategoryNameAR { get; set; }
        public string CategoryDescriptionEN { get; set; }
        public string CategoryDescriptionAR { get; set; }
        public bool? IsActive { get; set; }
        //Department Id
        [ForeignKey("Department")]
        public int DepartmentId { get; set; }
        public Department Department { get; set; }
        ///list of SubCategory
        public ICollection<SubCategory> SubCategories { get; set; } = new List<SubCategory>();
        public ICollection<CategoryTrainers> CategoryTrainers { get; set; } = new List<CategoryTrainers>();
    }
}
