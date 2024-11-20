using Academy.Models;
using System.ComponentModel.DataAnnotations;

namespace Academy.DTO
{
    public class CompetitionTeamVM
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Competition Team Name is Required")]

        [Display(Name = "Competition Team Name")]
        public string Name { get; set; } 
        public string? Image { get; set; }
        public bool IsActive { get; set; }
        [Required(ErrorMessage = "Competition Department  is Required")]

        [Display(Name = "Competition Department")]
        public int CompetitionDepartmentId { get; set; }
        [Required(ErrorMessage = "Trainer is Required")]

        [Display(Name = "Trainer")]
        public int TrainerId { get; set; }

        [Required(ErrorMessage = "Please select at least one Trainee.")]
        public List<int> SelectedTrainees { get; set; } = new List<int>();

    }
}
