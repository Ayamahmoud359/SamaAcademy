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
            var Depts= _context.Departments.Include(d=>d.Branch).Where(d=>!d.IsDeleted&&!d.Branch.IsDeleted&&d.Branch.IsActive).Select(i => new
            {
                i.DepartmentName,
                i.DepartmentId,
                i.BranchId,
                i.IsActive

            });
            return Json(await DataSourceLoader.LoadAsync(Depts, loadOptions));


        }
        [HttpGet]
        public async Task<IActionResult> BranchsLookup(DataSourceLoadOptions loadOptions)
        {
           

            var lookup = from i in _context.Branches
                         where  !i.IsDeleted &&i.IsActive
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
            var Branchs = _context.Branches.Where(d => !d.IsDeleted).Select(i => new
            {
                i.BranchName,
                i.BranchAddress,
                i.BranchId,
                i.IsActive
                
            });
            return Json(await DataSourceLoader.LoadAsync(Branchs, loadOptions));


        }

        [HttpGet]
        public async Task<IActionResult> GetParents(DataSourceLoadOptions loadOptions)
        {
            var barents = _context.Parents.Where(d => !d.IsDeleted).Select(i => new
            {
                i.ParentName,
                i.ParentAddress,
                i.ParentId,
                i.ParentPhone,
                i.IsActive,
                i.ParentEmail
                
            });
            return Json(await DataSourceLoader.LoadAsync(barents, loadOptions));


        }

        [HttpGet]
        public async Task<IActionResult> GetTrainees(DataSourceLoadOptions loadOptions)
        {
            var barents = _context.Trainees.Where(d => !d.IsDeleted&&!d.Parent.IsDeleted).Select(i => new
            {
                i.TraineeName,
                i.TraineeAddress,
                i.ParentId,
                i.TraineeEmail,
                i.IsActive,
                i.TraineePhone,
                i.Nationality,
                i.BirthDate,
                i.ResidencyNumber,
                i.TraineeId

            });
            return Json(await DataSourceLoader.LoadAsync(barents, loadOptions));


        }

        [HttpGet]
        public async Task<IActionResult> ParentsLookup(DataSourceLoadOptions loadOptions)
        {


            var lookup = from i in _context.Parents
                         where !i.IsDeleted 
                         orderby i.ParentName
                         select new
                         {
                             Value = i.ParentId,
                             Text = i.ParentName
                         };
            return Json(await DataSourceLoader.LoadAsync(lookup, loadOptions));
        }

    }
}
