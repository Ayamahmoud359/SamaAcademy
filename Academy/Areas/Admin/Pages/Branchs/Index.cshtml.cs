using Academy.Data;
using Academy.DTO;
using Academy.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using NToastNotify;

namespace Academy.Areas.Admin.Pages.Branchs
{
    public class IndexModel : PageModel
    {
       
        public DepartmentVM Dept{ set; get; }
        private readonly AcademyContext _context;
        [BindProperty]
        public int Branchid { set; get; }
        private readonly IToastNotification _toastNotification;
        public IndexModel(AcademyContext context, IToastNotification toastNotification)
        {
            _context = context;
            _toastNotification = toastNotification;
        }
        public void OnGet()
        {

        }

        public IActionResult OnPostAddDepartment([FromBody] Department dept)
        {
            try
            {
                //var category = JsonConvert.DeserializeObject<Category>(data);
                if (dept != null)
                {
                    Department department = new Department()
                    {

                        IsActive = true,
                        DepartmentName = dept.DepartmentName,
                        DepartmentDescription = dept.DepartmentDescription,
                        BranchId = dept.BranchId

                    };

                    _context.Departments.Add(department);
                     _context.SaveChanges();
                     
                  
                    return new JsonResult("Added");
                }

                return new JsonResult("SomeThing Went Wrong");
            }
            catch (Exception ex)
            {
                return new JsonResult(ex.Message);
            }


        }

        public async Task<IActionResult> OnPostDeleteBranch()
        {
            try
            {
                var branch = _context.Branches.Find(Branchid);
                if (branch != null)
                {
                    branch.IsActive = false;
                    branch.IsDeleted = true;
                    _context.Attach(branch).State = EntityState.Modified;
                    await _context.SaveChangesAsync();
                    _toastNotification.AddSuccessToastMessage("Deleted Successfully");
                }

                else
                {
                    _toastNotification.AddErrorToastMessage("Something went wrong");
                }
            }
            catch (Exception)

            {
               _toastNotification.AddErrorToastMessage("Something went wrong");
            }

            return RedirectToPage("Index");

        }
    }
}
