using Academy.Data;
using Academy.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Academy.Areas.Admin.Pages.Trainees
{
    public class EditModel : PageModel
    {
        private readonly AcademyContext _context;


        public List<SelectListItem> Nationalities { get; set; }
        public EditModel(AcademyContext context)
        {
            _context = context;
            Nationalities = new List<SelectListItem>
                {
            new SelectListItem { Text = "American", Value = "US" },
            new SelectListItem { Text = "Canadian", Value = "CA" },
            new SelectListItem { Text = "Mexican", Value = "MX" },
            new SelectListItem { Text = "British", Value = "GB" },
            new SelectListItem { Text = "German", Value = "DE" },
            new SelectListItem { Text = "Indian", Value = "IN" },
            new SelectListItem { Text = "Australian", Value = "AU" },
        };

        }
        [BindProperty]

        public Trainee Trainee { get; set; }



        public async Task<IActionResult> OnGetAsync(int? id)
        {

            try
            {

               Trainee= await _context.Trainees.FirstOrDefaultAsync(m => m.TraineeId == id);
                if (Trainee != null)
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
                var TraineeToEdit = await _context.Trainees.FirstOrDefaultAsync(m => m.TraineeId == Trainee.TraineeId);
                TraineeToEdit.TraineeName = Trainee.TraineeName;
                TraineeToEdit.TraineeAddress = Trainee.TraineeAddress;
                TraineeToEdit.TraineePhone = Trainee.TraineePhone;
                TraineeToEdit.Nationality = Trainee.Nationality;
                TraineeToEdit.ResidencyNumber = Trainee.ResidencyNumber;
                TraineeToEdit.IsActive = Trainee.IsActive;
                TraineeToEdit.BirthDate = Trainee.BirthDate;
                _context.Attach(TraineeToEdit).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                return RedirectToPage("Index");

            }
            catch (Exception e)
            {
                ModelState.AddModelError(string.Empty, e.Message);
                return Page();
            }

        }
    }
}
