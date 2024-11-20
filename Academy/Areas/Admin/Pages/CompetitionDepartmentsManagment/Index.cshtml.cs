using Academy.Data;
using Academy.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using NToastNotify;

namespace Academy.Areas.Admin.Pages.CompetitionDepartmentsManagment
{
    public class IndexModel : PageModel
    {
     
        private readonly AcademyContext _context;
        [BindProperty]
        public int CompetitionDepartmentid { set; get; }
        private readonly IToastNotification _toastNotification;
        private readonly IWebHostEnvironment _hostEnvironment;

        public IndexModel(AcademyContext context
            , IToastNotification toastNotification
            , IWebHostEnvironment hostEnvironment)
        {
            _context = context;
            _toastNotification = toastNotification;
            _hostEnvironment = hostEnvironment;
        }
        public void OnGet()
        {

        }

        public async Task<IActionResult> OnPostDeleteCompetitiondepartment()
        {
            try
            {
                var competitiondept = _context.CompetitionDepartment.Find(CompetitionDepartmentid);
                if (competitiondept != null)
                {
                    List<TraineeCompetitionTeam> traineeCompetitionTeams = new List<TraineeCompetitionTeam>();
                    traineeCompetitionTeams = _context.TraineeCompetitionTeams.Include(a => a.CompetitionTeam)
                        .ThenInclude(e=>e.CompetitionDepartment)

                       .Where(a => a.CompetitionTeam.CompetitionDepartment.Id == CompetitionDepartmentid).ToList();
                    foreach (var item in traineeCompetitionTeams)
                    {
                        item.IsActive = false;
                        item.IsDeleted = true;
                    }
                    List<CompetitionTeam> competitionTeams = new List<CompetitionTeam>();
                    competitionTeams = _context.CompetitionTeam.Include(a => a.CompetitionDepartment)
                       
                       .Where(a => a.CompetitionDepartment.Id == CompetitionDepartmentid).ToList();
                    foreach (var item in traineeCompetitionTeams)
                    {
                        item.IsActive = false;
                        item.IsDeleted = true;
                    }
                    competitiondept.IsDeleted = true;
                    competitiondept.IsActive = false;

                    _context.Attach(competitiondept).State = EntityState.Modified;
                    await _context.SaveChangesAsync();
                    _toastNotification.AddSuccessToastMessage("Deleted Successfull");

                    return RedirectToPage("Index");
                }

            }
            catch (Exception)

            {

            }

            _toastNotification.AddErrorToastMessage("Something went wrong");

            return RedirectToPage("Index");

        }

      
    }
}
