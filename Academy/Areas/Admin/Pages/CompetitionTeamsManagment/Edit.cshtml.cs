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

        public EditModel(AcademyContext context, IToastNotification toastNotification)
        {
            CompetitionTeamVM = new CompetitionTeamVM();
            _context = context;
            _toastNotification = toastNotification;
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
                Trainees = _context.Trainees.Where(t => !t.IsDeleted && t.IsActive).ToList();
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
        public async Task<IActionResult> OnPostAsync()
        {

            try
            {
                CompetitionDepartments = _context.CompetitionDepartment.Where(b => !b.IsDeleted && b.IsActive).ToList();
                Trainers = _context.Trainers.Where(t => !t.IsDeleted && t.IsActive).ToList();
                Trainees = _context.Trainees.Where(t => !t.IsDeleted && t.IsActive).ToList();

                var competitionTeam = await _context.CompetitionTeam.FirstOrDefaultAsync(m => m.Id == CompetitionTeamVM.Id);
                if (competitionTeam != null)
                {
                   competitionTeam.Name = CompetitionTeamVM.Name;
                    competitionTeam.CompetitionDepartmentId = CompetitionTeamVM.CompetitionDepartmentId;
                    competitionTeam.TrainerId = CompetitionTeamVM.TrainerId;
                    competitionTeam.IsActive = CompetitionTeamVM.IsActive;
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
    }
}