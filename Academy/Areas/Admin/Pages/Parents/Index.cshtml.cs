using Academy.Data;
using Academy.DTO;
using Academy.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using NToastNotify;

namespace Academy.Areas.Admin.Pages.Parents
{
    public class IndexModel : PageModel
    {
   
       
        private readonly AcademyContext _context;

        [BindProperty]
        public int Parentid { set; get; }
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
                var parent = _context.Parents.Find(Parentid);
                if (parent != null)
                {
                    List<Absence> Absences = new List<Absence>();
                    Absences = _context.Abscenses.Include(a => a.Subscription)
                       .ThenInclude(a => a.Trainee).ThenInclude(a=>a.Parent)
                       .Where(a => a.Subscription.Trainee.Parent.ParentId == Parentid).ToList();
                    foreach (var item in Absences)
                    {
                        item.IsDeleted = true;
                    }
                    List<Exam> Exams = new List<Exam>();
                    Exams = _context.Exams.Include(a => a.Subscription)
                       .ThenInclude(a => a.Trainee)
                       .ThenInclude(a=>a.Parent)
                       .Where(a => a.Subscription.Trainee.Parent.ParentId == Parentid).ToList();
                    foreach (var item in Exams)
                    {
                        item.IsDeleted = true;
                    }
                    List<Subscription> subscriptions = new List<Subscription>();
                    subscriptions = _context.Subscriptions.Include(a =>a.Trainee)
                        .ThenInclude(a=>a.Parent)
                       .Where(a => a.Trainee.Parent.ParentId == Parentid).ToList();
                    foreach (var item in subscriptions)
                    {
                        item.IsDeleted = true;
                        item.IsActive = false;
                    }


                    List<TraineeChampion> traineeChampions = new List<TraineeChampion>();
                    traineeChampions = _context.TraineeChampions.Include(a => a.Trainee)
                        .ThenInclude(a=>a.Parent)
                        .Where(a => a.Trainee.Parent.ParentId == Parentid).ToList();
                    foreach (var item in traineeChampions)
                    {
                        item.IsDeleted = true;
                        item.IsActive = false;
                    }
                    List<Trainee> trainees = new List<Trainee>();
                    trainees = _context.Trainees.Include(a => a.Parent)
                        .Where(a => a.Parent.ParentId == Parentid).ToList();
                    foreach (var item in trainees)
                    {
                        item.IsDeleted = true;
                        item.IsActive = false;
                    }
                    parent.IsActive = false;
                    parent.IsDeleted = true;
                    _context.Attach(parent).State = EntityState.Modified;
                    await _context.SaveChangesAsync();
                    _toastNotification.AddSuccessToastMessage("Deleted Successfully");
                    return RedirectToPage("Index");

                    //var user = await _userManager.FindByNameAsync(parent.UserName);
                    //if (user != null)
                    //{
                    //    var result = await _userManager.DeleteAsync(user);
                    //    if (result.Succeeded)
                    //    {

                    //        await _context.SaveChangesAsync();
                    //        _toastNotification.AddSuccessToastMessage("Deleted Successfully");
                    //        return RedirectToPage("Index");
                    //    }

                    //}

                }

              
            }
            catch (Exception)

            {
            }
            _toastNotification.AddErrorToastMessage("Something went wrong");
            return RedirectToPage("Index");

        }
    }
}
