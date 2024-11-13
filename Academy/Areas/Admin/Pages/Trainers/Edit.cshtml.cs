using Academy.Data;
using Academy.DTO;
using Academy.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Academy.Areas.Admin.Pages.Trainers
{
    public class EditModel : PageModel
    {
        [BindProperty]
        public Trainer Trainer { get; set; }
       
        

        private readonly AcademyContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        public EditModel(AcademyContext context)
        {
            _context = context;
        
        }


        public async Task<IActionResult> OnGetAsync(int? id) { 
            try
            {
             Trainer=  _context.Trainers.FirstOrDefault(t => t.TrainerId == id);
                if (Trainer!= null)
                {
                    //List<TrainerCategories> assignedCategories = new List<TrainerCategories>();
                    //assignedCategories = _context.CategoryTrainers.Include(e => e.Trainer)
                    //    .Include(e => e.Category).ThenInclude(e => e.Department)
                    //    .ThenInclude(e => e.Branch).Where(c =>
                    //!c.IsDeleted && c.IsActive
                    //&& !c.Category.IsDeleted
                    //&& !c.Category.Department.IsDeleted
                    //&& !c.Category.Department.Branch.IsDeleted
                    //&& !c.Trainer.IsDeleted
                    //&& c.Category.IsActive
                    //&& c.Category.Department.IsActive
                    //&& c.Category.Department.Branch.IsActive
                    //&& c.Trainer.IsActive &&
                    //c.TrainerId == trainer.TrainerId &&
                    //c.Category.Department.DepartmentId == trainer.CurrentDepartment
                    //&& c.Category.Department.Branch.BranchId == trainer.CurrentBranch).ToList();
                  
                    return Page();
                }

                return RedirectToPage("../NotFound");
            }
            catch
            {
                return RedirectToPage("../Error");
            }
         
            
        }


        public async Task<IActionResult> OnPostAsync()
        {
           
            if (!ModelState.IsValid)
            {
                return Page();
            }
            try
            {
                var TrainerToEdit = await _context.Trainers.FirstOrDefaultAsync(m => m.TrainerId == Trainer.TrainerId);
                TrainerToEdit.TrainerName = Trainer.TrainerName;
                TrainerToEdit.TrainerAddress = Trainer.TrainerAddress;
                TrainerToEdit.TrainerPhone = Trainer.TrainerPhone;
                TrainerToEdit.HiringDate = Trainer.HiringDate;
                TrainerToEdit.IsActive = Trainer.IsActive;
                _context.Attach(TrainerToEdit).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                return Redirect("Index");

            }
            catch (Exception e)
            {
                ModelState.AddModelError(string.Empty, e.Message);
                return Page();
            }

        }




    }
}
