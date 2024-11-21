using Academy.Data;
using Academy.DataGridVM;
using Academy.Models;
using DevExtreme.AspNet.Data;
using DevExtreme.AspNet.Mvc;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace Academy.Areas.Admin.Pages.Categories
{
    public class CategorySubscriptionsModel : PageModel
    {
        private readonly AcademyContext _context;



        public CategorySubscriptionsModel(AcademyContext context)
        {
            _context = context;

        }


        public int Id { get; set; }



        public async Task<IActionResult> OnGetAsync(int id)
        {

            try
            {


                if (id != 0)
                {
                    Id = id;
                    return Page();
                }
                return RedirectToPage("../NotFound");
            }
            catch (Exception e)
            {
                return RedirectToPage("../Error");
            }



        }
        public IActionResult OnGetGridData(DataSourceLoadOptions loadOptions, int id)
        {
            List<SubscriptionDataGridVM> data = new List<SubscriptionDataGridVM>();
            if (id != 0)
            {
                var Allsubscriptions = _context.Subscriptions.Where(d => !d.IsDeleted).ToList();
                foreach (var item in Allsubscriptions)
                {
                    if (item.EndDate < DateOnly.FromDateTime(DateTime.Now))
                    {
                        item.IsActive = false;
                    }


                }
                _context.SaveChanges();
                data = _context.Subscriptions.Include(a => a.Trainee)
                   
                    .Where(d => !d.IsDeleted 
                && !d.Trainee.IsDeleted
                && d.Trainee.IsActive
                && d.CategoryId == id).Select(i => new SubscriptionDataGridVM()
                {
                    SubscriptionId = i.SubscriptionId,
                    StartDate = i.StartDate,
                    EndDate = i.EndDate,
                  
                    TraineeName = i.Trainee == null ? "" : i.Trainee.TraineeName,
                   
                    IsActive = i.IsActive

                }).ToList();
            }

            return new JsonResult(DataSourceLoader.Load(data, loadOptions));
        }

    }
}
