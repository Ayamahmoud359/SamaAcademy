using Academy.Data;
using Academy.DTO;
using Academy.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using NToastNotify;

namespace Academy.Areas.Admin.Pages.CompetitionDepartmentsManagment
{
    public class EditModel : PageModel
    {
        private readonly AcademyContext _context;
        private readonly IToastNotification _toastNotification;
        private readonly IWebHostEnvironment _hostEnvironment;

        public EditModel(AcademyContext context
            , IToastNotification toastNotification
             , IWebHostEnvironment hostEnvironment)
        {
            _context = context;
            _toastNotification = toastNotification;
            _hostEnvironment = hostEnvironment;
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
                    CompetitionDepartmentVM.Id = competitionDepartment.Id;
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
        public async Task<IActionResult> OnPostAsync(IFormFile? fileUpload)
        {
           
            try
            {

                var competitionDepartment = await _context.CompetitionDepartment.FirstOrDefaultAsync(m => m.Id == CompetitionDepartmentVM.Id);
                if (competitionDepartment != null)
                {
                    competitionDepartment.Name = CompetitionDepartmentVM.Name;
                    competitionDepartment.Description = CompetitionDepartmentVM.Description;
                    competitionDepartment.IsActive = CompetitionDepartmentVM.IsActive;
                    if (fileUpload != null && fileUpload.Length > 0)
                    {
                        //if (competitionDepartment.Image != null)
                        //{
                        //    var ImagePath = Path.Combine(_hostEnvironment.WebRootPath, competitionDepartment.Image);
                        //    if (System.IO.File.Exists(ImagePath))
                        //    {
                        //        System.IO.File.Delete(ImagePath);
                        //    }
                        //}
                        string folder = "uploads/CompetitionDepartments/";
                        competitionDepartment.Image = await UploadImage(folder, fileUpload);

                    }
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

        private async Task<string> UploadImage(string folderPath, IFormFile file)
        {

            folderPath += Guid.NewGuid().ToString() + "_" + file.FileName;

            string serverFolder = Path.Combine(_hostEnvironment.WebRootPath, folderPath);

            var directory = Path.GetDirectoryName(serverFolder);
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            await file.CopyToAsync(new FileStream(serverFolder, FileMode.Create));

            return folderPath;
        }
    }
}
