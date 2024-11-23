using Academy.Data;
using Academy.DTO;
using Academy.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using NToastNotify;
using System.Security.Claims;

namespace Academy.Areas.Admin.Pages.Departments
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

        public DepartmentVM Dept{ get; set; }
      
       
        public List<Branch> Branches { get; set; }
        public async Task<IActionResult> OnGetAsync(int? id)
        {

            try
            {
                Branches = _context.Branches.Where(b => b.IsActive && !b.IsDeleted).ToList();
                var department = await _context.Departments.Include(d=>d.Branch).FirstOrDefaultAsync(m => /*m.Branch.IsActive&&*/ m.DepartmentId == id );
                if (department != null)
                {
                    Dept = new DepartmentVM();
                    Dept.DepartmentName = department.DepartmentName;
                    Dept.DepartmentDescription = department.DepartmentDescription;
                   Dept.BranchId = department.BranchId;
                    Dept.DepartmentId = department.DepartmentId;
                    Dept.IsActive = department.IsActive;
                    return Page();
                }
                return RedirectToPage("../NotFound");
            }
            catch(Exception e)
            {
               return  RedirectToPage("../Error");
            }
          

           
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync(IFormFile? fileUpload)
        {
            Branches = _context.Branches.Where(b => b.IsActive && !b.IsDeleted).ToList();
          

            try
            {
                var department = await _context.Departments.FirstOrDefaultAsync(m => m.DepartmentId == Dept.DepartmentId);
                if (department != null)
                {
                    department.DepartmentName = Dept.DepartmentName;
                    department.DepartmentDescription = Dept.DepartmentDescription;
                  
                    department.IsActive = Dept.IsActive;
                    if (fileUpload != null && fileUpload.Length > 0)
                    {
                        if (department.Image != null)
                        {
                            var ImagePath = Path.Combine(_hostEnvironment.WebRootPath, department.Image);
                            if (System.IO.File.Exists(ImagePath))
                            {
                                System.IO.File.Delete(ImagePath);
                            }
                        }
                        string folder = "uploads/Departments/";
                        department.Image = await UploadImage(folder, fileUpload);

                    }
                    _context.Attach(department).State = EntityState.Modified;
                    await _context.SaveChangesAsync();
                    _toastNotification.AddSuccessToastMessage("Deparment Edited Successfully");
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
