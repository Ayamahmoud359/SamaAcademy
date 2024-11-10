using Academy.Data;
using Academy.DTO;
using Academy.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Academy.Areas.Admin.Pages.Departments
{
    public class EditModel : PageModel
    {
        private readonly AcademyContext _context;
     
        

        public EditModel(AcademyContext context)
        {
            _context = context;
           
        }
        [BindProperty]

        public DepartmentVM Dept{ get; set; }
      
       
        public List<Branch> Branches { get; set; }
        public async Task<IActionResult> OnGetAsync(int? id)
        {

            try
            {
                Branches = _context.Branches.Where(b => b.IsActive && !b.IsDeleted).ToList();
                var department = await _context.Departments.Include(d=>d.Branch).FirstOrDefaultAsync(m => /*m.Branch.IsActive&&*/ m.DepartmentId == id );
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
            catch(Exception e)
            {
               return  RedirectToPage("../Error");
            }
          

           
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            Branches = _context.Branches.Where(b => b.IsActive && !b.IsDeleted).ToList();
            if (!ModelState.IsValid)
            {
                return Page();
            }

            try
            {
                var department = await _context.Departments.FirstOrDefaultAsync(m => m.DepartmentId == Dept.DepartmentId);
                if (department != null)
                {
                    department.DepartmentName = Dept.DepartmentName;
                    department.DepartmentDescription = Dept.DepartmentDescription;
                   // department.BranchId = Dept.BranchId;
                    department.IsActive = Dept.IsActive;
                    _context.Attach(department).State = EntityState.Modified;
                    await _context.SaveChangesAsync();
                    return RedirectToPage("Index");
                }
                ModelState.AddModelError(string.Empty, "SomeThing Went Wrong");

                return Page();
            }
            catch (Exception e)
            {
                ModelState.AddModelError(string.Empty, e.Message);
                return Page();
            }

        }
    }
}
