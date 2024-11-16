using Academy.Data;
using Academy.DTO;
using Academy.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using NToastNotify;

namespace Academy.Areas.Admin.Pages.Categories
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

            public Category category { get; set; }

            public async Task<IActionResult> OnGetAsync(int? id)
            {

                try
                {
                   
                   category = await _context.Categories.FirstOrDefaultAsync(m => m.CategoryId == id);
                    if (category != null)
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


                try
                {
                    var categoryToEdit = await _context.Categories.FirstOrDefaultAsync(m => m.CategoryId == category.CategoryId);
                    if (categoryToEdit != null)
                    {
                       categoryToEdit.CategoryName=category.CategoryName;
                    categoryToEdit.CategoryDescription = category.CategoryDescription;
                    categoryToEdit.IsActive = category.IsActive;
                        _context.Attach(categoryToEdit).State = EntityState.Modified;
                        await _context.SaveChangesAsync();
                    _toastNotification.AddSuccessToastMessage("Category Edited Successfully");
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
