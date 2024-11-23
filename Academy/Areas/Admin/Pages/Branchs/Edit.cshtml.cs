using Academy.Data;
using Academy.DTO;
using Academy.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using NToastNotify;

namespace Academy.Areas.Admin.Pages.Branchs
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
        public async Task<IActionResult> OnPostAsync(IFormFile? fileUpload)
        {
           
         

            try
            {
                var branch = await _context.Branches.FirstOrDefaultAsync(m => m.BranchId == Branch.BranchId);
                if (branch != null)
                {
                     branch.BranchName=  Branch.BranchName ;
                    branch.BranchAddress   = Branch.BranchAddress ;
                    branch.IsActive = Branch.IsActive;
                   
                    if (fileUpload != null && fileUpload.Length > 0)
                    {
                        if (branch.Image!= null)
                        {
                            var ImagePath = Path.Combine(_hostEnvironment.WebRootPath, branch.Image);
                            if (System.IO.File.Exists(ImagePath))
                            {
                                System.IO.File.Delete(ImagePath);
                            }
                        }
                        string folder = "uploads/Branchs/";
                        branch.Image = await UploadImage(folder, fileUpload);

                    }
                    _context.Attach(branch).State = EntityState.Modified;
                    await _context.SaveChangesAsync();
                    _toastNotification.AddSuccessToastMessage("Branch Edited Successfully");
                    return RedirectToPage("Index");
                }
                
                ModelState.AddModelError(string.Empty, "SomeThing Went Wrong");

                
            }
            catch (Exception e)
            {
                ModelState.AddModelError(string.Empty, e.Message);
                
            }
            _toastNotification.AddErrorToastMessage("Somthing went Error");
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
