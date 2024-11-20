using System.ComponentModel.DataAnnotations;

namespace Academy.DTO
{
    public class CompetitionDepartmentVM
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Competition Department Name is Required")]

        [Display(Name = "Competition Department Name")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Competition Department Description is Required")]

        [Display(Name = "Competition Department Description")]
        public string Description { get; set; } = string.Empty;

        public string? Image { get; set; }
        public bool IsActive { get; set; }

    }
}
