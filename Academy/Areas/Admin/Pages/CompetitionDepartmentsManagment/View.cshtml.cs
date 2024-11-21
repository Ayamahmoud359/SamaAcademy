using Academy.Data;
using Academy.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace Academy.Areas.Admin.Pages.CompetitionDepartmentsManagment
{
    public class ViewModel : PageModel
    {
        private readonly AcademyContext _context;

        public CompetitionDepartment CompetitionDepartment { get; set; }

        public ViewModel(AcademyContext context)
        {
            _context = context;

        }
        public async Task<IActionResult> OnGetAsync(int? id)
        {

            try
            {

               CompetitionDepartment = await _context.CompetitionDepartment.FirstOrDefaultAsync(m => m.Id == id);
                if (CompetitionDepartment != null)
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
