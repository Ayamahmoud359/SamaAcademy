using Academy.Data;
using Academy.DataGridVM;
using Academy.Models;
using DevExtreme.AspNet.Data;
using DevExtreme.AspNet.Mvc;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace Academy.Areas.Admin.Pages.CompetitionTeamsManagment
{
    public class competitionTeamTraineesModel : PageModel
    {
        private readonly AcademyContext _context;



        public competitionTeamTraineesModel(AcademyContext context)
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
            List<TraineeCompetitionTeamDataGridVM> data = new List<TraineeCompetitionTeamDataGridVM>();
            if (id != 0)
            {
                data = _context.TraineeCompetitionTeams.Include(a => a.Trainee)
                 .Include(a=>a.CompetitionTeam)
                 .ThenInclude(a=>a.CompetitionDepartment)
                  .Include(a => a.CompetitionTeam)
                 .ThenInclude(a => a.Trainer)
                    .Where(d => !d.IsDeleted
                && !d.Trainee.IsDeleted
                //&&  d.Trainee.IsActive
                && d.CompetitionTeamId== id).Select(i => new TraineeCompetitionTeamDataGridVM()
                {
                    Id = i.Id,
                
                    TraineeName = i.Trainee.TraineeName,
                   
                    Trainer = i.CompetitionTeam.Trainer.IsDeleted ? "" : i.CompetitionTeam.Trainer.TrainerName,
                    CompetitionDepartment = i.CompetitionTeam.CompetitionDepartment.Name,
                    IsActive = i.IsActive,

                }).ToList();
            }

            return new JsonResult(DataSourceLoader.Load(data, loadOptions));
        }

    }
}
