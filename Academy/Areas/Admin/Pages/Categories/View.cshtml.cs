using Academy.Data;
using Academy.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace Academy.Areas.Admin.Pages.Categories
{
    public class ViewModel : PageModel
    {
        private readonly AcademyContext _context;

        public Category cat { get; set; }

        public ViewModel(AcademyContext context)
        {
            _context = context;
            cat= new Category();

        }
        public async Task<IActionResult> OnGetAsync(int? id)
        {

            try
            {

                cat= await _context.Categories.Include(a => a.Department)
                      .ThenInclude(a=>a.Branch)
                     .FirstOrDefaultAsync(m => m.CategoryId == id);
                if (cat != null)
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
