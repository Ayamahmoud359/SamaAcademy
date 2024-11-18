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
        private readonly ApplicationDbContext _applicationDb;
        public EditModel(AcademyContext context
            ,IToastNotification toastNotification
              , ApplicationDbContext applicationDb
            , UserManager<ApplicationUser> userManager)
        {
            _context = context;
           _toastNotification = toastNotification;
            _applicationDb = applicationDb;
            _userManager = userManager;
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
                if (Trainer.UserName!= TrainerToEdit.UserName)
                {
                    var checkuser = await _userManager.FindByNameAsync(Trainer.UserName);
                    if (checkuser != null)
                    {
                        ModelState.AddModelError(string.Empty, "User Name alredy Token");
                        _toastNotification.AddErrorToastMessage("SomeThing Went Wrong");
                        return Page();
                    }
                    var ExistedUser = await _userManager.FindByNameAsync(TrainerToEdit.UserName);
                    if (ExistedUser != null)
                    {
                        ExistedUser.UserName = Trainer.UserName;
                        var result = await _userManager.UpdateAsync(ExistedUser);
                        if (result.Succeeded)
                        {
                            TrainerToEdit.UserName = Trainer.UserName;

                        }
                        else
                        {
                            foreach (var error in result.Errors)
                            {
                                ModelState.AddModelError(string.Empty, error.Description);
                            }
                            _toastNotification.AddErrorToastMessage("SomeThing Went Wrong");
                            return Page();
                        }
                    }

                }
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
