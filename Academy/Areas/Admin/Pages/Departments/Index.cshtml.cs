using Academy.Data;
using Academy.DTO;
using Academy.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using NToastNotify;
using static Azure.Core.HttpHeader;

namespace Academy.Areas.Admin.Pages.Departments
{
    public class IndexModel : PageModel
    {
      
        public CategoryVM cat { set; get; }
        private readonly AcademyContext _context;
        [BindProperty]
        public int deptid { set; get; }
        private readonly IToastNotification _toastNotification;
        public IndexModel(AcademyContext context , IToastNotification toastNotification)
        {
            _context = context;
            _toastNotification = toastNotification;
        }
        public void OnGet()
        {

        }

        public IActionResult OnPostAddCategory([FromBody] Category category)
        {
            try
            {
                //var category = JsonConvert.DeserializeObject<Category>(data);
                if (category != null)
                {
                    var department = _context.Departments.Include(d=>d.Branch).FirstOrDefault(d=>d.DepartmentId==category.DepartmentId);
                    if (department != null && (!department.IsActive||!department.Branch.IsActive))
                    {
                        return new JsonResult("Sorry ,You can't add new Category in this Department as This Department isn't Active or in Branch isn't Active");
                    }
                    Category newCategory = new Category()
                    {
                        CategoryName=category.CategoryName,
                        CategoryDescription=category.CategoryDescription,
                        DepartmentId=category.DepartmentId,
                        IsActive=true

                    };

                    _context.Categories.Add(newCategory);
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

        public async Task<IActionResult> OnPostDeleteDepartment()
        {
            try
            {
                var dept = _context.Departments.Find(deptid);
                if (dept != null)
                {
                    dept.IsActive = false;
                    dept.IsDeleted=true;
                    _context.Attach(dept).State = EntityState.Modified;
                    await _context.SaveChangesAsync();
                    _toastNotification.AddSuccessToastMessage("Deleted Successfull");
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
