using Academy.Data;
using Academy.DTO;
using Academy.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using NToastNotify;

namespace Academy.Areas.Admin.Pages.Parents
{
    public class IndexModel : PageModel
    {
   
       
        private readonly AcademyContext _context;
        [BindProperty]
        public int Parentid { set; get; }
        private readonly IToastNotification _toastNotification;
        private readonly UserManager<ApplicationUser> _userManager;
        public IndexModel(AcademyContext context, IToastNotification toastNotification, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _toastNotification = toastNotification;
            _userManager = userManager;
        }
        public void OnGet()
        {

        }

      
        public async Task<IActionResult> OnPostDeleteParent()
        {
            try
            {
                var parent = _context.Parents.Find(Parentid);
                if (parent != null)
                {
                    parent.IsActive = false;
                    parent.IsDeleted = true;
                    _context.Attach(parent).State = EntityState.Modified;
                    var user = await _userManager.FindByNameAsync(parent.ParentEmail);
                    if (user != null)
                    {
                        var result = await _userManager.DeleteAsync(user);
                        if (result.Succeeded)
                        {

                            await _context.SaveChangesAsync();
                            _toastNotification.AddSuccessToastMessage("Deleted Successfully");
                            return RedirectToPage("Index");
                        }

                    }
                }

              
            }
            catch (Exception)

            {
                _toastNotification.AddErrorToastMessage("Something went wrong");
            }
            _toastNotification.AddErrorToastMessage("Something went wrong");
            return RedirectToPage("Index");

        }
    }
}
