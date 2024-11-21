using Academy.Data;
using Academy.Models;
using DevExtreme.AspNet.Data;
using DevExtreme.AspNet.Mvc;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace Academy.Areas.Admin.Pages.CompetitionDepartmentsManagment
{
    public class CompetitionTeamsModel : PageModel
    {
        private readonly AcademyContext _context;



        public CompetitionTeamsModel(AcademyContext context)
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
            List<CompetitionTeam> data = new List<CompetitionTeam>();
            if (id != 0)
            {
                data = _context.CompetitionTeam.Include(a=>a.Trainer).Where(d => !d.IsDeleted && d.CompetitionDepartmentId == id).ToList();
            }

            return new JsonResult(DataSourceLoader.Load(data, loadOptions));
        }
    }
}
