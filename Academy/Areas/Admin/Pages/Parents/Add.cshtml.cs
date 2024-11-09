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
        public List<Branch> Branches { get; set; }
        [BindProperty]
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress]
        [Display(Name = "Email")]
        [Remote("IsEmailAvailable", "Functions", ErrorMessage = "This email is already taken.")]
        public string Email { get; set; }

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
            Branches = _context.Branches.Where(b=>b.IsActive).ToList();
            
        }


        public async Task<IActionResult> OnPostAsync()
        {
            Branches = _context.Branches.Where(b=>b.IsActive).ToList();
            if (!ModelState.IsValid)
            {
                return Page();
            }
            try
            {
                var parent = new Parent
                {

                    IsActive = true,
                    BranchId = ParentVM.BranchId,
                    ParentEmail = Email,
                    ParentName = ParentVM.ParentName,
                    ParentAddress = ParentVM.ParentAddress,
                    ParentPhone = ParentVM.ParentPhone,

                };

                _context.Parents.Add(parent);
                await _context.SaveChangesAsync();


                var user = new ApplicationUser
                {
                    UserName = Email,
                    Email = Email,
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

