using Academy.Data;
using Academy.DTO;
using Academy.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using NToastNotify;

namespace Academy.Areas.Admin.Pages.CompetitionDepartmentsManagment
{
    public class EditModel : PageModel
    {
        private readonly AcademyContext _context;
        private readonly IToastNotification _toastNotification;

        public EditModel(AcademyContext context, IToastNotification toastNotification)
        {
            _context = context;
            _toastNotification = toastNotification;
        }
        [BindProperty]

        public CompetitionDepartmentVM CompetitionDepartmentVM { get; set; }


        public async Task<IActionResult> OnGetAsync(int? id)
        {

            try
            {
              
                var competitionDepartment = await _context.CompetitionDepartment.FirstOrDefaultAsync(m =>  m.Id == id);
                if (competitionDepartment != null)
                {
                   CompetitionDepartmentVM=new CompetitionDepartmentVM();
                    CompetitionDepartmentVM.Name = competitionDepartment.Name;
                    CompetitionDepartmentVM.Description = competitionDepartment.Description;
                    CompetitionDepartmentVM.Image = competitionDepartment.Image;
                    CompetitionDepartmentVM.IsActive = competitionDepartment.IsActive;
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

                var competitionDepartment = await _context.CompetitionDepartment.FirstOrDefaultAsync(m => m.Id == CompetitionDepartmentVM.Id);
                if (competitionDepartment != null)
                {
                    competitionDepartment.Name = CompetitionDepartmentVM.Name;
                    competitionDepartment.Description = CompetitionDepartmentVM.Description;
                    competitionDepartment.IsActive = CompetitionDepartmentVM.IsActive;
                    _context.Attach(competitionDepartment).State = EntityState.Modified;
                    await _context.SaveChangesAsync();
                    _toastNotification.AddSuccessToastMessage("Competition Deparment Edited Successfully");
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
