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

namespace Academy.Areas.Admin.Pages.AddTrainer
{
    public class IndexModel : PageModel
    {
        [BindProperty]
        public TrainerVM Trainer { get; set; }
        // Property to hold the selected values
       
        [BindProperty]
        public List<int> SelectedCategories { get; set; }
        public List<Branch> Branches { get; set; }
        public List<Department> Departments { get; set; }
        public CategoriesDepartmentsVM categories { get; set; }

        private readonly AcademyContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        public IndexModel(AcademyContext context, UserManager<ApplicationUser> userManager,SignInManager<ApplicationUser> signInManager)
        {
            Trainer = new TrainerVM();
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
            categories= new CategoriesDepartmentsVM();

        }
        public void OnGet()
        {
            Branches = _context.Branches.ToList();
            Departments = _context.Departments.ToList();
        }


        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {
                var coach = new Trainer
                {
                    TrainerName = Trainer.TrainerName,
                    TrainerAddress = Trainer.TrainerAddress,
                    TrainerEmail = Trainer.Email,
                    TrainerPhone = Trainer.TrainerPhone,
                    BranchId = Trainer.BranchId,
                    DepartmentId = Trainer.DepartmentId,
                    IsActive = true,
                    
                    
                };
                try
                {
                    _context.Trainers.Add(coach);
                    await _context.SaveChangesAsync();
                }

               
                catch(Exception exc)
                {
                    ModelState.AddModelError(string.Empty, exc.Message);
                    return Page();

                }
                var categoryTrainer = new List<CategoryTrainers>();
                foreach (var category in SelectedCategories)
                {
                    categoryTrainer.Add(new CategoryTrainers
                    {
                        TrainerId = coach.TrainerId,
                        CategoryId = category
                    });
                }
                try
                {
                    _context.CategoryTrainers.AddRange(categoryTrainer);
                    await _context.SaveChangesAsync();
                }
                catch (Exception exc)
                {
                    ModelState.AddModelError(string.Empty, exc.Message);
                    return Page();
                }

                var user = new ApplicationUser
                {
                    UserName = Trainer.Email,
                    Email = Trainer.Email,
                    
                    PhoneNumber = Trainer.TrainerPhone,
                    EntityId= coach.TrainerId,
                    EntityName= "Trainer"
                    
                };

                try
                {
                    var result = await _userManager.CreateAsync(user, Trainer.Password);

                    if (result.Succeeded)
                    {
                        Redirect("/Admin/Index");
                        
                    }
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                    // If we got this far, something failed, redisplay form
                    return Page();
                }
                catch(Exception exc)
                {
                    ModelState.AddModelError(string.Empty, exc.Message);
                    
                }
               
            }
            return Redirect("/add/Trainers/addTrainer");


        }

        public async Task<PartialViewResult> OnPostCategoriesDepartment(int departmentId)
        {
            try
            {
                // Retrieve categories based on the department ID
                var categoriesInDepartment = _context.Categories
                    .Where(e => e.DepartmentId == departmentId)
                    .ToList();

                // Assign the department ID and categories to the view model
                categories = new CategoriesDepartmentsVM
                {
                    DepartmentId = departmentId,
                    categories = categoriesInDepartment
                };

                // Return the partial view with the populated view model
                return Partial("Trainers/_categories", categories);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }




    }
}
