using System.ComponentModel.DataAnnotations;

namespace Academy.DTO
{
    public class BranchVM

    {
        public int BranchId { get; set; }
        [Required(ErrorMessage ="Branch Name is Required")]
       
        [Display(Name = "Branch Name")]
        public string BranchName { get; set; }
       
        [Required(ErrorMessage = "Branch Address is Required")]
      
        [Display(Name = "Branch Address")]
        public string BranchAddress { get; set; }

        [Required(ErrorMessage = "Branch Phone is Required")]
        [Display(Name = "Branch Phone")]
        public string Phone { get; set; } 
        public bool IsActive { get; set; }
       

    }
}
