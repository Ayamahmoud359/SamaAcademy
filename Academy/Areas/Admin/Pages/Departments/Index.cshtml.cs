using Academy.Data;
using Academy.DTO;
using Academy.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using static Azure.Core.HttpHeader;

namespace Academy.Areas.Admin.Pages.Departments
{
    public class IndexModel : PageModel
    {
        [BindProperty]
        public Category cat { set; get; }
        private readonly AcademyContext _context;
        [BindProperty]
        public int deptid { set; get; }
        public IndexModel(AcademyContext context)
        {
            _context = context;

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

                    category.IsActive = true;
                    _context.Categories.Add(category);
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
                    _context.Attach(dept).State = EntityState.Modified;
                    await _context.SaveChangesAsync();
                    
                }
            }
            catch (Exception)

            {
               // _toastNotification.AddErrorToastMessage("Something went wrong");
            }

            return RedirectToPage("Index");

        }
    }
}
