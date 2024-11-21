using Academy.Data;
using Academy.DataGridVM;
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
            List<CompetitionTeamDataGridVM> data = new List<CompetitionTeamDataGridVM>();
            if (id != 0)
            {
                data = _context.CompetitionTeam.Include(a=>a.Trainer)
                    .Where(d => !d.IsDeleted
                    && d.CompetitionDepartmentId == id).Select(i => new CompetitionTeamDataGridVM()
                    {
                        Id = i.Id,
                  
                        TeamName = i.Name,
                        TrainerName = i.Trainer.IsDeleted ? "" : i.Trainer.TrainerName,
                        IsActive = i.IsActive
                       

                    }).ToList();
            }

            return new JsonResult(DataSourceLoader.Load(data, loadOptions));
        }
    }
}
