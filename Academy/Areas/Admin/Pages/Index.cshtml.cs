using Academy.Data;
using Academy.Models;
using Academy.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Academy.Areas.Admin.Pages
{
    public class IndexModel : PageModel
    {
        private readonly AcademyContext _context;

        public HomeStatistcsVM HomeStatistcsVM { get; set; }


        public IndexModel(AcademyContext context)
        {
            _context = context;
            HomeStatistcsVM = new HomeStatistcsVM();
        }

        public void OnGet()
        {
            HomeStatistcsVM = new HomeStatistcsVM
            {
                BranchesCount = _context.Branches.Where(e=> e.IsActive && e.IsDeleted == false).Count(),
                TrainersCount = _context.Trainers.Where(e => e.IsActive && e.IsDeleted == false).Count(),
                TraineesCount = _context.Trainees.Where(e => e.IsActive && e.IsDeleted == false).Count(),
                ParentsCount = _context.Parents.Where(e => e.IsActive && e.IsDeleted == false).Count(),
                CompetitionTeamsCount = _context.CompetitionTeam.Where(e => e.IsActive && e.IsDeleted == false).Count(),
                ChampionsCount = _context.Champions.Where(e => e.IsDeleted == false).Count()
            };


        }
    }
}
