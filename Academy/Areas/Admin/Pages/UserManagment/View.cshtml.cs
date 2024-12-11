using Academy.Data;
using Academy.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace Academy.Areas.Admin.Pages.UserManagment
{
    public class ViewModel : PageModel
    {

        //User Manager
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly AcademyContext _context;
        ///User 

        public ApplicationUser User { get; set; }
        public Branch? UserBranch { get; set; }
        public ViewModel(UserManager<ApplicationUser> userManager,AcademyContext context)
        {
             _userManager = userManager;
            _context = context;
            User = new ApplicationUser();
        }

        public async Task<IActionResult> OnGetAsync(string Id)
        {
            try
            {

                User = await _userManager.Users.Where(e=>e.Id==Id).FirstOrDefaultAsync();
                if (User != null)
                {
                    if(User.BranchId != null)
                    {
                        UserBranch = await _context.Branches.FirstOrDefaultAsync(e=> e.BranchId == User.BranchId && e.IsActive && !e.IsDeleted);
                    }

                    return Page();
                }
                return RedirectToPage("../NotFound");
            }
            catch (Exception e)
            {
                return RedirectToPage("../Error");
            }
        }

        
    }
}
