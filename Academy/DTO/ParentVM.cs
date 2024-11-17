using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Academy.DTO
{
    public class ParentVM
    {
        public int ParentId { get; set; }

        [EmailAddress]
        [Display(Name = "Email")]

        public string? ParentEmail { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [StringLength(10, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 8)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }
        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        [Required(ErrorMessage = "Parent Name is required")]
        public string ParentName { get; set; }
        //[Required(ErrorMessage = "Parent Phone is required")]
        public string? ParentPhone { get; set; }
        //[Required(ErrorMessage = "Parent Address is required")]
        public string? ParentAddress { get; set; }

       
    }
}
