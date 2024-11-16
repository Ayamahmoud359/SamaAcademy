using Academy.Data;
using Academy.DTO;
using Academy.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using NToastNotify;

namespace Academy.Areas.Admin.Pages.Champions
{
    public class AddModel : PageModel
    {
        [BindProperty]
        public ChampionVM ChampionVM { get; set; }

        private readonly AcademyContext _context;
        private readonly IToastNotification _toastNotification;
        private readonly IWebHostEnvironment _hostEnvironment;
        public List<Branch> Branches { get; set; } = new List<Branch>();

        public List<Department> Departments { get; set; } = new List<Department>();

        public AddModel(AcademyContext context
            , IToastNotification toastNotification
            , IWebHostEnvironment hostEnvironment)
        {
            _context = context;
            _toastNotification = toastNotification;
            _hostEnvironment = hostEnvironment;
            ChampionVM = new ChampionVM();
        }
        public void OnGet()
        {

            Branches = _context.Branches.Where(b => b.IsActive && !b.IsDeleted).ToList();
        }
        public async Task<IActionResult> OnPostAsync(IFormFile? fileUpload)
        {

            Branches = _context.Branches.Where(b => !b.IsDeleted && b.IsActive).ToList();
            Departments = _context.Departments.Where(d => !d.IsDeleted && d.IsActive && d.BranchId == ChampionVM.BranchId).ToList();
            try
            {
                if (!ModelState.IsValid)
                {
                    _toastNotification.AddErrorToastMessage("SomeThing Went Wrong");
                    return Page();
                }


                Champion champion = new Champion()
                {
                    ChampionName = ChampionVM.ChampionName,
                    ChampionDate = ChampionVM.ChampionDate,
                    DepartmentId = ChampionVM.DepartmentId,
                    ChampionDescription = ChampionVM.ChampionDescription,
                    IsActive = true
                };
                //if (fileUpload != null && fileUpload.Length > 0)
                //{
                //    string folder = "uploads/Champions/";
                //    branch.Image = await UploadImage(folder, fileUpload);

                //}

                _context.Champions.Add(champion);
                _context.SaveChanges();
                _toastNotification.AddSuccessToastMessage("Champion Added Successfully");
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

        public IActionResult OnGetGetDepartments(int id)
        {
            try
            {

                if (id != 0)
                {


                    var departments = _context.Departments.Where(b => !b.IsDeleted && b.IsActive && b.BranchId == id).Select(b => new { b.DepartmentName, b.DepartmentId }).ToList();


                    return new JsonResult(departments);
                }

                return new JsonResult("SomeThing Went Wrong");
            }
            catch (Exception ex)
            {
                return new JsonResult(ex.Message);
            }


        }

    }
}
