using Academy.DTO;
using Academy.Models;
using Academy.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using NToastNotify;
using Microsoft.AspNetCore.Hosting;


namespace Academy.Areas.Admin.Pages.Branchs
{
    public class AddModel : PageModel
    {
        [BindProperty]
        public BranchVM BranchVM { get; set; }

        private readonly AcademyContext _context;
        private readonly IToastNotification _toastNotification;
        private readonly IWebHostEnvironment _hostEnvironment;
        public AddModel(AcademyContext context
            , IToastNotification toastNotification
            , IWebHostEnvironment hostEnvironment)
        {
            _context = context;
            _toastNotification = toastNotification;
            _hostEnvironment = hostEnvironment;
        }
        public void OnGet()
        {
        }
        public async Task<IActionResult> OnPostAsync(IFormFile? fileUpload)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    _toastNotification.AddErrorToastMessage("Somthing went Error");
                    return Page();
                }


                Branch branch = new Branch()
                {
                    BranchName = BranchVM.BranchName,
                    BranchAddress = BranchVM.BranchAddress,
                    IsActive = true
                };
                if (fileUpload != null && fileUpload.Length > 0)
                {
                    string folder = "uploads/Branchs/";
                    branch.Image = await UploadImage(folder, fileUpload);

                }

                _context.Branches.Add(branch);
                _context.SaveChanges();
                _toastNotification.AddSuccessToastMessage("Branch Added Successfully");
                return RedirectToPage("Index");
            }
            catch (Exception exc)
            {
                ModelState.AddModelError(string.Empty, exc.Message);
                _toastNotification.AddErrorToastMessage("Somthing went Error");
                return Page();
            }
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

