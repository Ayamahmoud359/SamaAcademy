using Academy.Data;
using Academy.DTO;
using Academy.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace Academy.Areas.Admin.Pages.Parents
{
    public class EditModel : PageModel
    {
        private readonly AcademyContext _context;



        public EditModel(AcademyContext context)
        {
            _context = context;

        }
        [BindProperty]

        public Parent Parent { get; set; }


       
        public async Task<IActionResult> OnGetAsync(int? id)
        {

            try
            {
              
               Parent = await _context.Parents.FirstOrDefaultAsync(m => m.ParentId == id);
                if (Parent != null)
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
                var ParentToEdit = await _context.Parents.FirstOrDefaultAsync(m => m.ParentId == Parent.ParentId);
                ParentToEdit.ParentName = Parent.ParentName;
                ParentToEdit.ParentAddress = Parent.ParentAddress;
                ParentToEdit.ParentPhone = Parent.ParentPhone;
                ParentToEdit.IsActive = Parent.IsActive;
                _context.Attach(ParentToEdit).State = EntityState.Modified;
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
