using Academy.DTO;
using Academy.Models;
using Academy.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using NToastNotify;

namespace Academy.Areas.Admin.Pages.Departments
{
    public class AddModel : PageModel
    {
        [BindProperty]
        public DepartmentVM Dept { get; set; } = new DepartmentVM();
        public List<Branch> Branches { get; set; } = new List<Branch>();

        private readonly AcademyContext _context;
        private readonly IToastNotification _toastNotification;

        public AddModel(AcademyContext context,IToastNotification toastNotification)
        {
            _context = context;
           _toastNotification = toastNotification;
        }
        public void OnGet()
        {
            Branches = _context.Branches.Where(b=>b.IsActive&&!b.IsDeleted).ToList();
        }
        public async Task<IActionResult> OnPostAsync()

        {
            Branches = _context.Branches.Where(b=>b.IsActive&&!b.IsDeleted).ToList();
            if (!ModelState.IsValid)
            {
                _toastNotification.AddErrorToastMessage("Something Went Wrong");
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
                _toastNotification.AddSuccessToastMessage("Department Added Successfully");
                return RedirectToPage("Index");
            }
            catch (Exception exc)
            {
                ModelState.AddModelError(string.Empty, exc.Message);
                _toastNotification.AddErrorToastMessage("Something Went Wrong");
                return Page();
            }


        }
    }
}

