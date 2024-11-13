using Academy.Data;
using Academy.DTO;
using Academy.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.CodeAnalysis.Operations;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using NToastNotify;
using static Azure.Core.HttpHeader;

namespace Academy.Areas.Admin.Pages.Departments
{
    public class IndexModel : PageModel
    {
      
        public CategoryVM cat { set; get; }
        private readonly AcademyContext _context;
        [BindProperty]
        public int deptid { set; get; }
        private readonly IToastNotification _toastNotification;
        public IndexModel(AcademyContext context , IToastNotification toastNotification)
        {
            _context = context;
            _toastNotification = toastNotification;
        }
        public void OnGet()
        {

        }

        public IActionResult OnPostAddCategory([FromBody] Category category)
        {
            try
            {
                //var category = JsonConvert.DeserializeObject<Category>(data);
                if (category != null)
                {
                    var department = _context.Departments.Include(d=>d.Branch).FirstOrDefault(d=>d.DepartmentId==category.DepartmentId);
                    if (department != null && (!department.IsActive||!department.Branch.IsActive))
                    {
                        return new JsonResult("Sorry ,You can't add new Category in this Department as This Department isn't Active or in Branch isn't Active");
                    }
                    Category newCategory = new Category()
                    {
                        CategoryName=category.CategoryName,
                        CategoryDescription=category.CategoryDescription,
                        DepartmentId=category.DepartmentId,
                        IsActive=true

                    };

                    _context.Categories.Add(newCategory);
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

        public async Task<IActionResult> OnPostDeleteDepartment()
        {
            try
            {
                var dept = _context.Departments.Find(deptid);
                if (dept != null)
                {
                    List<Absence> Absences = new List<Absence>();
                    Absences = _context.Abscenses.Include(a => a.Subscription)
                       .ThenInclude(a => a.Category)
                       .ThenInclude(a => a.Department)
                       .Where(a => a.Subscription.Category.Department.DepartmentId==deptid).ToList();
                    foreach (var item in Absences)
                    {
                        item.IsDeleted = true;
                    }
                    List<Exam> Exams = new List<Exam>();
                    Exams = _context.Exams.Include(a => a.Subscription)
                       .ThenInclude(a => a.Category)
                       .ThenInclude(a => a.Department)
                       .Where(a => a.Subscription.Category.Department.DepartmentId==deptid).ToList();
                    foreach (var item in Exams)
                    {
                        item.IsDeleted = true;
                    }
                    List<Subscription> subscriptions = new List<Subscription>();
                    subscriptions = _context.Subscriptions.Include(a => a.Category)

                       .ThenInclude(a => a.Department)
                       .Where(a => a.Category.Department.DepartmentId==deptid).ToList();
                    foreach (var item in subscriptions)
                    {
                        item.IsDeleted = true;
                        item.IsActive = false;
                    }
                    List<TrainerCategories> trainerCategories = new List<TrainerCategories>();
                    trainerCategories = _context.CategoryTrainers.Include(a => a.Category)

                        .ThenInclude(a => a.Department)
                        .Where(a => a.Category.Department.DepartmentId==deptid).ToList();
                    foreach (var item in trainerCategories)
                    {
                        item.IsDeleted = true;
                        item.IsActive = false;
                    }
                    List<Category> categories = new List<Category>();
                    categories = _context.Categories.Include(a => a.Department)
                        .Where(a => a.Department.DepartmentId==deptid).ToList();
                    foreach (var item in categories)
                    {
                        item.IsDeleted = true;
                        item.IsActive = false;
                    }
                    List<TraineeChampion> traineeChampions = new List<TraineeChampion>();
                    traineeChampions = _context.TraineeChampions.Include(a => a.Champion)
                       .ThenInclude(a => a.Department)
                    
                       .Where(a => a.Champion.Department.DepartmentId==deptid).ToList();
                    foreach (var item in traineeChampions)
                    {
                        item.IsDeleted = true;
                        item.IsActive = false;
                    }
                    List<Champion> champions = new List<Champion>();
                    champions = _context.Champions.Include(a => a.Department)
                        .Where(a => a.Department.DepartmentId==deptid).ToList();
                    foreach (var item in champions)
                    {
                        item.IsDeleted = true;
                        item.IsActive = false;
                    }


                    dept.IsActive = false;
                  
                    _context.Attach(dept).State = EntityState.Modified;
                    await _context.SaveChangesAsync();
                    _toastNotification.AddSuccessToastMessage("Deleted Successfull");

                    return RedirectToPage("Index");
                }
               
            }
            catch (Exception)

            {
                _toastNotification.AddErrorToastMessage("Something went wrong");
            }

            _toastNotification.AddErrorToastMessage("Something went wrong");
        
            return RedirectToPage("Index");

        }
    }
}
