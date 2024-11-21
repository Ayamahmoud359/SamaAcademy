using Academy.Data;
using Academy.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace Academy.Areas.Admin.Pages.CompetitionTeamsManagment
{
    public class ViewModel : PageModel
    {
        private readonly AcademyContext _context;

        public CompetitionTeam CompetitionTeam { get; set; }

        public ViewModel(AcademyContext context)
        {
            _context = context;

        }
        public async Task<IActionResult> OnGetAsync(int? id)
        {

            try
            {

               CompetitionTeam= await _context.CompetitionTeam.Include(a=>a.Trainer).Include(a=>a.CompetitionDepartment).FirstOrDefaultAsync(m => m.Id == id);
                if (CompetitionTeam != null)
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
