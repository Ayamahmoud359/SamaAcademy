using Academy.Data;
using Academy.DataGridVM;
using Academy.Models;
using DevExtreme.AspNet.Data;
using DevExtreme.AspNet.Mvc;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace Academy.Areas.Admin.Pages.Trainers
{
    public class TrainerCategoriesModel : PageModel
    {
        private readonly AcademyContext _context;



        public TrainerCategoriesModel(AcademyContext context)
        {
            _context = context;

        }


        public int Id { get; set; }



        public async Task<IActionResult> OnGetAsync(int id)
        {

            try
            {


                if (id != 0)
                {
                    Id = id;
                    return Page();
                }
                return RedirectToPage("../NotFound");
            }
            catch (Exception e)
            {
                return RedirectToPage("../Error");
            }



        }
        public IActionResult OnGetGridData(DataSourceLoadOptions loadOptions, int id)
        {
            List<TrainerCategoryDataGridVM> data = new List<TrainerCategoryDataGridVM>();
            if (id != 0)
            {
                data = _context.CategoryTrainers.Include(a => a.Category)
                    .ThenInclude(a=>a.Department)
                    .ThenInclude(a=>a.Branch)
                .Where(d => !d.IsDeleted && d.IsActive
                && !d.Category.IsDeleted && d.Category.IsActive
                && !d.Category.Department.IsDeleted && d.Category.Department.IsActive
                && !d.Category.Department.Branch.IsDeleted && d.Category.Department.Branch.IsActive
                && d.TrainerId== id).Select(i => new TrainerCategoryDataGridVM()
                {
                    TrainerCategoriesId = i.TrainerCategoriesId,
            
                    CategoryName = i.Category.CategoryName,
                    BranchName = i.Category.Department.Branch.BranchName,
                    DepartmentName = i.Category.Department.DepartmentName,
                    IsActive = i.IsActive

                }).ToList();
            }

            return new JsonResult(DataSourceLoader.Load(data, loadOptions));
        }


    }
}

