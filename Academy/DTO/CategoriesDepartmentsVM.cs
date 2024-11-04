using Academy.Models;

namespace Academy.DTO
{
    public class CategoriesDepartmentsVM
    {
        public int DepartmentId { get; set; }
        public ICollection<Category> categories { get; set; } = new List<Category>();
    }
}
