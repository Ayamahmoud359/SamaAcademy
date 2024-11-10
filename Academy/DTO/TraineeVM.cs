using Academy.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace Academy.DTO
{
    public class TraineeVM
    {
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
     
        [Required(ErrorMessage = "Trainee Phone is required")]
        [Display(Name = "Trainee phone")]
        public string TraineePhone { get; set; }
      
        [Display(Name = "Trainee Address")]
        public string? TraineeAddress { get; set; }
        public int TraineeId { get; set; }
        [Required(ErrorMessage = "Trainee Name is required")]
        [Display(Name = "Trainee Name")]
        public string TraineeName { get; set; }
  
        [DataType(DataType.Date)]
        [Required(ErrorMessage = "Trainee Birthdate is required")]
        [Display(Name = "Trainee Birthdate")]
        public string BirthDate { get; set; }

        public string? Image { get; set; }
        [Required(ErrorMessage = "Trainee Nationality is required")]
        [Display(Name = "Trainee Nationality")]
        public string Nationality { get; set; }

        [Display(Name = "ResidencyNumber")]
        [Required(ErrorMessage = "Trainee ResidencyNumber is required")]
    
        public string ResidencyNumber { get; set; }
        public bool IsActive { get; set; }

        public int ParentId { get; set; }

        [Required(ErrorMessage = "Branch is required")]
        [Display(Name = "Branch")]

        public int BranchId { get; set; }

        [DataType(DataType.Date)]
        [Required(ErrorMessage = "Supsciption StartDate is required")]
        [Display(Name = "StartDate")]
        public string StartDate { get; set; }
        [DataType(DataType.Date)]
        [Required(ErrorMessage = "Supsciption EndDate is required")]
        [Display(Name = "EndDate")]
        public string EndDate { get; set; }

        //[Required(ErrorMessage = "Department is required")]
        [Display(Name = "Department")]
        public int? DepartmentId { get; set; }

        //[Required(ErrorMessage = "Category is required")]
        [Display(Name = "Category")]
        public int? CategoryId { get; set; }

    }
}
