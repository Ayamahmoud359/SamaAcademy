using Academy.DTO;
using Academy.Models;
using Academy.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Academy.Areas.Admin.Pages.Departments
{
    public class AddModel : PageModel
    {
        [BindProperty]
        public Department Dept { get; set; }
        public List<Branch> Branches  { get; set; }

        private readonly AcademyContext _context;
        public AddModel(AcademyContext context)
        {
            _context = context;
        }
        public void OnGet()
        {
            Branches = _context.Branches.ToList();
        }
        public async Task<IActionResult> OnPostAsync()
        {
            Department department = new Department()
            {
           
                IsActive = true,
                DepartmentNameAR=Dept.DepartmentNameAR,
                DepartmentNameEN=Dept.DepartmentNameAR,
                DepartmentDescriptionAR=Dept.DepartmentDescriptionAR,
                DepartmentDescriptionEN=Dept.DepartmentDescriptionAR,
                BranchId=Dept.BranchId

            };
            try
            {
                _context.Departments.Add(department);
                _context.SaveChanges();
                return RedirectToPage("/Admin/Index");
            }
            catch (Exception exc)
            {
                ModelState.AddModelError(string.Empty, exc.Message);
                return Page();
            }


        }
    }
}

