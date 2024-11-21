using Academy.Data;
using Academy.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace Academy.Areas.Admin.Pages.TrainerCategoriesManagment
{
    public class ViewModel : PageModel
    {

        private readonly AcademyContext _context;

        public TrainerCategories trainerCategory { get; set; }

        public ViewModel(AcademyContext context)
        {
            _context = context;
          trainerCategory = new TrainerCategories();

        }
        public async Task<IActionResult> OnGetAsync(int? id)
        {

            try
            {

                trainerCategory = await _context.CategoryTrainers.Include(a => a.Trainer)
                    .Include(a=>a.Category)
                    .ThenInclude(a=>a.Department)
                    .ThenInclude(a=>a.Branch)
                     .FirstOrDefaultAsync(m => m.CategoryId == id);
                if (trainerCategory != null)
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
