using Academy.Data;
using Academy.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace Academy.Areas.Admin.Pages.Trainees
{
    public class ViewModel : PageModel
    {
        private readonly AcademyContext _context;

        public Trainee trainee { get; set; }

        public ViewModel(AcademyContext context)
        {
            _context = context;
           trainee = new Trainee();
        }
        public async Task<IActionResult> OnGetAsync(int? id)
        {

            try
            {

              trainee = await _context.Trainees.Include(a=>a.Parent).FirstOrDefaultAsync(m => m.TraineeId == id);
                if (trainee != null)
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
