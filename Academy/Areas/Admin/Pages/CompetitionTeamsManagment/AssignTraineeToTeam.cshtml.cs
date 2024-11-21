using Academy.Data;
using Academy.DTO;
using Academy.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using NToastNotify;

namespace Academy.Areas.Admin.Pages.CompetitionTeamsManagment
{
    public class AssignTraineeToTeamModel : PageModel
    {

        [BindProperty]
        public CompetitionTeamVM TeamVM { set; get; }
        private readonly AcademyContext _context;
        private readonly IToastNotification _toastNotification;
        public List<Trainee> Trainees { get; set; }
        public AssignTraineeToTeamModel(AcademyContext context, IToastNotification toastNotification)
        {
            _context = context;
            _toastNotification = toastNotification;
            TeamVM = new CompetitionTeamVM();

        }
        public async Task<IActionResult> OnGet(int id)
        {
            try
            {

                if (id != 0)
                {

                    Trainees = _context.Trainees.Include(a => a.Parent).Where(t => !t.IsDeleted && t.IsActive && !t.Parent.IsDeleted && t.Parent.IsActive).ToList();
                    TeamVM.Id= id;
                  
                    return Page();
                }
                return RedirectToPage("../NotFound");
            }
            catch (Exception e)
            {
                return RedirectToPage("../Error");
            }

        }
        public async Task<IActionResult> OnPost()
        {
            try
            {

                Trainees = _context.Trainees.Include(a => a.Parent).Where(t => !t.IsDeleted && t.IsActive && !t.Parent.IsDeleted && t.Parent.IsActive).ToList();
                if (TeamVM.Id != 0 && TeamVM.SelectedTrainees != null &&TeamVM.SelectedTrainees.Count > 0)
                {
                    var team = _context.CompetitionTeam.Include(a => a.CompetitionDepartment).FirstOrDefault(a => a.Id == TeamVM.Id);
                    if (team != null && (!team.IsActive || !team.CompetitionDepartment.IsActive))
                    {
                        ModelState.AddModelError(string.Empty, "Sorry ,You can't add new subscription in this team as This team isn't Active or in competition Department Not Active");
                        _toastNotification.AddErrorToastMessage("This team isn't Active or in competition Department Not Active");
                        return Page();
                      
                    }
                    var teamsubscriptions = _context.TraineeCompetitionTeams
                        .Include(a=>a.Trainee)
                        .Where(a => !a.IsDeleted && a.IsActive
                         &&!a.Trainee.IsDeleted
                         &&a.Trainee.IsActive
                         && a.CompetitionTeamId == TeamVM.Id
                    );
                    var traineecomptitionTeams = new List<TraineeCompetitionTeam>();
                    foreach (var item in TeamVM.SelectedTrainees)
                    {
                        if (!teamsubscriptions.Any(a => a.TraineeId == item))
                        {
                            traineecomptitionTeams.Add(new TraineeCompetitionTeam()
                            {
                                TraineeId = item,
                                CompetitionTeamId = TeamVM.Id,
                                IsActive = true
                            });
                        }

                    }
                    await _context.TraineeCompetitionTeams.AddRangeAsync(traineecomptitionTeams);
                    _context.SaveChanges();

                    _toastNotification.AddSuccessToastMessage("Trainees Is Assigned To Team Successfully");
                    return Redirect("~/Admin/TraineeCompetitionTeamsManagment/Index");
                }

                _toastNotification.AddErrorToastMessage("SomeThing Went Wrong");
                return Page();
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                _toastNotification.AddErrorToastMessage("SomeThing Went Wrong");
                return Page();
            }


        }

    }
}
