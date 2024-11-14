using Academy.DTO;
using Academy.Models;
using Academy.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using NToastNotify;


namespace Academy.Areas.Admin.Pages.Branchs
{
    public class AddModel : PageModel
    {
        [BindProperty]
        public BranchVM BranchVM { get; set; }

        private readonly AcademyContext _context;
        private readonly IToastNotification _toastNotification;

        public AddModel(AcademyContext context, IToastNotification toastNotification)
        {
            _context = context;
            _toastNotification = toastNotification;
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
                _toastNotification.AddSuccessToastMessage("Branch Added Successfully");
                return RedirectToPage("Index");
            }
            catch(Exception exc)
            {
                ModelState.AddModelError(string.Empty, exc.Message);
                _toastNotification.AddErrorToastMessage("Somthing went Error");
                return Page();
            }

            
        }
    }
}
