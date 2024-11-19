using Academy.Data;
using Academy.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using NToastNotify;

namespace Academy.Areas.Admin.Pages.Champions
{
    public class IndexModel : PageModel
    {
        private readonly AcademyContext _context;
        [BindProperty]
        public int championid { set; get; }
        private readonly IToastNotification _toastNotification;
        public IndexModel(AcademyContext context, IToastNotification toastNotification)
        {
            _context = context;
            _toastNotification = toastNotification;
        }
        public void OnGet()
        {

        }


        public async Task<IActionResult> OnPostDeleteChampion()
        {
            try
            {
                var champion = _context.Champions.Find(championid);
                if (champion != null)
                {
                  
                 
                    champion.IsDeleted = true;


                    _context.Attach(champion).State = EntityState.Modified;
                    await _context.SaveChangesAsync();
                    _toastNotification.AddSuccessToastMessage("Deleted Successfull");

                    return RedirectToPage("Index");
                }


            }
            catch (Exception)

            {

            }
            _toastNotification.AddErrorToastMessage("Something went wrong");


            return RedirectToPage("Index");

        }
    }
}
