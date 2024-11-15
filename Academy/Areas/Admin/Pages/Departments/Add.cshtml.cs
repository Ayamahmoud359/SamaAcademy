using Academy.DTO;
using Academy.Models;
using Academy.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using NToastNotify;
using Microsoft.Extensions.Hosting;

namespace Academy.Areas.Admin.Pages.Departments
{
    public class AddModel : PageModel
    {
        [BindProperty]
        public DepartmentVM Dept { get; set; } = new DepartmentVM();
        public List<Branch> Branches { get; set; } = new List<Branch>();

        private readonly AcademyContext _context;
        private readonly IToastNotification _toastNotification;
        private readonly IWebHostEnvironment _hostEnvironment;

        public AddModel(AcademyContext context
            ,IToastNotification toastNotification
            ,IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
           _toastNotification = toastNotification;
            _hostEnvironment = webHostEnvironment;
        }
        public void OnGet()
        {
            Branches = _context.Branches.Where(b=>b.IsActive&&!b.IsDeleted).ToList();
        }
        public async Task<IActionResult> OnPostAsync(IFormFile fileUpload)

        {
            Branches = _context.Branches.Where(b=>b.IsActive&&!b.IsDeleted).ToList();
            if (!ModelState.IsValid)
            {
                _toastNotification.AddErrorToastMessage("Something Went Wrong");
                return Page();
            }
           
            try
            {
                Department department = new Department()
                {

                    IsActive = true,
                    DepartmentName = Dept.DepartmentName,

                    DepartmentDescription = Dept.DepartmentDescription,
                    BranchId = Dept.BranchId

                };
                if (fileUpload != null && fileUpload.Length > 0)
                {
                    string folder = "uploads/Departments/";
                    department.Image = await UploadImage(folder, fileUpload);

                }
                _context.Departments.Add(department);
                _context.SaveChanges();
                _toastNotification.AddSuccessToastMessage("Department Added Successfully");
                return RedirectToPage("Index");
            }
            catch (Exception exc)
            {
                ModelState.AddModelError(string.Empty, exc.Message);
                _toastNotification.AddErrorToastMessage("Something Went Wrong");
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

