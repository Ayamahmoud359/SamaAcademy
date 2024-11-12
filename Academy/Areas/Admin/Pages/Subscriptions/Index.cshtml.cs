using Academy.Data;
using Academy.DTO;
using Academy.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using NToastNotify;

namespace Academy.Areas.Admin.Pages.Subscriptions
{
    public class IndexModel : PageModel
    {
      
        private readonly AcademyContext _context;
        [BindProperty]
        public int Subscriptionid { set; get; }
        private readonly IToastNotification _toastNotification;
      
        public IndexModel(AcademyContext context, IToastNotification toastNotification)
        {
            _context = context;
            _toastNotification = toastNotification;
        }
       
        public async Task<IActionResult> OnPostDeleteSubscription()
        {
            try
            {
                var sub = _context.Subscriptions.Find(Subscriptionid);
                if (sub != null)
                {
                    sub.IsActive = false;
                    sub.IsDeleted = true;
                    _context.Attach(sub).State = EntityState.Modified;
                    await _context.SaveChangesAsync();
                    _toastNotification.AddSuccessToastMessage("Deleted Successfully");
                }

                else
                {
                    _toastNotification.AddErrorToastMessage("Something went wrong");
                }
            }
            catch (Exception)

            {
                _toastNotification.AddErrorToastMessage("Something went wrong");
            }

            return RedirectToPage("Index");

        }


     
    }
}

