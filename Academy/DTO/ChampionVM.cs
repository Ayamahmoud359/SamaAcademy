using Academy.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Academy.DTO
{
    public class ChampionVM
    {
        public int ChampionId { get; set; }

        [Required(ErrorMessage = "Champion Name is Required")]

        [Display(Name = "Champion Name")]
        public string ChampionName { get; set; }

        [Required(ErrorMessage = "Champion Description is Required")]

        [Display(Name = "Champion Description")]
        public string? ChampionDescription { get; set; }


        [Required(ErrorMessage = "Champion Date is Required")]

        [Display(Name = "Champion Date")]
        public DateOnly? ChampionDate { get; set; }
      

    }
}
