using Academy.Data;
using Academy.DataGridVM;
using Academy.Models;
using DevExtreme.AspNet.Data;
using DevExtreme.AspNet.Mvc;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace Academy.Areas.Admin.Pages.Trainees
{
    public class SubscriptionsModel : PageModel
    {
        private readonly AcademyContext _context;



        public SubscriptionsModel(AcademyContext context)
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

                data = _context.Subscriptions
                    .Include(a => a.Category)
                    .ThenInclude(a => a.Department)
                    .ThenInclude(a => a.Branch)
                    .Where(d => !d.IsDeleted
                && !d.Category.IsDeleted && d.Category.IsActive
                && !d.Category.Department.IsDeleted && d.Category.Department.IsActive
                && !d.Category.Department.Branch.IsDeleted && d.Category.Department.Branch.IsActive
                && d.TraineeId == id).Select(i => new SubscriptionDataGridVM()
                {
                    SubscriptionId = i.SubscriptionId,
                    StartDate = i.StartDate,
                    EndDate = i.EndDate,
                   
                    CategoryName = i.Category == null ? "" : i.Category.CategoryName,
                    DepartmentName = i.Category.Department == null ? "" : i.Category.Department.DepartmentName,
                    BranchName = i.Category.Department.Branch == null ? "" : i.Category.Department.Branch.BranchName,
                    IsActive = i.IsActive

                }).ToList();
            }

            return new JsonResult(DataSourceLoader.Load(data, loadOptions));
        }

    }
}


