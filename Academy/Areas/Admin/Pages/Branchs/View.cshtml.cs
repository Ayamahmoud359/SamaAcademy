using Academy.Data;
using Academy.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace Academy.Areas.Admin.Pages.Branchs
{
    public class ViewModel : PageModel
    {

        private readonly AcademyContext _context;

        public Branch Branch { get; set; }

        public ViewModel(AcademyContext context)
        {
            _context = context;

        }
        public async Task<IActionResult> OnGetAsync(int? id)
        {

            try
            {

               Branch = await _context.Branches.FirstOrDefaultAsync(m => m.BranchId == id);
                if (Branch != null)
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

