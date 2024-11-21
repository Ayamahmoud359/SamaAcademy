using Academy.Data;
using Academy.DataGridVM;
using Academy.Models;
using DevExtreme.AspNet.Data;
using DevExtreme.AspNet.Mvc;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace Academy.Areas.Admin.Pages.Categories
{
    public class CategoryTrainersModel : PageModel
    {
        private readonly AcademyContext _context;



        public CategoryTrainersModel(AcademyContext context)
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
                data = _context.CategoryTrainers
                    .Include(a=>a.Trainer)
                    .Where(d => !d.IsDeleted &&d.IsActive
                &&!d.Trainer.IsDeleted
                &&d.Trainer.IsActive
                && d.CategoryId == id).Select(i => new TrainerCategoryDataGridVM()
                {
                    TrainerCategoriesId = i.TrainerCategoriesId,
                    TrainerName = i.Trainer.TrainerName,
                   
                    IsActive = i.IsActive

                }).ToList();
            }

            return new JsonResult(DataSourceLoader.Load(data, loadOptions));
        }


    }
}
