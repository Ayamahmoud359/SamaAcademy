using Academy.Models;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Academy.DTO
{
    public class TrainerVM
    {
        public int TrainerId { get; set; }
        [EmailAddress]
        [Display(Name = "Email")]
        [Remote("IsEmailAvailable", "Functions", ErrorMessage = "This email is already taken.")]
        public string? Email { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [StringLength(10, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 8)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }
        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        [Required(ErrorMessage = "Trainer Name is required")]
        public string TrainerName { get; set; }
        [Required(ErrorMessage = "Phone is required")]
        public string TrainerPhone { get; set; }
        [Required(ErrorMessage = "Trainer Address is required")]
        public string TrainerAddress { get; set; }

        [Required(ErrorMessage = "Branch is required")]
        public int BranchId { get; set; }
        ///Department Id
        [Required(ErrorMessage = "Department is required")]
        public int DepartmentId { get; set; }
        [Required(ErrorMessage = "Please select at least one category.")]
        public List<int> SelectedCategories { get; set; } = new List<int>();
    }
}
