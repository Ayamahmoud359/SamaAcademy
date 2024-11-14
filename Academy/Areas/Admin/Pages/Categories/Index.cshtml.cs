using Academy.Data;
using Academy.DTO;
using Academy.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using NToastNotify;

namespace Academy.Areas.Admin.Pages.Categories
{
    public class IndexModel : PageModel
    {
       
        private readonly AcademyContext _context;
        [BindProperty]
        public int Catid { set; get; }
        private readonly IToastNotification _toastNotification;
        public IndexModel(AcademyContext context, IToastNotification toastNotification)
        {
            _context = context;
            _toastNotification = toastNotification;
        }
        public void OnGet()
        {

        }

     
        public async Task<IActionResult> OnPostDeleteCategory()
        {
            try
            {
                var category = _context.Categories.Find(Catid);
                if (category != null)
                {
                    List<Absence> Absences = new List<Absence>();
                    Absences = _context.Abscenses.Include(a => a.Subscription)
                       .ThenInclude(a => a.Category)
                       
                       .Where(a => a.Subscription.Category.CategoryId==Catid).ToList();
                    foreach (var item in Absences)
                    {
                        item.IsDeleted = true;
                    }
                    List<Exam> Exams = new List<Exam>();
                    Exams = _context.Exams.Include(a => a.Subscription)
                       .ThenInclude(a => a.Category)
                       
                       .Where(a => a.Subscription.Category.CategoryId ==Catid).ToList();
                    foreach (var item in Exams)
                    {
                        item.IsDeleted = true;
                    }
                    List<Subscription> subscriptions = new List<Subscription>();
                    subscriptions = _context.Subscriptions.Include(a => a.Category)

                       .Where(a => a.Category.CategoryId==Catid).ToList();
                    foreach (var item in subscriptions)
                    {
                        item.IsDeleted = true;
                        item.IsActive = false;
                    }
                    List<TrainerCategories> trainerCategories = new List<TrainerCategories>();
                    trainerCategories = _context.CategoryTrainers.Include(a => a.Category)

                        .Where(a => a.Category.CategoryId==Catid).ToList();
                    foreach (var item in trainerCategories)
                    {
                        item.IsDeleted = true;
                        item.IsActive = false;
                    }
                    category.IsActive = false;
                    category.IsDeleted = true;

                
                    _context.Attach(category).State = EntityState.Modified;
                    await _context.SaveChangesAsync();
                    _toastNotification.AddSuccessToastMessage("Deleted Successfull");
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
