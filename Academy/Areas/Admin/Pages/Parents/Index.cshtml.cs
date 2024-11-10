using Academy.Data;
using Academy.DTO;
using Academy.Models;
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
        public IndexModel(AcademyContext context, IToastNotification toastNotification)
        {
            _context = context;
            _toastNotification = toastNotification;
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

                    await _context.SaveChangesAsync();
                    _toastNotification.AddSuccessToastMessage("Deleted Successfully");
                }

                else
                {
                    _toastNotification.AddErrorToastMessage("Something went wrong");
                }
            }
            catch (Exception)

            {
                _toastNotification.AddErrorToastMessage("Something went wrong");
            }

            return RedirectToPage("Index");

        }
    }
}
