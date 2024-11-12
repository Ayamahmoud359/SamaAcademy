using Academy.Data;
using Academy.DTO;
using Academy.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Academy.Areas.Admin.Pages.Parents
{
    public class AddTraineeModel : PageModel
    {
        [BindProperty]
        public TraineeVM trainee { get; set; }

        [BindProperty]
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress]
        [Display(Name = "Email")]
        [Remote("IsEmailAvailable", "Functions", ErrorMessage = "This email is already taken.")]
        public string Email { get; set; }

        public List<SelectListItem> Nationalities { get; set; }


        private readonly AcademyContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        public AddTraineeModel(AcademyContext context, UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            trainee = new TraineeVM();

            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;

            Nationalities = new List<SelectListItem>
                {
            new SelectListItem { Text = "American", Value = "US" },
            new SelectListItem { Text = "Canadian", Value = "CA" },
            new SelectListItem { Text = "Mexican", Value = "MX" },
            new SelectListItem { Text = "British", Value = "GB" },
            new SelectListItem { Text = "German", Value = "DE" },
            new SelectListItem { Text = "Indian", Value = "IN" },
            new SelectListItem { Text = "Australian", Value = "AU" },
        };

        }


        public async Task<IActionResult> OnGet(int id)
        {
            try
            {

                if (id != 0)
                {
                    trainee.ParentId = id;

                    return Page();
                }
                return RedirectToPage("../Error");
            }
            catch (Exception e)
            {
                return RedirectToPage("../Error");
            }


        }
    

            public async Task<IActionResult> OnPostAsync()
            {
                if (!ModelState.IsValid)
                {
                    return Page();
                }
                try
                {
                var newtrainee = new Trainee
                {
                    TraineeName = trainee.TraineeName,
                    TraineePhone=trainee.TraineePhone,
                    TraineeAddress=trainee.TraineeAddress,
                    TraineeEmail=Email,
                    BirthDate=trainee.BirthDate,
                    ParentId=trainee.ParentId,
                    Nationality=trainee.Nationality,
                    ResidencyNumber=trainee.ResidencyNumber,
                    IsActive = true
                };

                _context.Trainees.Add(newtrainee);
                    await _context.SaveChangesAsync();
              
                    var user = new ApplicationUser
                    {
                        UserName = Email,
                        Email = Email,
                        PhoneNumber = trainee.TraineePhone,
                        EntityId = newtrainee.TraineeId,
                        EntityName = "Trainee"

                    };


                    var result = await _userManager.CreateAsync(user, trainee.Password);

                    if (result.Succeeded)
                    {
                        return Redirect("~/Admin/Trainees/Index");

                    }
                   
                    _context.Trainees.Remove(newtrainee);
                    _context.SaveChanges();
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                    return Page();


                }
                catch (Exception e)
                {
                    ModelState.AddModelError(string.Empty, e.Message);
                    return Page();
                }

            }

          

        }
    }
