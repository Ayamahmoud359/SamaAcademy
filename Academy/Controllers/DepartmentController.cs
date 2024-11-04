using Academy.Data;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Academy.Controllers
{
    [Route("api/[controller]/[action]")]

    public class DepartmentController : Controller
    {
        private readonly AcademyContext _context;

        public DepartmentController(AcademyContext context)
        {
            _context = context;
        }
        //    [HttpGet]
        //    public async Task<IActionResult> GetDepartments(DataSourceLoadOptions loadOptions)
        //    {
        //        var assets = _context.Departments.Select(i => new
        //        {
        //            i.DepartmentNameAR,

        //            i.DepartmentId


        //        });
        //        return Json(await DataSourceLoader.LoadAsync(assets, loadOptions));


        //    }
        //} 
    }
}
