using Academy.Data;
using Academy.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace Academy.Areas.Admin.Pages.TraineeCompetitionTeamsManagment
{
    public class ViewModel : PageModel
    {
      

            private readonly AcademyContext _context;

            public TraineeCompetitionTeam traineeCompetitionTeam{ get; set; }

            public ViewModel(AcademyContext context)
            {
                _context = context;
                traineeCompetitionTeam = new TraineeCompetitionTeam();

            }
            public async Task<IActionResult> OnGetAsync(int? id)
            {

                try
                {

                   traineeCompetitionTeam = await _context.TraineeCompetitionTeams.Include(a => a.Trainee)
                        .Include(a => a.CompetitionTeam)
                        .ThenInclude(a => a.CompetitionDepartment)
                        .Include(a => a.CompetitionTeam)
                        .ThenInclude(a => a.Trainer)
                         .FirstOrDefaultAsync(m => m.Id == id);
                    if (traineeCompetitionTeam != null)
                    {

                        return Page();
                    }
                    return RedirectToPage("../NotFound");
                }
                catch (Exception e)
                {
                    return RedirectToPage("../Error");
                }
            }
        }
}
