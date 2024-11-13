using Academy.Data;
using Academy.DTO;
using Academy.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace Academy.Areas.Admin.Pages.Parents
{
    public class AddModel : PageModel
    {
        [BindProperty]
        public ParentVM ParentVM { get; set; }
     
        [BindProperty]
        [Required(ErrorMessage = "User Name  is required")]
        [EmailAddress]
        [Display(Name = "User Name")]
        [Remote("IsEmailAvailable", "Functions", ErrorMessage = "This User Name is already taken.")]
        public string UserName { get; set; }

        private readonly AcademyContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        public AddModel(AcademyContext context, UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            ParentVM = new ParentVM();
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
        }
        public void OnGet()
        {
           
        }


        public async Task<IActionResult> OnPostAsync()
        {
           
            if (!ModelState.IsValid)
            {
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
                   return RedirectToPage("Index"); 

                }
                _context.Parents.Remove(parent);
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
                // If we got this far, something failed, redisplay form
                return Page();
            }

            catch (Exception exc)
            {
                ModelState.AddModelError(string.Empty, exc.Message);
                return Page();


            }


        }
          

        }
    }

