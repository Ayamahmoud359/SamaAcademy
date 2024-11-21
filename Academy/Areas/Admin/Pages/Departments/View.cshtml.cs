using Academy.Data;
using Academy.DTO;
using Academy.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace Academy.Areas.Admin.Pages.Departments
{
    public class ViewModel : PageModel
    {
        private readonly AcademyContext _context;

      public  Department Dept { get; set; } 

        public ViewModel(AcademyContext context)
        {
            _context = context;

        }
        public async Task<IActionResult> OnGetAsync(int?id)
        {

            try
            {
               
                Dept = await _context.Departments.Include(a=>a.Branch).FirstOrDefaultAsync(m => m.DepartmentId == id);
                if (Dept != null)
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
