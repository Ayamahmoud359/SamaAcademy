using Academy.Data;
using Academy.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace Academy.Areas.Admin.Pages.Subscriptions
{
    public class ViewModel : PageModel
    {
        private readonly AcademyContext _context;
        public Subscription Subscription { get; set; }

        public ViewModel(AcademyContext context)
        {
            _context = context;
          Subscription = new Subscription();

        }
        public async Task<IActionResult> OnGetAsync(int? id)
        {

            try
            {

                Subscription = await _context.Subscriptions.Include(a => a.Trainee)
                      .Include(a => a.Category)
                      .ThenInclude(a => a.Department)
                      .ThenInclude(a => a.Branch).FirstOrDefaultAsync(m => m.SubscriptionId == id);
                if (Subscription != null)
                {

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
