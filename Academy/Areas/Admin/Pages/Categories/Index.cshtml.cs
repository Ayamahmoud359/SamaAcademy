using Academy.Data;
using Academy.DTO;
using Academy.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using NToastNotify;

namespace Academy.Areas.Admin.Pages.Categories
{
    public class IndexModel : PageModel
    {
       
        private readonly AcademyContext _context;
        [BindProperty]
        public int Catid { set; get; }
        private readonly IToastNotification _toastNotification;
        public IndexModel(AcademyContext context, IToastNotification toastNotification)
        {
            _context = context;
            _toastNotification = toastNotification;
        }
        public void OnGet()
        {

        }

     
        public async Task<IActionResult> OnPostDeleteCategory()
        {
            try
            {
                var category = _context.Categories.Find(Catid);
                if (category != null)
                {
                    category.IsActive = false;
                    category.IsDeleted = true;
                    _context.Attach(category).State = EntityState.Modified;
                    await _context.SaveChangesAsync();
                    _toastNotification.AddSuccessToastMessage("Deleted Successfull");
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
