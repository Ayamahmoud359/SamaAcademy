using Academy.Data;
using Academy.DTO;
using Academy.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using NToastNotify;
using System.ComponentModel.DataAnnotations;

namespace Academy.Areas.Admin.Pages.Trainers
{
    public class EditModel : PageModel
    {
        [BindProperty]
        public Trainer Trainer { get; set; }
       
        

        private readonly AcademyContext _context;
        private readonly IToastNotification _toastNotification;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        public EditModel(AcademyContext context,IToastNotification toastNotification)
        {
            _context = context;
           _toastNotification = toastNotification;
        }


        public async Task<IActionResult> OnGetAsync(int? id) { 
            try
            {
             Trainer=  _context.Trainers.FirstOrDefault(t => t.TrainerId == id);
                if (Trainer!= null)
                {
                    
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
                _toastNotification.AddSuccessToastMessage("Trainer Information Edited Suuccessfully");
                return Redirect("Index");

            }
            catch (Exception e)
            {
                ModelState.AddModelError(string.Empty, e.Message);
                _toastNotification.AddErrorToastMessage("SomeThing Went Wrong");
                return Page();
            }

        }




    }
}
