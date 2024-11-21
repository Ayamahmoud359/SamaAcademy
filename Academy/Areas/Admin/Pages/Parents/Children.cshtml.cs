using Academy.Data;
using Academy.Models;
using DevExtreme.AspNet.Data;
using DevExtreme.AspNet.Mvc;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Academy.Areas.Admin.Pages.Parents
{
    public class ChildrenModel : PageModel
    {
        private readonly AcademyContext _context;



        public ChildrenModel(AcademyContext context)
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
            List<Trainee> data = new List<Trainee>();
            if (id != 0)
            {
                data = _context.Trainees.Where(d => !d.IsDeleted && d.ParentId == id).ToList();
            }

            return new JsonResult(DataSourceLoader.Load(data, loadOptions));
        }
    }
}
