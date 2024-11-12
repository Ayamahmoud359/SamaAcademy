using Academy.Data;
using Academy.DTO;
using Academy.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace Academy.Areas.Admin.Pages.Categories
{
    public class EditModel : PageModel
    {
      
            private readonly AcademyContext _context;



            public EditModel(AcademyContext context)
            {
                _context = context;

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

            if (!ModelState.IsValid)
                {
                    return Page();
                }

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
                        return RedirectToPage("Index");
                    }
                    ModelState.AddModelError(string.Empty, "SomeThing Went Wrong");

                    return Page();
                }
                catch (Exception e)
                {
                    ModelState.AddModelError(string.Empty, e.Message);
                    return Page();
                }

            }
        }
}
