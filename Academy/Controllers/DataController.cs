using Academy.Data;
using DevExtreme.AspNet.Data;
using DevExtreme.AspNet.Mvc;
using Microsoft.AspNetCore.Mvc;

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
            var Depts= _context.Departments.Select(i => new
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
                      
                         orderby i.BranchName
                         select new
                         {
                             Value = i.BranchId,
                             Text = i.BranchName
                         };
            return Json(await DataSourceLoader.LoadAsync(lookup, loadOptions));
        }

    }
}
