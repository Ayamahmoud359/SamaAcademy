using Academy.Data;
using Academy.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace Academy.Areas.Admin.Pages.Banners
{
    public class ViewModel : PageModel
    {
        private readonly AcademyContext _context;

        public Banner Banner { get; set; }

        public ViewModel(AcademyContext context)
        {
            _context = context;
            Banner = new Banner();

        }
        public async Task<IActionResult> OnGetAsync(int? id)
        {

            try
            {

                Banner = await _context.Banners.FirstOrDefaultAsync(m => m.Id == id);
                if (Banner != null)
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
