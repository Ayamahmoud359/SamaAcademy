using Academy.Data;
using Academy.DTO;
using Academy.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace Academy.Areas.Admin.Pages.Branchs
{
    public class EditModel : PageModel
    {
        private readonly AcademyContext _context;



        public EditModel(AcademyContext context)
        {
            _context = context;

        }
        [BindProperty]

        public BranchVM Branch { get; set; }


        
        public async Task<IActionResult> OnGetAsync(int? id)
        {

            try
            {
               
                var branch = await _context.Branches.FirstOrDefaultAsync(m =>m.BranchId == id);
                if (branch != null)
                {
                    Branch = new BranchVM();
                    Branch.BranchName= branch.BranchName;
                    Branch.BranchAddress = branch.BranchAddress;
                    Branch.BranchId =branch.BranchId;

                    Branch.IsActive = branch.IsActive;
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
                var branch = await _context.Branches.FirstOrDefaultAsync(m => m.BranchId == Branch.BranchId);
                if (branch != null)
                {
                   branch.BranchName=  Branch.BranchName ;
                    branch.BranchAddress   = Branch.BranchAddress ;
                    branch.BranchId = Branch.BranchId;


                    branch.IsActive = Branch.IsActive;
                    _context.Attach(branch).State = EntityState.Modified;
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
