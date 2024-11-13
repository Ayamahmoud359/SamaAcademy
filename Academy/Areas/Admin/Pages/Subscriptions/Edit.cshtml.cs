using Academy.Data;
using Academy.DTO;
using Academy.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Academy.Areas.Admin.Pages.Subscriptions
{
    public class EditModel : PageModel
    {
        private readonly AcademyContext _context;


        public EditModel(AcademyContext context)
        {
            _context = context;
            Subscription = new SubscriptionVM();
         

        }
        [BindProperty]

        public SubscriptionVM Subscription { get; set; }



        public async Task<IActionResult> OnGetAsync(int? id)
        {

            try
            {

               var Sub = await _context.Subscriptions.FirstOrDefaultAsync(m => m.SubscriptionId == id);
                if (Subscription != null)
                {
                    Subscription.StartDate = Sub.StartDate;
                    Subscription.EndDate = Sub.EndDate;
                    Subscription.TraineeId = Sub.TraineeId;
                    Subscription.CategoryId = Sub.CategoryId;
                    Subscription.DepartmentId = (int)Sub.Department;
                    Subscription.BranchId = (int)Sub.Branch;
                    Subscription.SubscriptionId = Sub.SubscriptionId;
                    Subscription.IsActive = Sub.IsActive;
                    return Page();
                }
                return RedirectToPage("../NotFound");
            }
            catch (Exception e)
            {
                return RedirectToPage("../Error");
            }



        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
          
            if (!ModelState.IsValid)
            {
                return Page();
            }

            try
            {
                var SubscriptionToEdit = await _context.Subscriptions.FirstOrDefaultAsync(m => m.SubscriptionId == Subscription.SubscriptionId);
                if (SubscriptionToEdit.EndDate < DateOnly.FromDateTime(DateTime.Now))
                {
                    ModelState.AddModelError(string.Empty, "Subscription EndDate Has Already Passed ,You can't activate it");
                    return Page();
                }
                if (Subscription.EndDate == Subscription.StartDate || Subscription.EndDate < Subscription.StartDate)
                {
                    ModelState.AddModelError(string.Empty, "EndDate must be greater than StartDate");
                    return Page();
                   
                }

           
                
                SubscriptionToEdit.StartDate = Subscription.StartDate;
                SubscriptionToEdit.EndDate = Subscription.EndDate;
                SubscriptionToEdit.IsActive = Subscription.IsActive;

                _context.Attach(SubscriptionToEdit).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                return  RedirectToPage("Index");

            }
            catch (Exception e)
            {
                ModelState.AddModelError(string.Empty, e.Message);
                return Page();
            }

        }
    }
}
