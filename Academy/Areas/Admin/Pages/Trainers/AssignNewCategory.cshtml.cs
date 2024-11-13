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
        public AssignNewCategoryModel(AcademyContext context)
        {
            _context = context;
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
                var Trainer = _context.Trainers.FirstOrDefault(t => t.TrainerId == TrainerCategoryVM.TrainerId);
                if (Trainer != null)
                {
                    List<TrainerCategories> assignedCategories = new List<TrainerCategories>();
                    assignedCategories = _context.CategoryTrainers.Include(e => e.Trainer)
                        .Include(e => e.Category).ThenInclude(e => e.Department)
                        .ThenInclude(e => e.Branch).Where(c =>
                    !c.IsDeleted && c.IsActive
                    && !c.Category.IsDeleted
                    && !c.Category.Department.IsDeleted
                    && !c.Category.Department.Branch.IsDeleted
                    && !c.Trainer.IsDeleted
                    && c.Category.IsActive
                    && c.Category.Department.IsActive
                    && c.Category.Department.Branch.IsActive
                    && c.Trainer.IsActive &&
                    c.TrainerId == Trainer.TrainerId &&
                    c.Category.Department.DepartmentId == Trainer.CurrentDepartment
                    && c.Category.Department.Branch.BranchId == Trainer.CurrentBranch).ToList();
                    List<int> assignedCategoriesIdS = new List<int>();
                    foreach (var item in assignedCategories)
                    {

                    }
                    if (TrainerCategoryVM.BranchId == Trainer.CurrentBranch && TrainerCategoryVM.DepartmentId == Trainer.CurrentDepartment)
                    {

                    }
                    else
                    {

                    }

                   
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return Page();
            }

            return Page();
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
