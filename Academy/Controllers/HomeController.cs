using Academy.Data;

using Microsoft.AspNetCore.Mvc;

namespace Academy.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class HomeController : Controller
    {
       private readonly AcademyContext _context;

        public HomeController(AcademyContext context)
        {
            _context = context;
        }
        //[HttpGet]
        //public async Task<IActionResult> GetDepartments(DataSourceLoadOptions loadOptions)
        //{
        //    var assets = _context.Departments.Select(i => new
        //    {
        //        i.DepartmentNameAR,
        //        i.DepartmentId,
        //        i.BranchId
        //    });
        //    return Json(await DataSourceLoader.LoadAsync(assets, loadOptions));


        //}
   
    }
}
