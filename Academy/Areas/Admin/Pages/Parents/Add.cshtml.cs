using Academy.Data;
using Academy.DTO;
using Academy.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using NToastNotify;
using System.ComponentModel.DataAnnotations;

namespace Academy.Areas.Admin.Pages.Parents
{
    public class AddModel : PageModel
    {
        [BindProperty]
        public ParentVM ParentVM { get; set; }
     
        [BindProperty]
        [Required(ErrorMessage = "User Name  is required")]
    
        [Display(Name = "User Name")]
        [Remote("IsEmailAvailable", "Functions", ErrorMessage = "This User Name is already taken.")]
        public string UserName { get; set; }

        private readonly AcademyContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IToastNotification _toastNotification;

        public AddModel(AcademyContext context
            , UserManager<ApplicationUser> userManager
            , SignInManager<ApplicationUser> signInManager
            ,IToastNotification toastNotification)
        {
            ParentVM = new ParentVM();
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
           _toastNotification = toastNotification;
        }
        public void OnGet()
        {
           
        }


        public async Task<IActionResult> OnPostAsync()
        {
           
            if (!ModelState.IsValid)
            {
                _toastNotification.AddErrorToastMessage("Something Went Wrong");
                return Page();

            }
            try
            {
                var parent = new Parent
                {

                    IsActive = true,
                    ParentEmail = ParentVM.ParentEmail,
                    ParentName = ParentVM.ParentName,
                    ParentAddress = ParentVM.ParentAddress,
                    ParentPhone = ParentVM.ParentPhone,
                    UserName=UserName

                };

                _context.Parents.Add(parent);
                await _context.SaveChangesAsync();


                var user = new ApplicationUser
                {
                    UserName = UserName,
                    Email = ParentVM.ParentEmail==null?"":ParentVM.ParentEmail,
                    PhoneNumber = ParentVM.ParentPhone,
                    EntityId = parent.ParentId,
                    EntityName = "Parent"

                };

                var result = await _userManager.CreateAsync(user, ParentVM.Password);

                if (result.Succeeded)
                {
                    _toastNotification.AddSuccessToastMessage("Parent Added Successfullly");
                    return RedirectToPage("Index"); 

                }
                _context.Parents.Remove(parent);
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
                // If we got this far, something failed, redisplay form
               
            }

            catch (Exception exc)
            {
                ModelState.AddModelError(string.Empty, exc.Message);
                


            }
            _toastNotification.AddErrorToastMessage("Something Went Wrong");
            return Page();

        }
          

        }
    }

