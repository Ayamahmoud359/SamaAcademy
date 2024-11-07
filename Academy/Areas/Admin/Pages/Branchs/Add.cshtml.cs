using Academy.DTO;
using Academy.Models;
using Academy.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Academy.Areas.Admin.Pages.Branchs
{
    public class AddModel : PageModel
    {
        [BindProperty]
        public BranchVM BranchVM { get; set; }

        private readonly AcademyContext _context;
        public AddModel(AcademyContext context)
        {
            _context = context;
        }
        public void OnGet()
        {
        }
        public async Task<IActionResult> OnPostAsync()
        {
          
            Branch branch = new Branch()
            {
                BranchName = BranchVM.BranchName,
               
                BranchAddress = BranchVM.BranchAddress,
                IsActive = true
            };
            try
            {
                _context.Branches.Add(branch);
                _context.SaveChanges();
                return RedirectToPage("Index");
            }
            catch(Exception exc)
            {
                ModelState.AddModelError(string.Empty, exc.Message);
                return Page();
            }

            
        }
    }
}
