using Academy.Data;
using Academy.DTO;
using Academy.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using NToastNotify;

namespace Academy.Areas.Admin.Pages.CompetitionTeamsManagment
{
    public class EditModel : PageModel
    {
        private readonly AcademyContext _context;
        private readonly IToastNotification _toastNotification;
        private readonly IWebHostEnvironment _hostEnvironment;

        public EditModel(AcademyContext context
            , IToastNotification toastNotification
             , IWebHostEnvironment hostEnvironment)
        {
            CompetitionTeamVM = new CompetitionTeamVM();
            _context = context;
            _toastNotification = toastNotification;
            _hostEnvironment = hostEnvironment;
        }
        [BindProperty]
        public CompetitionTeamVM CompetitionTeamVM { get; set; }




        public List<CompetitionDepartment> CompetitionDepartments { get; set; }
        public List<Trainer> Trainers { get; set; }
        public List<Trainee> Trainees { get; set; }
        public async Task<IActionResult> OnGetAsync(int? id)
        {


            try
            {

                CompetitionDepartments = _context.CompetitionDepartment.Where(b => !b.IsDeleted && b.IsActive).ToList();
                Trainers = _context.Trainers.Where(t => !t.IsDeleted && t.IsActive).ToList();
                Trainees = _context.Trainees.Include(a => a.Parent).Where(t => !t.IsDeleted && t.IsActive && !t.Parent.IsDeleted && t.Parent.IsActive).ToList();
                var competitionTeam = await _context.CompetitionTeam.FirstOrDefaultAsync(m => m.Id == id);
                if (competitionTeam != null)
                {
                    CompetitionTeamVM.Name = competitionTeam.Name;
                    CompetitionTeamVM.Id = competitionTeam.Id;
                    CompetitionTeamVM.CompetitionDepartmentId = competitionTeam.CompetitionDepartmentId;
                    CompetitionTeamVM.TrainerId = competitionTeam.TrainerId;
                    CompetitionTeamVM.IsActive = competitionTeam.IsActive;
                    CompetitionTeamVM.Image = competitionTeam.Image;
                    return Page();
                }
                return RedirectToPage("../NotFound");
            }
            catch (Exception e)
            {
                return RedirectToPage("../Error");
            }



        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync(IFormFile? fileUpload)
        {

            try
            {
                CompetitionDepartments = _context.CompetitionDepartment.Where(b => !b.IsDeleted && b.IsActive).ToList();
                Trainers = _context.Trainers.Where(t => !t.IsDeleted && t.IsActive).ToList();
                Trainees = _context.Trainees.Include(a => a.Parent).Where(t => !t.IsDeleted && t.IsActive && !t.Parent.IsDeleted && t.Parent.IsActive).ToList();
                var competitionTeam = await _context.CompetitionTeam.FirstOrDefaultAsync(m => m.Id == CompetitionTeamVM.Id);
                if (competitionTeam != null)
                {
                   competitionTeam.Name = CompetitionTeamVM.Name;
                    competitionTeam.CompetitionDepartmentId = CompetitionTeamVM.CompetitionDepartmentId;
                    competitionTeam.TrainerId = CompetitionTeamVM.TrainerId;
                    competitionTeam.IsActive = CompetitionTeamVM.IsActive;
                    if (fileUpload != null && fileUpload.Length > 0)
                    {
                        //if (competitionTeam.Image != null)
                        //{
                        //    var ImagePath = Path.Combine(_hostEnvironment.WebRootPath, competitionTeam.Image);
                        //    if (System.IO.File.Exists(ImagePath))
                        //    {
                        //        System.IO.File.Delete(ImagePath);
                        //    }
                        //}
                        string folder = "uploads/CompetitionDepartments/";
                        competitionTeam.Image = await UploadImage(folder, fileUpload);

                    }
                    _context.Attach(competitionTeam).State = EntityState.Modified;
                    await _context.SaveChangesAsync();
                    _toastNotification.AddSuccessToastMessage("Competition Team Information Edited Successfully");
                    return RedirectToPage("Index");
                }
                ModelState.AddModelError(string.Empty, "SomeThing Went Wrong");


            }
            catch (Exception e)
            {
                ModelState.AddModelError(string.Empty, e.Message);

            }
            _toastNotification.AddErrorToastMessage("Something Went Wrong");
            return Page();
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
