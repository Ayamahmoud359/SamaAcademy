using Academy.Data;
using Academy.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using NToastNotify;

namespace Academy.Areas.Admin.Pages.TrainerCategoriesManagment
{
    public class IndexModel : PageModel
    {

        private readonly AcademyContext _context;
        [BindProperty]
        public int TrainerCategoryid { set; get; }
        private readonly IToastNotification _toastNotification;

        public IndexModel(AcademyContext context, IToastNotification toastNotification)
        {
            _context = context;
            _toastNotification = toastNotification;
        }

        public async Task<IActionResult> OnPostDeleteTrainerCategory()
        {
            try
            {
                var tranierCat = _context.CategoryTrainers.Find(TrainerCategoryid);
                if (tranierCat != null)
                {
                 
                   
                    tranierCat.IsActive = false;
                    tranierCat.IsDeleted = true;


                    _context.Attach(tranierCat).State = EntityState.Modified;
                    await _context.SaveChangesAsync();
                    _toastNotification.AddSuccessToastMessage("Deleted Successfully");
                    return RedirectToPage("Index");
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
