using Academy.Data;
using Academy.Models;
using DevExtreme.AspNet.Data;
using DevExtreme.AspNet.Mvc;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Academy.Areas.Admin.Pages.Branchs
{
    public class DepartmentsModel : PageModel
    {
        private readonly AcademyContext _context;



        public DepartmentsModel(AcademyContext context)
        {
            _context = context;

        }
        

        public  int BranchId { get; set; }



        public async Task<IActionResult> OnGetAsync(int id)
        {

            try
            {

               
                if (id != 0)
                {
                    BranchId = id;
                    return Page();
                }
                return RedirectToPage("../NotFound");
            }
            catch (Exception e)
            {
                return RedirectToPage("../Error");
            }



        }
        public IActionResult OnGetGridData(DataSourceLoadOptions loadOptions,int id)
        {
            List<Department> depts = new List<Department>();
            if (id != 0)
            {
                 depts = _context.Departments.Where(d => !d.IsDeleted && d.BranchId == id).ToList();
            }
         
            return new JsonResult(DataSourceLoader.Load(depts, loadOptions));
        }


    }
}
