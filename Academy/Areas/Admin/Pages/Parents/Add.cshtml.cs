using Academy.Data;
using Academy.DTO;
using Academy.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Academy.Areas.Admin.Pages.Parents
{
    public class AddModel : PageModel
    {
        [BindProperty]
        public ParentVM ParentVM { get; set; }
        public List<Branch> Branches { get; set; }
      

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
            Branches = _context.Branches.ToList();
            
        }


        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {
                var parent = new Parent
                {
                    
                    IsActive = true,
                    BranchId=ParentVM.BranchId,
                    ParentEmail=ParentVM.Email,
                    ParentName=ParentVM.ParentName,
                    ParentAddress=ParentVM.ParentAddress,
                    ParentPhone=ParentVM.ParentPhone,

                };
                try
                {
                    _context.Parents.Add(parent);
                    await _context.SaveChangesAsync();
                }
                catch (Exception exc)
                {
                    ModelState.AddModelError(string.Empty, exc.Message);
                    return Page();

                }


                var user = new ApplicationUser
                {
                    UserName = ParentVM.Email,
                    Email = ParentVM.Email,

                    PhoneNumber = ParentVM.ParentPhone,
                    EntityId = parent.ParentId,
                    EntityName = "Parent"

                };

                try
                {
                    var result = await _userManager.CreateAsync(user, ParentVM.Password);

                    if (result.Succeeded)
                    {
                        Redirect("/Admin/Index");

                    }
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

                }

            }
            return Page();


        }
    }
}
