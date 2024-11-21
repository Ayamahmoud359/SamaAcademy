using Academy.Data;
using Academy.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace Academy.Areas.Admin.Pages.Trainers
{
    public class ViewModel : PageModel
    {
        private readonly AcademyContext _context;

        public Trainer Trainer { get; set; }
        public string CurrentBranch { get; set; }
        public string CurrentDepartment { get; set; }
        public ViewModel(AcademyContext context)
        {
            _context = context;
           Trainer = new Trainer();
        }
        public async Task<IActionResult> OnGetAsync(int? id)
        {

            try
            {

                Trainer = await _context.Trainers.FirstOrDefaultAsync(m => m.TrainerId== id);
                if (Trainer != null)
                {
                    
                    CurrentBranch = _context.Branches.FirstOrDefault(a => a.BranchId == Trainer.CurrentBranch).BranchName;
                    CurrentDepartment = _context.Departments.FirstOrDefault(a => a.DepartmentId == Trainer.CurrentDepartment).DepartmentName;
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
