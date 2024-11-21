using Academy.Data;
using Academy.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace Academy.Areas.Admin.Pages.Parents
{
    public class ViewModel : PageModel
    {
        private readonly AcademyContext _context;

        public Parent Parent { get; set; }

        public ViewModel(AcademyContext context)
        {
            _context = context;
           Parent = new Parent();
        }
        public async Task<IActionResult> OnGetAsync(int? id)
        {

            try
            {

               Parent = await _context.Parents.FirstOrDefaultAsync(m => m.ParentId== id);
                if (Parent != null)
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
