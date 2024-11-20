using Academy.Data;
using Academy.DTO;
using Academy.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using NToastNotify;
using System.ComponentModel.DataAnnotations;

namespace Academy.Areas.Admin.Pages.CompetitionTeamsManagment
{
    public class AddModel : PageModel
    {
        [BindProperty]
        public CompetitionTeamVM CompetitionTeamVM{ get; set; }




        public List<CompetitionDepartment> CompetitionDepartments{ get; set; }
        public List<Trainer> Trainers{ get; set; }
        public List<Trainee> Trainees{ get; set; }

        private readonly AcademyContext _context;
       
        private readonly IToastNotification _toastNotification;
        private readonly IWebHostEnvironment _hostEnvironment;

        public AddModel(AcademyContext context
           
            , IToastNotification toastNotification
            , IWebHostEnvironment hostEnvironment)
        {
           CompetitionTeamVM = new CompetitionTeamVM();
            _context = context;
           
            _toastNotification = toastNotification;
            _hostEnvironment = hostEnvironment;
        }


        public void OnGet()
        {
           CompetitionDepartments= _context.CompetitionDepartment.Where(b => !b.IsDeleted && b.IsActive).ToList();
            Trainers = _context.Trainers.Where(t=>!t.IsDeleted&&t.IsActive).ToList();
            Trainees = _context.Trainees.Where(t => !t.IsDeleted && t.IsActive).ToList();
        }


        public async Task<IActionResult> OnPostAsync(IFormFile? fileUpload)
        {
            CompetitionDepartments = _context.CompetitionDepartment.Where(b => !b.IsDeleted && b.IsActive).ToList();
            Trainers = _context.Trainers.Where(t => !t.IsDeleted && t.IsActive).ToList();
            Trainees = _context.Trainees.Where(t => !t.IsDeleted && t.IsActive).ToList();
            if (!ModelState.IsValid)
            {
                _toastNotification.AddErrorToastMessage("SomeThing Went Wrong");
                return Page();
            }
            try
            {
                var Team = new CompetitionTeam()
                {
                    Name = CompetitionTeamVM.Name,
                    IsActive = true,
                    CompetitionDepartmentId = CompetitionTeamVM.CompetitionDepartmentId,
                    TrainerId = CompetitionTeamVM.TrainerId
                };
                if (fileUpload != null && fileUpload.Length > 0)
                {
                    string folder = "uploads/CompetitionTeams/";
                    Team.Image = await UploadImage(folder, fileUpload);

                }
              
               

                if (CompetitionTeamVM.SelectedTrainees != null && CompetitionTeamVM.SelectedTrainees.Count != 0)
                {
                    foreach (var item in CompetitionTeamVM.SelectedTrainees)
                    {
                        Team.traineeCompetitionTeams.Add(new TraineeCompetitionTeam()
                        {
                        TraineeId=item,
                            IsActive = true
                        });
                    }

                  
                }


                _context.CompetitionTeam.Add(Team);
                await _context.SaveChangesAsync();

                _toastNotification.AddSuccessToastMessage("Team Added Successfully");
                return RedirectToPage("Index");




            }
            catch (Exception e)
            {
                ModelState.AddModelError(string.Empty, e.Message);
                _toastNotification.AddErrorToastMessage("SomeThing Went Wrong");
                return Page();

            }
           

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
