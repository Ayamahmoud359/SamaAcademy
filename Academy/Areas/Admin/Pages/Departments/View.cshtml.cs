using Academy.Data;
using Academy.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace Academy.Areas.Admin.Pages.Departments
{
    public class ViewModel : PageModel
    {
        private readonly AcademyContext _context;

      public  DepartmentVM Dept { get; set; } 

        public ViewModel(AcademyContext context)
        {
            _context = context;

        }
        public async Task<IActionResult> OnGetAsync(int?id)
        {

            try
            {
               
                var department = await _context.Departments.FirstOrDefaultAsync(m => m.DepartmentId == id);
                if (department != null)
                {
                    Dept = new DepartmentVM();
                    Dept.DepartmentName = department.DepartmentName;
                    Dept.DepartmentDescription = department.DepartmentDescription;
                    Dept.BranchId = department.BranchId;
                    Dept.DepartmentId = department.DepartmentId;
                    Dept.IsActive = department.IsActive;
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
