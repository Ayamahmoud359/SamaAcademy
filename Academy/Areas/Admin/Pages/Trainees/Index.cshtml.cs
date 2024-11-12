using Academy.Data;
using Academy.DTO;
using Academy.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using NToastNotify;

namespace Academy.Areas.Admin.Pages.Trainees
{
    public class IndexModel : PageModel
    {
        public SubscriptionVM subscription { set; get; }
        private readonly AcademyContext _context;
        [BindProperty]
        public int Traineeid { set; get; }
        private readonly IToastNotification _toastNotification;
        public List<Branch> Branches { get; set; }
        public List<Department> Departments { get; set; }
        public List<Category> Categories { get; set; }
        private readonly UserManager<ApplicationUser> _userManager;
        public IndexModel(AcademyContext context, IToastNotification toastNotification, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _toastNotification = toastNotification;
            _userManager = userManager;
        }
        public void OnGet()
        {
            Branches = _context.Branches.Where(b => !b.IsDeleted && b.IsActive).ToList();
            Departments = new List<Department>();
            Categories = new List<Category>();
        }

        public IActionResult OnPostAddSubscription([FromBody] SubscriptionVM subscription)
        {
            try
            {
                if (subscription != null)
                {
                    var traineee = _context.Trainees.FirstOrDefault(d => d.TraineeId == subscription.TraineeId);
                    if (traineee != null && !traineee.IsActive )
                    {
                        return new JsonResult("Sorry ,You can't add new Supscription for This Trianee as This Trainee isn't Active");
                    }
                    Subscription newsubscription = new Subscription()
                    {

                        IsActive = true,
                      StartDate=subscription.StartDate,
                      EndDate=subscription.EndDate,
                      Branch=subscription.BranchId,
                      Department=subscription.DepartmentId,
                      TraineeId=subscription.TraineeId,
                      CategoryId=subscription.CategoryId,

                    };
                    if (subscription.EndDate == subscription.StartDate || subscription.EndDate < subscription.StartDate)
                    {

                        return new JsonResult("EndDate must be greater than StartDate");
                    }
                
                    if(_context.Subscriptions.Any(s=>
                    (!s.IsDeleted
                    &&s.TraineeId==newsubscription.TraineeId
                    && s.Branch==newsubscription.Branch
                    &&s.Department==newsubscription.Department
                   && s.CategoryId==newsubscription.CategoryId
                   )
                   &&
                   (
                   s.EndDate > newsubscription.StartDate
                   ||s.EndDate>DateOnly.FromDateTime(DateTime.Now))
                    
                    ))
                    {
                        return new JsonResult("You have an active subscription on this department ,you can subscripe after your current subscription ended");
                    }

                    _context.Subscriptions.Add(newsubscription);
                    _context.SaveChanges();


                    return new JsonResult("Added");
                }

                return new JsonResult("SomeThing Went Wrong");
            }
            catch (Exception ex)
            {
                return new JsonResult(ex.Message);
            }


        }

        public async Task<IActionResult> OnPostDeleteTrainee()
        {
            try
            {
                var trainee = _context.Trainees.Find(Traineeid);
                if (trainee != null)
                {
                    trainee.IsActive = false;
                    trainee.IsDeleted = true;
                    _context.Attach(trainee).State = EntityState.Modified;
                    var user = await _userManager.FindByNameAsync(trainee.TraineeEmail);
                    if (user != null)
                    {
                        var result = await _userManager.DeleteAsync(user);
                        if (result.Succeeded)
                        {

                            await _context.SaveChangesAsync();
                            _toastNotification.AddSuccessToastMessage("Deleted Successfully");
                            return RedirectToPage("Index");
                        }

                    }
                   
                }

            }
            catch (Exception)

            {
                _toastNotification.AddErrorToastMessage("Something went wrong");
            }
            _toastNotification.AddErrorToastMessage("Something went wrong");
            return RedirectToPage("Index");

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
