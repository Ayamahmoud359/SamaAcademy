using System.ComponentModel.DataAnnotations;

namespace Academy.DTO
{
    public class BranchVM
    {
        [Required(ErrorMessage ="Branch Name is Required")] 
        public string BranchNameEN { get; set; }
        [Required(ErrorMessage = "Branch Name is Required")]
        public string BranchNameAR { get; set; }
        [Required(ErrorMessage = "Branch Address is Required")]
        public string BranchAddress { get; set; }
    }
}
