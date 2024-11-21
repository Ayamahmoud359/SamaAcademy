using Academy.Data;
using Academy.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace Academy.Areas.Admin.Pages.Champions
{
    public class ViewModel : PageModel
    {
        private readonly AcademyContext _context;

        public Champion Champion { get; set; }

        public ViewModel(AcademyContext context)
        {
            _context = context;
            Champion = new Champion();

        }
        public async Task<IActionResult> OnGetAsync(int? id)
        {

            try
            {

               Champion= await _context.Champions.FirstOrDefaultAsync(m => m.ChampionId == id);
                if (Champion != null)
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
