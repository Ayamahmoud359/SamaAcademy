using Academy.Data;
using Academy.DTO;
using Academy.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using NToastNotify;
using System.Diagnostics.Metrics;

namespace Academy.Areas.Admin.Pages.Trainers
{
    public class AssignNewCategoryModel : PageModel
    {
        [BindProperty]
        public TrainerCategoryVM TrainerCategoryVM{ set; get; }
        private readonly AcademyContext _context;
        private readonly IToastNotification _toastNotification;

        public AssignNewCategoryModel(AcademyContext context,IToastNotification toastNotification)
        {
            _context = context;
            _toastNotification = toastNotification;
            TrainerCategoryVM = new TrainerCategoryVM();
            
        }
        public List<Branch> Branches { get; set; }
        public List<Department> Departments { get; set; }
        public List<Category> Categories { get; set; }
        public async Task<IActionResult> OnGet(int id)
        {
            try
            {

                if (id != 0)
                {
                    TrainerCategoryVM.TrainerId = id;
                    Branches = _context.Branches.Where(b => !b.IsDeleted && b.IsActive).ToList();
                    Departments = new List<Department>();
                    Categories = new List<Category>();

                    return Page();
                }
                return RedirectToPage("../NotFound");
            }
            catch (Exception e)
            {
                return RedirectToPage("../Error");
            }
          
        }
        public IActionResult OnPost()
        {
            try
            {
                Branches = _context.Branches.Where(b => !b.IsDeleted && b.IsActive).ToList();
                Departments = _context.Departments.Where(b => !b.IsDeleted && b.IsActive&&b.BranchId==TrainerCategoryVM.BranchId).ToList();
                Categories = _context.Categories.Where(b => !b.IsDeleted && b.IsActive && b.DepartmentId == TrainerCategoryVM.DepartmentId).ToList(); ;
                var Trainer = _context.Trainers.FirstOrDefault(t => t.TrainerId == TrainerCategoryVM.TrainerId);
                if (Trainer != null)
                {
                    if (!Trainer.IsActive )
                    {
                        ModelState.AddModelError(string.Empty, "Sorry ,You can't assign this trainer to  new Categories as This Trainer isn't Active ");
                        _toastNotification.AddErrorToastMessage("Trainer Isn't Active");
                        return Page();

                    }
                    List<TrainerCategories> assignedCategories = new List<TrainerCategories>();
                    assignedCategories = _context.CategoryTrainers.Include(e => e.Trainer)
                        .Include(e => e.Category).ThenInclude(e => e.Department)
                        .ThenInclude(e => e.Branch).Where(c =>
                    !c.IsDeleted && c.IsActive
                    && !c.Category.IsDeleted
                    && !c.Category.Department.IsDeleted
                    && !c.Category.Department.Branch.IsDeleted
                    && c.Category.IsActive
                    && c.Category.Department.IsActive
                    && c.Category.Department.Branch.IsActive
                    &&
                    c.TrainerId == Trainer.TrainerId &&
                    c.Category.Department.DepartmentId == Trainer.CurrentDepartment
                    && c.Category.Department.Branch.BranchId == Trainer.CurrentBranch).ToList();

                  
                    List<TrainerCategories> trainerCategories = new List<TrainerCategories>();
                    if (TrainerCategoryVM.BranchId == Trainer.CurrentBranch && TrainerCategoryVM.DepartmentId == Trainer.CurrentDepartment)
                    {
                        if (TrainerCategoryVM.SelectedCategories != null && TrainerCategoryVM.SelectedCategories.Count() != 0)
                        {
                           
                            foreach(var item in TrainerCategoryVM.SelectedCategories)
                            {
                               if(!assignedCategories.Any(a => a.CategoryId == item))
                                {
                                    trainerCategories.Add(new TrainerCategories()
                                    {
                                        CategoryId = item,
                                        TrainerId = TrainerCategoryVM.TrainerId,
                                        IsActive = true

                                    });
                                }
                              
                            }
                            _context.CategoryTrainers.AddRange(trainerCategories);
                            _context.SaveChanges();
                            _toastNotification.AddSuccessToastMessage("Trainer Is Assigned To categories Successfully");
                            return Redirect("~/Admin/TrainerCategoriesManagment/Index");
                           
                        }
                    }
                    else
                    {
                        Trainer.CurrentBranch = TrainerCategoryVM.BranchId;
                        Trainer.CurrentDepartment = TrainerCategoryVM.DepartmentId;
                        if (assignedCategories != null && assignedCategories.Count() != 0)
                        {
                            foreach(var item in assignedCategories)
                            {
                                item.IsActive = false;
                                item.IsDeleted = true;
                            }
                        }

                        if (TrainerCategoryVM.SelectedCategories != null && TrainerCategoryVM.SelectedCategories.Count() != 0)
                        {

                            foreach (var item in TrainerCategoryVM.SelectedCategories)
                            {
                                trainerCategories.Add(new TrainerCategories()
                                {
                                    CategoryId = item,
                                    TrainerId = TrainerCategoryVM.TrainerId,
                                    IsActive = true

                                });
                            }
                            _context.CategoryTrainers.AddRange(trainerCategories);

                        }
                        _context.SaveChanges();
                        _toastNotification.AddSuccessToastMessage("Trainer Is Assigned To categories Successfully");
                        return Redirect("~/Admin/TrainerCategoriesManagment/Index");
                    }


                    }
                _toastNotification.AddErrorToastMessage("SomeThing Went Wrong");
                return Page();
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                _toastNotification.AddErrorToastMessage("SomeThing Went Wrong");
                return Page();
            }

         
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
