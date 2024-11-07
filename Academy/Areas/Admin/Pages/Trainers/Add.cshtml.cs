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

namespace Academy.Areas.Admin.Pages.AddTrainer
{
    public class AddModel : PageModel
    {
        [BindProperty]
        public TrainerVM Trainer { get; set; }
        // Property to hold the selected values
        [BindProperty]
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress]
        [Display(Name = "Email")]
        [Remote("IsEmailAvailable", "Functions", ErrorMessage = "This email is already taken.")]
        public string Email { get; set; }
        [BindProperty]
        [Required(ErrorMessage = "Please select at least one category.")]
        public List<int> SelectedCategories { get; set; }
        public List<Branch> Branches { get; set; }
        public List<Department> Departments { get; set; }
        public List<Category> Categories { get; set; }

        private readonly AcademyContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        public AddModel(AcademyContext context, UserManager<ApplicationUser> userManager,SignInManager<ApplicationUser> signInManager)
        {
            Trainer = new TrainerVM();
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
           

        }
       
      
        public void OnGet()
        {
            Branches = _context.Branches.ToList();
            Departments = new List<Department>();
            Categories = new List<Category>();
        }
       
     
        public async Task<IActionResult> OnPostAsync()
        {
            Branches = _context.Branches.ToList();
            Departments =_context.Departments.Where(d=>d.BranchId== Trainer.BranchId).ToList();
            Categories = _context.Categories.Where(d => d.DepartmentId == Trainer.DepartmentId).ToList();
            if (!ModelState.IsValid)
            {
                return Page();
            }
            try
            {
                var coach = new Trainer
                {
                    TrainerName = Trainer.TrainerName,
                    TrainerAddress = Trainer.TrainerAddress,
                    TrainerEmail =Email,
                    TrainerPhone = Trainer.TrainerPhone,
                    BranchId = Trainer.BranchId,
                    DepartmentId = Trainer.DepartmentId,
                    IsActive = true,


                };

                _context.Trainers.Add(coach);
                await _context.SaveChangesAsync();
       
            var categoryTrainer = new List<TrainerCategories>();
            if (SelectedCategories != null && SelectedCategories.Count != 0)
            {
                foreach (var category in SelectedCategories)
                {
                    categoryTrainer.Add(new TrainerCategories
                    {
                        TrainerId = coach.TrainerId,
                        CategoryId = category
                    });
                }

                _context.CategoryTrainers.AddRange(categoryTrainer);
                await _context.SaveChangesAsync();
            }

            var user = new ApplicationUser
            {
                UserName = Email,
                Email = Email,

                PhoneNumber = Trainer.TrainerPhone,
                EntityId = coach.TrainerId,
                EntityName = "Trainer"

            };


            var result = await _userManager.CreateAsync(user, Trainer.Password);

            if (result.Succeeded)
            {
            return RedirectToPage ("Index");

            }
            _context.CategoryTrainers.RemoveRange(categoryTrainer);
            _context.Trainers.Remove(coach);
            _context.SaveChanges();
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
                return Page();
               

            }
            catch(Exception e)
            {
                ModelState.AddModelError(string.Empty, e.Message);
                return Page();
            }
        
        }

        public async Task<IActionResult> OnGetCategoriesDepartment(int departmentId)
        {
            try
            {
                // Retrieve categories based on the department ID
                var categoriesInDepartment = _context.Categories
                    .Where(e => e.DepartmentId == departmentId).Select(c=>new {c.CategoryId,c.CategoryName})
                    .ToList();

                // Assign the department ID and categories to the view model
            

                // Return the partial view with the populated view model
                return new JsonResult(categoriesInDepartment);
               
            }
            catch (Exception ex)
            {
                return new JsonResult(ex.Message); 
            }
        }
        public IActionResult OnGetGetDepartments(int id)
        {
            try
            {
                //var category = JsonConvert.DeserializeObject<Category>(data);
                if (id != 0)
                {


                   var  departments = _context.Departments.Where(b => b.BranchId == id).Select(b=> new { b.DepartmentName, b.DepartmentId }).ToList();


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
