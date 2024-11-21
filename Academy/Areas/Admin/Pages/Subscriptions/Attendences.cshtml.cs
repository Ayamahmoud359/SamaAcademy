using Academy.Data;
using Academy.DataGridVM;
using Academy.Models;
using DevExtreme.AspNet.Data;
using DevExtreme.AspNet.Mvc;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace Academy.Areas.Admin.Pages.Subscriptions
{
    public class AttendencesModel : PageModel
    {
        private readonly AcademyContext _context;



        public AttendencesModel(AcademyContext context)
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
            List<AbsenceDataGridVM> data = new List<AbsenceDataGridVM>();
            if (id != 0)
            {
                data = _context.Abscenses.Include(a => a.Trainer)
                    .Where(d => !d.IsDeleted 
              
                && d.SubscriptionId == id).Select(i => new AbsenceDataGridVM()
                {
                    AbsenceId = i.AbsenceId,
                    AbsenceDate = i.AbsenceDate,
                    IsAbsent = i.IsAbsent,
                    Type = i.Type,
                    TrainerId = i.TrainerId,
                    TrainerName = i.Trainer.IsDeleted ? "" : i.Trainer.TrainerName,
                   
                }).ToList();
            }

            return new JsonResult(DataSourceLoader.Load(data, loadOptions));
        }


    }
}
