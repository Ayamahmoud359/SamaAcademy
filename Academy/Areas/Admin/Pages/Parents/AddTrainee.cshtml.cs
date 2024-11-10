using Academy.Data;
using Academy.DTO;
using Academy.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Academy.Areas.Admin.Pages.Parents
{
    public class AddTraineeModel : PageModel
    {
        [BindProperty]
        public TraineeVM trainee { get; set; }

        [BindProperty]
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress]
        [Display(Name = "Email")]
        [Remote("IsEmailAvailable", "Functions", ErrorMessage = "This email is already taken.")]
        public string Email { get; set; }


        public List<SubscriptionVM> SubscriptionVMs { get; set; } = new List<SubscriptionVM>();

        ////[BindProperty]
        //[Required(ErrorMessage = "Please select category.")]
        //public List<int> SelectedCategories { get; set; }
        ////[BindProperty]
        //[Required(ErrorMessage = "Please select Department.")]
        //public List<int> SelectedDepartments { get; set; }
        public List<Branch> Branches { get; set; }

        public List<Department> Departments { get; set; }
        public List<Category> Categories { get; set; }
        public List<SelectListItem> Nationalities { get; set; }

        private readonly AcademyContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        public AddTraineeModel(AcademyContext context, UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            trainee = new TraineeVM();

            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;


        }


        public async Task<IActionResult> OnGet(int id)
        {
            try
            {

                if (id != 0)
                {
                    trainee.ParentId = id;

                    Branches = _context.Branches.Where(b =>!b.IsDeleted && b.IsActive ).ToList();
                    Departments = new List<Department>();
                    Categories = new List<Category>();
                    Nationalities = new List<SelectListItem>
        {
            new SelectListItem { Text = "American", Value = "US" },
            new SelectListItem { Text = "Canadian", Value = "CA" },
            new SelectListItem { Text = "Mexican", Value = "MX" },
            new SelectListItem { Text = "British", Value = "GB" },
            new SelectListItem { Text = "German", Value = "DE" },
            new SelectListItem { Text = "Indian", Value = "IN" },
            new SelectListItem { Text = "Australian", Value = "AU" },
        };
                    return Page();
                }
                return RedirectToPage("../NotFound");
            }
            catch (Exception e)
            {
                return RedirectToPage("../Error");
            }


        }
    

            public async Task<IActionResult> OnPostAsync()
            {
                Branches = _context.Branches.Where(b => !b.IsDeleted && b.IsActive ).ToList();
                Departments = _context.Departments.Where(d => !d.IsDeleted && d.IsActive &&  d.BranchId == trainee.BranchId).ToList();
                //if (trainee.DepartmentId!=null&&trainee.DepartmentId != 0)
                //  {
                //Categories = _context.Categories.Where(d => !d.IsDeleted && d.IsActive && d.DepartmentId == trainee.DepartmentId).ToList();
                //  }
               
                if (!ModelState.IsValid)
                {
                    return Page();
                }
                try
                {
                var newtrainee = new Trainee
                {
                    TraineeName = trainee.TraineeName,
                    TraineePhone=trainee.TraineePhone,
                    TraineeAddress=trainee.TraineeAddress,
                    BranchId=trainee.BranchId,
                    TraineeEmail=trainee.Email,
                    BirthDate=trainee.BirthDate,
                    ParentId=trainee.ParentId,
                    Nationality=trainee.Nationality,
                    ResidencyNumber=trainee.ResidencyNumber,
                    IsActive = true,


                };
                //if (trainee.BranchId != null && trainee.DepartmentId != null && trainee.CategoryId != null)
                //{
                //    if (trainee.BranchId != 0 && trainee.DepartmentId != 0 && trainee.CategoryId != 0)
                //    {
                //      newtrainee.Subscriptions.Add(new Subscription
                //            {
                //               StartDate=trainee.StartDate,
                //               EndDate=trainee.EndDate,
                //               DepartmentId=trainee.DepartmentId,
                //               CategoryId=trainee.CategoryId
                              
                //            });
              
                //    }
                //}
          
                _context.Trainees.Add(newtrainee);
                    await _context.SaveChangesAsync();
              
                    var user = new ApplicationUser
                    {
                        UserName = Email,
                        Email = Email,
                        PhoneNumber = trainee.TraineePhone,
                        EntityId = newtrainee.TraineeId,
                        EntityName = "Trainee"

                    };


                    var result = await _userManager.CreateAsync(user, trainee.Password);

                    if (result.Succeeded)
                    {
                        return RedirectToPage("Admin/Trainees/Index");

                    }
                    if (newtrainee.Subscriptions.Count != 0)
                    {
                        _context.Subscriptions.RemoveRange(newtrainee.Subscriptions);
                    }

                    _context.Trainees.Remove(newtrainee);
                    _context.SaveChanges();
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                    return Page();


                }
                catch (Exception e)
                {
                    ModelState.AddModelError(string.Empty, e.Message);
                    return Page();
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

        }
    }
