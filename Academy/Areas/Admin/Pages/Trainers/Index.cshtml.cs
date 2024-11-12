using Academy.Data;
using Academy.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using NToastNotify;

namespace Academy.Areas.Admin.Pages.Trainers
{
    public class IndexModel : PageModel
    {
        private readonly AcademyContext _context;
        [BindProperty]
        public int Trainerid { set; get; }
        private readonly IToastNotification _toastNotification;
        private readonly UserManager<ApplicationUser> _userManager;
        public IndexModel(AcademyContext context, IToastNotification toastNotification, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _toastNotification = toastNotification;
            _userManager = userManager;
        }
        public void OnGet()
        {

        }


        public async Task<IActionResult> OnPostDeleteParent()
        {
            try
            {
                var trainer = _context.Trainers.FirstOrDefault(t=>t.TrainerId==Trainerid);
                if (trainer != null)
                {
                    trainer.IsActive = false;
                    trainer.IsDeleted = true;
                    _context.Attach(trainer).State = EntityState.Modified;
                    var user = await _userManager.FindByNameAsync(trainer.TrainerEmail);
                    if (user != null)
                    {
                        var result = await _userManager.DeleteAsync(user);
                        if (result.Succeeded)
                        {

                            await _context.SaveChangesAsync();
                            _toastNotification.AddSuccessToastMessage("Deleted Successfully");
                            return RedirectToPage("Index");
                        }

                    }
                }


            }
            catch (Exception)

            {
                _toastNotification.AddErrorToastMessage("Something went wrong");
            }
            _toastNotification.AddErrorToastMessage("Something went wrong");
            return RedirectToPage("Index");

        }
    }
}
