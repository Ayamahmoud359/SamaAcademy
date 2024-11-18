using Academy.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using NToastNotify;

namespace Academy.Areas.Admin.Pages.TraineeChampions
{
    public class IndexModel : PageModel
    {
        private readonly AcademyContext _context;
        [BindProperty]
        public int TraineeChampionid { set; get; }
        private readonly IToastNotification _toastNotification;

        public IndexModel(AcademyContext context, IToastNotification toastNotification)
        {
            _context = context;
            _toastNotification = toastNotification;
        }

        public async Task<IActionResult> OnPostDeleteTraineeChampion()
        {
            try
            {
                var tranieeChampion = _context.TraineeChampions.Find(TraineeChampionid);
                if ( tranieeChampion!= null)
                {


                    tranieeChampion.IsActive = false;
                    tranieeChampion.IsDeleted = true;


                    _context.Attach(tranieeChampion).State = EntityState.Modified;
                    await _context.SaveChangesAsync();
                    _toastNotification.AddSuccessToastMessage("Deleted Successfully");
                    return RedirectToPage("Index");
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
