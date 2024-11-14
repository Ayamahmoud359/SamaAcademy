using Academy.Data;
using Academy.DTO;
using Academy.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using NToastNotify;

namespace Academy.Areas.Admin.Pages.Branchs
{
    public class IndexModel : PageModel
    {
       
        public DepartmentVM Dept{ set; get; }
        private readonly AcademyContext _context;
        [BindProperty]
        public int Branchid { set; get; }
        private readonly IToastNotification _toastNotification;
        public IndexModel(AcademyContext context, IToastNotification toastNotification)
        {
            _context = context;
            _toastNotification = toastNotification;
        }
        public void OnGet()
        {

        }

        public IActionResult OnPostAddDepartment([FromBody] Department dept)
        {
            try
            {
                //var category = JsonConvert.DeserializeObject<Category>(data);
                if (dept != null)
                {
                    var branch = _context.Branches.Find(dept.BranchId);
                   if (branch!=null&&!branch.IsActive)
                    {
                        return new JsonResult("Sorry ,You can't add new Department in this Branch as This Branch isn't Active");
                    }
                    Department department = new Department()
                    {

                        IsActive = true,
                        DepartmentName = dept.DepartmentName,
                        DepartmentDescription = dept.DepartmentDescription,
                        BranchId = dept.BranchId

                    };

                    _context.Departments.Add(department);
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

        public async Task<IActionResult> OnPostDeleteBranch()
        {
            try
            {
               
                var branch = _context.Branches.Find(Branchid);
                if (branch != null)
                {
                    List<Absence> Absences = new List<Absence>();
                    Absences = _context.Abscenses.Include(a => a.Subscription)
                       .ThenInclude(a => a.Category)
                       .ThenInclude(a => a.Department).ThenInclude(a => a.Branch)
                       .Where(a => a.Subscription.Category.Department.Branch.BranchId == Branchid).ToList();
                    foreach(var item in Absences)
                    {
                        item.IsDeleted = true;
                    }
                    List<Exam> Exams = new List<Exam>();
                    Exams = _context.Exams.Include(a => a.Subscription)
                       .ThenInclude(a => a.Category)
                       .ThenInclude(a => a.Department).ThenInclude(a => a.Branch)
                       .Where(a => a.Subscription.Category.Department.Branch.BranchId == Branchid).ToList();
                    foreach (var item in Exams)
                    {
                        item.IsDeleted = true;
                    }
                    List<Subscription> subscriptions = new List<Subscription>();
                    subscriptions = _context.Subscriptions.Include(a => a.Category)
                       
                       .ThenInclude(a => a.Department).ThenInclude(a => a.Branch)
                       .Where(a => a.Category.Department.Branch.BranchId == Branchid).ToList();
                    foreach (var item in subscriptions)
                    {
                        item.IsDeleted = true;
                        item.IsActive = false;
                    }
                    List<TrainerCategories> trainerCategories = new List<TrainerCategories>();
                   trainerCategories = _context.CategoryTrainers.Include(a => a.Category)

                       .ThenInclude(a => a.Department).ThenInclude(a => a.Branch)
                       .Where(a => a.Category.Department.Branch.BranchId == Branchid).ToList();
                    foreach (var item in trainerCategories)
                    {
                        item.IsDeleted = true;
                        item.IsActive = false;
                    }
                    List<Category> categories = new List<Category>();
                    categories = _context.Categories.Include(a => a.Department).ThenInclude(a => a.Branch)
                        .Where(a => a.Department.Branch.BranchId == Branchid).ToList();
                    foreach (var item in categories)
                    {
                        item.IsDeleted = true;
                        item.IsActive = false;
                    }
                    List<TraineeChampion> traineeChampions = new List<TraineeChampion>();
                    traineeChampions = _context.TraineeChampions.Include(a => a.Champion)
                       .ThenInclude(a => a.Department)
                       .ThenInclude(a => a.Branch)
                       .Where(a => a.Champion.Department.Branch.BranchId == Branchid).ToList();
                    foreach (var item in traineeChampions)
                    {
                        item.IsDeleted = true;
                        item.IsActive = false;
                    }
                    List<Champion> champions = new List<Champion>();
                    champions= _context.Champions.Include(a => a.Department).ThenInclude(a => a.Branch)
                        .Where(a => a.Department.Branch.BranchId == Branchid).ToList();
                    foreach (var item in champions)
                    {
                        item.IsDeleted = true;
                        item.IsActive = false;
                    }


                    List<Department> departments = new List<Department>();
                    departments = _context.Departments.Include(a => a.Branch)
                        .Where(a => a.Branch.BranchId == Branchid).ToList();
                    foreach (var item in departments)
                    {
                        item.IsDeleted = true;
                        item.IsActive = false;
                    }


                    branch.IsActive = false;
                    branch.IsDeleted = true;
                  
                    _context.Attach(branch).State = EntityState.Modified;
                    await _context.SaveChangesAsync();
                    _toastNotification.AddSuccessToastMessage("Deleted Successfully");
                    return RedirectToPage("Index");
                }

               
            }
            catch (Exception)

            {
               
            }
            _toastNotification.AddErrorToastMessage("Something went wrong");
        
            return RedirectToPage("Index");

        }
    }
}
