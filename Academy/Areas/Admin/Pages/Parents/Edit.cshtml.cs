using Academy.Data;
using Academy.DTO;
using Academy.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using NToastNotify;

namespace Academy.Areas.Admin.Pages.Parents
{
    public class EditModel : PageModel
    {
        private readonly AcademyContext _context;
        private readonly IToastNotification _toastNotification;

        public EditModel(AcademyContext context,IToastNotification toastNotification)
        {
            _context = context;
            _toastNotification = toastNotification;
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
                _toastNotification.AddErrorToastMessage("SomeThing Went Wrong");
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
                _toastNotification.AddSuccessToastMessage("Parent Information Edited Successfully");
                return RedirectToPage("Index");
               
            }
            catch (Exception e)
            {
                ModelState.AddModelError(string.Empty, e.Message);
                _toastNotification.AddErrorToastMessage("SomeThing Went Wrong");
                return Page();
            }

        }
    }
}
