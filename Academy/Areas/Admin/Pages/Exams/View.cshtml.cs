using Academy.Data;
using Academy.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace Academy.Areas.Admin.Pages.Exams
{
    public class ViewModel : PageModel
    {
        private readonly AcademyContext _context;

        public Exam exam { get; set; }

        public ViewModel(AcademyContext context)
        {
            _context = context;
            exam = new Exam();

        }
        public async Task<IActionResult> OnGetAsync(int? id)
        {

            try
            {

                exam= await _context.Exams.Include(a => a.Trainer)
                      .Include(a => a.Subscription)
                      .ThenInclude(a => a.Trainee)
                      .Include(a => a.Subscription)
                      .ThenInclude(a => a.Category)
                      .ThenInclude(a => a.Department)
                      .ThenInclude(a => a.Branch).FirstOrDefaultAsync(m => m.ExamId == id);
                if (exam!= null)
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
