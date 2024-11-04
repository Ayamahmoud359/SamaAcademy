using System.ComponentModel.DataAnnotations;

namespace Academy.DTO
{
    public class ParentVM
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }
        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        [Required(ErrorMessage = "Trainer Name is required")]
        public string ParentName { get; set; }
        public string ParentPhone { get; set; }
        [Required(ErrorMessage = "Trainer Address is required")]
        public string ParentAddress { get; set; }

        [Required(ErrorMessage = "Branch is required")]
        public int BranchId { get; set; }
   
    }
}
