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
        public DepartmentVM Dept { get; set; } = new DepartmentVM();
        public List<Branch> Branches { get; set; } = new List<Branch>();

        private readonly AcademyContext _context;
        public AddModel(AcademyContext context)
        {
            _context = context;
        }
        public void OnGet()
        {
            Branches = _context.Branches.Where(b=>b.IsActive).ToList();
        }
        public async Task<IActionResult> OnPostAsync()

        {
            Branches = _context.Branches.Where(b=>b.IsActive).ToList();
            if (!ModelState.IsValid)
            {
                return Page();
            }
            Department department = new Department()
            {
           
                IsActive = true,
                DepartmentName=Dept.DepartmentName,
       
                DepartmentDescription=Dept.DepartmentDescription,
                BranchId=Dept.BranchId

            };
         
            try
            {
                _context.Departments.Add(department);
                _context.SaveChanges();
                return RedirectToPage("Index");
            }
            catch (Exception exc)
            {
                ModelState.AddModelError(string.Empty, exc.Message);
                return Page();
            }


        }
    }
}

