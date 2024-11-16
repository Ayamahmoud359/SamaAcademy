using Academy.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using NToastNotify;

namespace Academy.Areas.Admin.Pages.Exams
{
    public class IndexModel : PageModel
    {
        private readonly AcademyContext _context;
        [BindProperty]
        public int Evaluationid { set; get; }
        private readonly IToastNotification _toastNotification;

        public IndexModel(AcademyContext context, IToastNotification toastNotification)
        {
            _context = context;
            _toastNotification = toastNotification;
        }

        public async Task<IActionResult> OnPostDeleteEvaluation()
        {
            try
            {
                var Evaluation = _context.Exams.Find(Evaluationid);
                if (Evaluation != null)
                {
                    Evaluation.IsDeleted = true;


                    _context.Attach(Evaluation).State = EntityState.Modified;
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
