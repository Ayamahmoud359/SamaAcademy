using Academy.Data;
using Academy.Models;
using DevExtreme.AspNet.Data;
using DevExtreme.AspNet.Mvc;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Academy.Areas.Admin.Pages.Departments
{
    public class DepartmentCategoriesModel : PageModel
    {
        private readonly AcademyContext _context;



        public DepartmentCategoriesModel(AcademyContext context)
        {
            _context = context;

        }


        public int Id { get; set; }



        public async Task<IActionResult> OnGetAsync(int id)
        {

            try
            {


                if (id != 0)
                {
                    Id = id;
                    return Page();
                }
                return RedirectToPage("../NotFound");
            }
            catch (Exception e)
            {
                return RedirectToPage("../Error");
            }



        }
        public IActionResult OnGetGridData(DataSourceLoadOptions loadOptions, int id)
        {
            List<Category> data = new List<Category>();
            if (id != 0)
            {
                data= _context.Categories.Where(d => !d.IsDeleted && d.DepartmentId == id).ToList();
            }

            return new JsonResult(DataSourceLoader.Load(data, loadOptions));
        }
    }
}
