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
        ///User 
    
        public ApplicationUser User { get; set; }
        public ViewModel(UserManager<ApplicationUser> userManager)
        {
                _userManager = userManager;
            User = new ApplicationUser();
        }

        public async Task<IActionResult> OnGetAsync(string Id)
        {
            try
            {

                User = await _userManager.Users.Where(e=>e.Id==Id).FirstOrDefaultAsync();
                if (User != null)
                {

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
