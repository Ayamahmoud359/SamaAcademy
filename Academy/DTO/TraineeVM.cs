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

        [Required(ErrorMessage = "Trainee Address is required")]
        [Display(Name = "Trainee Address")]
        public string TraineeAddress { get; set; }
        public int TraineeId { get; set; }

        [Required(ErrorMessage = "Trainee Name is required")]
        [Display(Name = "Trainee Name")]
        public string TraineeName { get; set; }
  
       
        [Required(ErrorMessage = "Trainee Birthdate is required")]
        [Display(Name = "Trainee Birthdate")]
        public DateOnly? BirthDate { get; set; }

        public string? Image { get; set; }
        [Required(ErrorMessage = "Trainee Nationality is required")]
        [Display(Name = "Trainee Nationality")]
        public string Nationality { get; set; }

        [Display(Name = "ResidencyNumber")]
        [Required(ErrorMessage = "Trainee ResidencyNumber is required")]
    
        public string ResidencyNumber { get; set; }
        public bool IsActive { get; set; }

        [Display(Name = "Parent")]
        [Required(ErrorMessage = "Trainee Parent is required")]

        public int ParentId { get; set; }

      
       

    }
}
