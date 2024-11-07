using Academy.Data;
using Academy.DTO;
using Academy.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace Academy.Areas.Admin.Pages.Departments
{
    public class DeleteModel : PageModel
    {
        private readonly AcademyContext _context;

        public Department Dept { get; set; }

        public DeleteModel(AcademyContext context)
        {
            _context = context;

        }
        public async Task<IActionResult> OnGetAsync(int? id)
        {

            try
            {

                Dept = await _context.Departments.FirstOrDefaultAsync(m => m.DepartmentId == id);
                if (Dept != null)
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

        public IActionResult OnPostAsync(int?id)
        {
            try
            {
            Dept = _context.Departments.Find(id);
            if (Dept != null)
            {
                    var trainers = _context.Trainers.Where(t => t.DepartmentId == id).ToList();
                  //  _context.Trainers.RemoveRange(trainers);
                    _context.Departments.Remove(Dept);
               
                    _context.SaveChanges();
                   
                    return RedirectToPage("Index");
            }
                return RedirectToPage("../NotFound");


            }
            catch (Exception e)
            {
                ModelState.AddModelError(string.Empty, e.Message);
                return RedirectToPage("/Departments/Delete", new { id = Dept?.DepartmentId });
            }

          

        }

    }
}
