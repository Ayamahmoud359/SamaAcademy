using Academy.Data;
using Academy.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace Academy.Areas.Admin.Pages.Absences
{
    public class ViewModel : PageModel
    {
        private readonly AcademyContext _context;

        public Absence absence { get; set; }

        public ViewModel(AcademyContext context)
        {
            _context = context;
            absence = new Absence();

        }
        public async Task<IActionResult> OnGetAsync(int? id)
        {

            try
            {

              absence= await _context.Abscenses.Include(a=>a.Trainer)
                    .Include(a => a.Subscription)
                    .ThenInclude(a => a.Trainee)
                    .Include(a=>a.Subscription)
                    .ThenInclude(a=>a.Category)
                    .ThenInclude(a=>a.Department)
                    .ThenInclude(a=>a.Branch).FirstOrDefaultAsync(m => m.AbsenceId == id);
                if (absence!= null)
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
