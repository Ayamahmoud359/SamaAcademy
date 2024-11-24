using Academy.Data;
using Academy.DTO;
using Academy.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using NToastNotify;

namespace Academy.Areas.Admin.Pages.Banners
{
    public class EditModel : PageModel
    {
      
        private readonly AcademyContext _context;
        private readonly IToastNotification _toastNotification;
        private readonly IWebHostEnvironment _hostEnvironment;

        public EditModel(AcademyContext context
            ,IToastNotification toastNotification
             , IWebHostEnvironment hostEnvironment)
            {
                _context = context;
            _toastNotification = toastNotification;
            _hostEnvironment = hostEnvironment;
        }
            [BindProperty]

            public Banner Banner { get; set; }

            public async Task<IActionResult> OnGetAsync(int? id)
            {

                try
                {

                Banner = await _context.Banners.FirstOrDefaultAsync(m => m.Id == id);
                    if (Banner != null)
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
            public async Task<IActionResult> OnPostAsync(IFormFile? fileUpload)
            {


                try
                {
                    var bannerToEdit = await _context.Banners.FirstOrDefaultAsync(m => m.Id == Banner.Id);
                    if (bannerToEdit != null)
                    {
                    bannerToEdit.Title = Banner.Title;
                    bannerToEdit.IsActive = Banner.IsActive;
                    if (fileUpload != null && fileUpload.Length > 0)
                    {
                        //if (categoryToEdit.image != null)
                        //{
                        //    var ImagePath = Path.Combine(_hostEnvironment.WebRootPath, categoryToEdit.image);
                        //    if (System.IO.File.Exists(ImagePath))
                        //    {
                        //        System.IO.File.Delete(ImagePath);
                        //    }
                        //}
                        string folder = "uploads/Banners/";
                        bannerToEdit.Image = await UploadImage(folder, fileUpload);

                    }
                    _context.Attach(bannerToEdit).State = EntityState.Modified;
                        await _context.SaveChangesAsync();
                    _toastNotification.AddSuccessToastMessage("Banner Edited Successfully");
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
