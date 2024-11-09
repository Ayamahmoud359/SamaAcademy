using Academy.Data;
using DevExtreme.AspNet.Data;
using DevExtreme.AspNet.Mvc;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Academy.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class DataController : Controller
    {
       private readonly AcademyContext _context;

        public DataController(AcademyContext context)
        {
            _context = context;
        }
        [HttpGet]
        public async Task<IActionResult> GetDepartments(DataSourceLoadOptions loadOptions)
        {
            var Depts= _context.Departments.Include(d=>d.Branch).Where(d=>d.IsActive&&d.Branch.IsActive).Select(i => new
            {
                i.DepartmentName,
                i.DepartmentId,
                i.BranchId
            });
            return Json(await DataSourceLoader.LoadAsync(Depts, loadOptions));


        }
        [HttpGet]
        public async Task<IActionResult> BranchsLookup(DataSourceLoadOptions loadOptions)
        {
           

            var lookup = from i in _context.Branches
                         where  i.IsActive
                         orderby i.BranchName
                         select new
                         {
                             Value = i.BranchId,
                             Text = i.BranchName
                         };
            return Json(await DataSourceLoader.LoadAsync(lookup, loadOptions));
        }

        [HttpGet]
        public async Task<IActionResult> GetBranchs(DataSourceLoadOptions loadOptions)
        {
            var Branchs = _context.Branches.Where(d => d.IsActive).Select(i => new
            {
                i.BranchName,
                i.BranchAddress,
                i.BranchId
            });
            return Json(await DataSourceLoader.LoadAsync(Branchs, loadOptions));


        }

    }
}
