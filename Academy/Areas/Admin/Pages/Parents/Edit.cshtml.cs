using Academy.Data;
using Academy.DTO;
using Academy.Models;
using Microsoft.AspNetCore.Identity;
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
        private readonly ApplicationDbContext _applicationDb;
        private readonly UserManager<ApplicationUser> _userManager;

        public EditModel(AcademyContext context
            ,IToastNotification toastNotification
            ,ApplicationDbContext applicationDb
            ,UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _toastNotification = toastNotification;
            _applicationDb = applicationDb;
            _userManager = userManager;
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
           
            try
            {
                var ParentToEdit = await _context.Parents.FirstOrDefaultAsync(m => m.ParentId == Parent.ParentId);
                if (Parent.UserName!=ParentToEdit.UserName )
                {
                    var checkuser =await _userManager.FindByNameAsync(Parent.UserName);
                    if (checkuser!=null)
                    {
                        ModelState.AddModelError(string.Empty, "User Name alredy Token");
                        _toastNotification.AddErrorToastMessage("SomeThing Went Wrong");
                        return Page();
                    }
                    var ExistedUser =await _userManager.FindByNameAsync(ParentToEdit.UserName);
                    if (ExistedUser != null)
                    {
                        ExistedUser.UserName = Parent.UserName;
                        var result = await _userManager.UpdateAsync(ExistedUser);
                        if (result.Succeeded)
                        {
                            ParentToEdit.UserName = Parent.UserName;
                          
                        }
                        else
                        {
                            foreach (var error in result.Errors)
                            {
                                ModelState.AddModelError(string.Empty, error.Description);
                            }
                            _toastNotification.AddErrorToastMessage("SomeThing Went Wrong");
                            return Page();
                        }
                    }
                    
                }
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
