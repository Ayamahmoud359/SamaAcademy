using Academy.Data;
using Academy.DTO;
using Academy.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using NToastNotify;

namespace Academy.Areas.Admin.Pages.Champions
{
    public class EditModel : PageModel
    {

        private readonly AcademyContext _context;
        private readonly IToastNotification _toastNotification;

        public EditModel(AcademyContext context, IToastNotification toastNotification)
        {
            _context = context;
            _toastNotification = toastNotification;
            champion = new ChampionVM();
        }
        [BindProperty]

        public ChampionVM champion { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {

            try
            {

              var  Champion = await _context.Champions.FirstOrDefaultAsync(m => m.ChampionId == id);
                if (Champion != null)
                {
                    champion.ChampionName = Champion.ChampionName;
                    champion.ChampionDate = Champion.ChampionDate;
                    champion.ChampionId = Champion.ChampionId;
                    champion.ChampionDescription = Champion.ChampionDescription;
                  

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


            try
            {
                var championToEdit = await _context.Champions.FirstOrDefaultAsync(m => m.ChampionId ==champion.ChampionId);
                if (championToEdit != null)
                {
                    championToEdit.ChampionName = champion.ChampionName;
                    championToEdit.ChampionDescription = champion.ChampionDescription;
                   
                    championToEdit.ChampionDate = champion.ChampionDate;
                
                    _context.Attach(championToEdit).State = EntityState.Modified;
                    await _context.SaveChangesAsync();
                    _toastNotification.AddSuccessToastMessage("Champion Information Edited Successfully");
                    return RedirectToPage("Index");
                }
                ModelState.AddModelError(string.Empty, "SomeThing Went Wrong");

            }
            catch (Exception e)
            {
                ModelState.AddModelError(string.Empty, e.Message);

            }
            _toastNotification.AddErrorToastMessage("Something Went Wrong");
            return Page();
        }
    }
}
