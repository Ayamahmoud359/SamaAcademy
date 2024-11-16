using Academy.Data;
using Academy.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using NToastNotify;

namespace Academy.Areas.Admin.Pages.Absences
{
    public class IndexModel : PageModel
    {
        private readonly AcademyContext _context;
        [BindProperty]
        public int Absenceid { set; get; }
        private readonly IToastNotification _toastNotification;

        public IndexModel(AcademyContext context, IToastNotification toastNotification)
        {
            _context = context;
            _toastNotification = toastNotification;
        }

        public async Task<IActionResult> OnPostDeleteAttendence()
        {
            try
            {
                var attendence = _context.Abscenses.Find(Absenceid);
                if (attendence != null)
                {
                    attendence.IsDeleted = true;


                    _context.Attach(attendence).State = EntityState.Modified;
                    await _context.SaveChangesAsync();
                    _toastNotification.AddSuccessToastMessage("Deleted Successfully");

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
