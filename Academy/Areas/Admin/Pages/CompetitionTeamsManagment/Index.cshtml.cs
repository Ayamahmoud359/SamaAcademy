using Academy.Data;
using Academy.DTO;
using Academy.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using NToastNotify;

namespace Academy.Areas.Admin.Pages.CompetitionTeamsManagment
{
    public class IndexModel : PageModel
    {
        public CompetitionTeamVM  TeamVM{ set; get; }
        private readonly AcademyContext _context;
        [BindProperty]
        public int Teamid { set; get; }
        private readonly IToastNotification _toastNotification;
        private readonly IWebHostEnvironment _hostEnvironment;
        public List<Trainee> Trainees { get; set; }
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
            Trainees = _context.Trainees.Where(t => !t.IsDeleted && t.IsActive).ToList();

        }

        public async Task<IActionResult> OnPostAddTraineesAsync([FromBody] List<int> selectedValues,int? TeamId)
        {
            try
            {
                if (TeamId != null&&selectedValues!=null&&selectedValues.Count>0)
                {
                    var team = _context.CompetitionTeam.Find(TeamId);
                    if (team!= null && !team.IsActive)
                    {
                        return new JsonResult("Sorry ,You can't add new subscription in this team as This team isn't Active");
                    }
                    var teamsubscriptions = _context.TraineeCompetitionTeams.Where(a =>!a.IsDeleted&&a.IsActive&& a.CompetitionTeamId == TeamId);
                    var traineecomptitionTeams = new List<TraineeCompetitionTeam>();
                      foreach(var item in selectedValues)
                    {
                        if (!teamsubscriptions.Any(a => a.TraineeId == item)){
                            traineecomptitionTeams.Add(new TraineeCompetitionTeam()
                            {
                                TraineeId = item,
                                CompetitionTeamId = (int)TeamId,
                                IsActive = true
                            });
                        }

                    }
                    _context.TraineeCompetitionTeams.AddRangeAsync(traineecomptitionTeams);
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

        public async Task<IActionResult> OnPostDeleteTeam()
        {
            try
            {

                var team = _context.CompetitionTeam.Find(Teamid);
                if (team != null)
                {
                    List<TraineeCompetitionTeam> traineeCompetitionTeams = new List<TraineeCompetitionTeam>();
                    traineeCompetitionTeams = _context.TraineeCompetitionTeams.Include(a => a.CompetitionTeam)
                       
                       .Where(a => a.CompetitionTeam.Id==Teamid).ToList();
                    foreach (var item in traineeCompetitionTeams)
                    {
                        item.IsActive = false;
                        item.IsDeleted = true;
                    }
                   

                    team.IsActive = false;
                    team.IsDeleted = true;

                    _context.Attach(team).State = EntityState.Modified;
                    await _context.SaveChangesAsync();
                    _toastNotification.AddSuccessToastMessage("Deleted Successfully");
                    return RedirectToPage("Index");
                }


            }
            catch (Exception)

            {

            }
            _toastNotification.AddErrorToastMessage("Something went wrong");

            return RedirectToPage("Index");

        }

        private async Task<string> UploadImage(string folderPath, IFormFile file)
        {

            folderPath += Guid.NewGuid().ToString() + "_" + file.FileName;

            string serverFolder = Path.Combine(_hostEnvironment.WebRootPath, folderPath);

            var directory = Path.GetDirectoryName(serverFolder);
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            await file.CopyToAsync(new FileStream(serverFolder, FileMode.Create));

            return folderPath;
        }
    }
}
