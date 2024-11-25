using Academy.Data;
using Academy.Models;
using Academy.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Academy.Areas.Admin.Pages.UserManagment
{
    public class IndexModel : PageModel
    {
        [BindProperty]
        public AddUserVM userVM { get; set; }
        private readonly AcademyContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        public IndexModel(AcademyContext context, UserManager<ApplicationUser> userManager)
        {
            userVM= new AddUserVM();
            _context = context;
            _userManager = userManager;

        }
        public void OnGet()
        {
        }
        public IActionResult OnGetGetBranches()
        {
            var branchs = _context.Branches.ToList();
            return new JsonResult(branchs);
        }
        public async Task<IActionResult> OnPostAddUser()
        {
            try
            {
                // First, check if the user exists by email
                var userExistsByEmail = await _userManager.FindByEmailAsync(userVM.Email);

                // If a user exists by email, return to the page (error or message could be added)
                if (userExistsByEmail != null)
                {
                    return Page();
                }

                // Next, check if the user exists by username
                var userExistsByUserName = await _userManager.FindByNameAsync(userVM.UserName);

                // If a user exists by username, return to the page (error or message could be added)
                if (userExistsByUserName != null)
                {
                    return Page();
                }

                // Proceed with creating the new user if neither exists
                var user = new ApplicationUser
                {
                    UserName = userVM.UserName!=null?userVM.UserName:"",
                    Email = userVM.Email != null ? userVM.Email : "",
                    FullName = userVM.FullName,
                    PhoneNumber = userVM.Phone,
                    Address = userVM.Address,
                    EntityName = userVM.EntityType,
                    BranchId = userVM.BranchId != null ? userVM.BranchId : null
                };

                var result = await _userManager.CreateAsync(user, userVM.Password);

                if (result.Succeeded)
                {
                    return Redirect("/Admin/UserManagment/Index");
                }
                else
                {
                    return Redirect("/Admin/UserManagment/Index");
                }

            }
            catch (Exception)
            {
                return Redirect("/Admin/UserManagment/Index");
            }

        }
    }
}
