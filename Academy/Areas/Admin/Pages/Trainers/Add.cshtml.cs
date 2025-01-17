using Academy.DTO;
using Academy.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using System.Text.Encodings.Web;
using System.Text;
using static System.Runtime.InteropServices.JavaScript.JSType;
using Academy.Data;
using DevExtreme.AspNet.Data;
using DevExtreme.AspNet.Mvc;
using System.ComponentModel.DataAnnotations;
using NToastNotify;
using Microsoft.Extensions.Hosting;

namespace Academy.Areas.Admin.Pages.AddTrainer
{
    public class AddModel : PageModel
    {
        [BindProperty]
        public TrainerVM Trainer { get; set; }
        // Property to hold the selected values
        [BindProperty]
        [Required(ErrorMessage = "UserName is required")]
      
        [Display(Name = "UserName")]
        [Remote("IsEmailAvailable", "Functions", ErrorMessage = "This User Name is already taken.")]
        public string UserName { get; set; }
       
        
        public List<Branch> Branches { get; set; }
        public List<Department> Departments { get; set; }
        public List<Category> Categories { get; set; }

        private readonly AcademyContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IToastNotification _toastNotification;
        private readonly IWebHostEnvironment _hostEnvironment;

        public AddModel(AcademyContext context
            , UserManager<ApplicationUser> userManager
            ,SignInManager<ApplicationUser> signInManager
            ,IToastNotification toastNotification
            ,IWebHostEnvironment hostEnvironment)
        {
            Trainer = new TrainerVM();
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
           _toastNotification = toastNotification;
            _hostEnvironment = hostEnvironment;
        }
       
      
        public void OnGet()
        {
            Branches = _context.Branches.Where(b =>!b.IsDeleted&& b.IsActive ).ToList();
            Departments = new List<Department>();
            Categories = new List<Category>();
        }
       
     
        public async Task<IActionResult> OnPostAsync(IFormFile? fileUpload)
        {
            Branches = _context.Branches.Where(b => !b.IsDeleted&& b.IsActive  ).ToList();
            Departments =_context.Departments.Where(d=> !d.IsDeleted && d.IsActive&&  d.BranchId== Trainer.BranchId).ToList();
            Categories = _context.Categories.Where(d => !d.IsDeleted && d.IsActive&& d.DepartmentId == Trainer.DepartmentId).ToList();
            if (!ModelState.IsValid)
            {
                _toastNotification.AddErrorToastMessage("SomeThing Went Wrong");
                return Page();
            }
            try
            {
                var coach = new Trainer
                {
                    TrainerName = Trainer.TrainerName,
                    TrainerAddress = Trainer.TrainerAddress,
                    TrainerEmail = Trainer.TrainerEmail,
                    TrainerPhone = Trainer.TrainerPhone,
                    CurrentBranch = Trainer.BranchId,
                    CurrentDepartment = Trainer.DepartmentId,
                    HiringDate = Trainer.HiringDate,
                    UserName=UserName,
                    IsActive = true,


                };
                if (fileUpload != null && fileUpload.Length > 0)
                {
                    string folder = "uploads/Trainers/";
                    coach.Image = await UploadImage(folder, fileUpload);

                }
                _context.Trainers.Add(coach);
                await _context.SaveChangesAsync();
       
            var TrainerCategories= new List<TrainerCategories>();
            if (Trainer.SelectedCategories != null &&Trainer.SelectedCategories.Count != 0)
            {
                foreach (var category in Trainer.SelectedCategories)
                {
                        TrainerCategories.Add(new TrainerCategories
                    {
                        TrainerId = coach.TrainerId,
                        CategoryId = category,
                        IsActive=true
                    });
                }

                _context.CategoryTrainers.AddRange(TrainerCategories);
                await _context.SaveChangesAsync();
            }

            var user = new ApplicationUser
            {
                UserName = UserName,
                Email = Trainer.TrainerEmail==null?"":Trainer.TrainerEmail,
                PhoneNumber = Trainer.TrainerPhone,
                EntityId = coach.TrainerId,
                EntityName = "Trainer"

            };


            var result = await _userManager.CreateAsync(user, Trainer.Password);

            if (result.Succeeded)
            {
                    _toastNotification.AddSuccessToastMessage("Trainer Added Successfully");
                    return RedirectToPage ("Index");

            }
                if (TrainerCategories.Count != 0)
                {
                    _context.CategoryTrainers.RemoveRange(TrainerCategories);
                }
            
            _context.Trainers.Remove(coach);
            _context.SaveChanges();
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
               

            }
            catch(Exception e)
            {
                ModelState.AddModelError(string.Empty, e.Message);
                
            }
            _toastNotification.AddErrorToastMessage("SomeThing Went Wrong");
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
        public IActionResult OnGetGetDepartments(int id)
        {
            try
            {

                if (id != 0)
                {


                    var departments = _context.Departments.Where(b => !b.IsDeleted && b.IsActive &&  b.BranchId == id).Select(b => new { b.DepartmentName, b.DepartmentId }).ToList();


                    return new JsonResult(departments);
                }

                return new JsonResult("SomeThing Went Wrong");
            }
            catch (Exception ex)
            {
                return new JsonResult(ex.Message);
            }


        }

        public async Task<IActionResult> OnGetCategoriesDepartment(int departmentId)
        {
            try
            {

                if (departmentId != 0)
                {

                    // Retrieve categories based on the department ID
                    var categoriesInDepartment = _context.Categories
                    .Where(e => !e.IsDeleted && e.IsActive && e.DepartmentId == departmentId).Select(c => new { c.CategoryId, c.CategoryName })
                    .ToList();


                    return new JsonResult(categoriesInDepartment);
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
