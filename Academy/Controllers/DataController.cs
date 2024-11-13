using Academy.Data;
using Academy.DataGridVM;
using Academy.Models;
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
            IQueryable<DepartmentDataGridVM> Depts = _context.Departments.Include(d=>d.Branch).Where(d=>!d.IsDeleted&&!d.Branch.IsDeleted).Select(i => new DepartmentDataGridVM()
            {
              DepartmentName=  i.DepartmentName,
              DepartmentId=   i.DepartmentId,
              BranchName=  i.Branch==null ? "":i.Branch.BranchName,
              IsActive=  i.IsActive

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
            IQueryable<TraineeDataGridVM> trainees = _context.Trainees.Include(e=>e.Parent).Where(d => !d.IsDeleted).Select(i => new TraineeDataGridVM()
            {
              TraineeName=  i.TraineeName,
              TraineeAddress=  i.TraineeAddress,
              ParentName=  i.Parent==null?"":i.Parent.ParentName,
             TraineeEmail=   i.TraineeEmail,
              IsActive= i.IsActive,
              TraineePhone=  i.TraineePhone,
              Nationality=  i.Nationality,
              BirthDate=  i.BirthDate,
              ResidencyNumber=  i.ResidencyNumber,
               TraineeId= i.TraineeId

            });
            return Json(await DataSourceLoader.LoadAsync(trainees, loadOptions));


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


        [HttpGet]
        public async Task<IActionResult> GetSubscriptions(DataSourceLoadOptions loadOptions)
        {
           
                var Allsubscriptions = _context.Subscriptions.Include(s => s.Trainee).Where(d => !d.IsDeleted).ToList();
                foreach (var item in Allsubscriptions)
                {
                    if (item.EndDate < DateOnly.FromDateTime(DateTime.Now))
                    {
                        item.IsActive = false;
                    }
                }
                _context.SaveChanges();

                IQueryable<SubscriptionDataGridVM> subscriptions = _context.Subscriptions.Include(s => s.Trainee)
                    .Include(s => s.Category).ThenInclude(s => s.Department)
                    .ThenInclude(s => s.Branch).Where(d =>
                    !d.IsDeleted && !d.Trainee.IsDeleted
                    && !d.Category.IsDeleted
                    && !d.Category.Department.IsDeleted
                    && !d.Category.Department.Branch.IsDeleted
                ).Select(i => new SubscriptionDataGridVM()
                {
                    SubscriptionId = i.SubscriptionId,
                    StartDate = i.StartDate,
                    EndDate = i.EndDate,
                    Branch = i.Branch,
                    Department = i.Department,
                    TraineeName = i.Trainee == null ? "" : i.Trainee.TraineeName,
                    CategoryName = i.Category == null ? "" : i.Category.CategoryName,
                    DepartmentName = i.Category.Department == null ? "" : i.Category.Department.DepartmentName,
                    BranchName = i.Category.Department.Branch == null ? "" : i.Category.Department.Branch.BranchName,
                    IsActive = i.IsActive

                });

                return Json(await DataSourceLoader.LoadAsync(subscriptions, loadOptions));
          
            


        }

        [HttpGet]
        public async Task<IActionResult> GetCategories(DataSourceLoadOptions loadOptions)
        {
        
            var Categories = _context.Categories.Include(s => s.Department).ThenInclude(c=>c.Branch).Where(d => !d.IsDeleted && !d.Department.IsDeleted&&!d.Department.Branch.IsDeleted).Select(i => new
            {
               i.CategoryName,
               i.CategoryId,
               i.Department.DepartmentName,
               i.Department.Branch.BranchName,
                i.IsActive

            });

            return Json(await DataSourceLoader.LoadAsync(Categories, loadOptions));


        }

        [HttpGet]
        public async Task<IActionResult> GetTrainers(DataSourceLoadOptions loadOptions)
        {

           IQueryable<TrainerDataGridVM>  Trainers = _context.Trainers.Where(d => !d.IsDeleted).Select(i => new TrainerDataGridVM()
            {
              TrainerName=  i.TrainerName,
              TrainerId=  i.TrainerId,
              TrainerPhone= i.TrainerPhone,
               HiringDate=  i.HiringDate,
              TrainerAddress= i.TrainerAddress,
               TrainerEmail= i.TrainerEmail,
               CurrentBranchName=_context.Branches.FirstOrDefault(b=>b.BranchId==i.CurrentBranch)==null?"": _context.Branches.FirstOrDefault(b => b.BranchId == i.CurrentBranch).BranchName,
              CurrentDepartmentName=_context.Departments.FirstOrDefault(d=>d.DepartmentId==i.CurrentDepartment)==null?"": _context.Departments.FirstOrDefault(d => d.DepartmentId == i.CurrentDepartment).DepartmentName,
              IsActive=  i.IsActive

            });

            return Json(await DataSourceLoader.LoadAsync(Trainers, loadOptions));


        }

        [HttpGet]
        public async Task<IActionResult> GetTrainerCategories(DataSourceLoadOptions loadOptions)
        {

            IQueryable<TrainerCategoryDataGridVM> TrainerCategories =
                _context.CategoryTrainers.Include(d=>d.Trainer)
                .Include(d=>d.Category).ThenInclude(d=>d.Department)
                .ThenInclude(d=>d.Branch)
                .Where(d => !d.IsDeleted&&d.IsActive&&!d.Category.IsDeleted
                &&d.Category.IsActive&&!d.Category.Department.IsDeleted
                &&d.Category.Department.IsActive &&!d.Category.Department.Branch.IsDeleted
            &&d.Category.Department.Branch.IsActive&&!d.Trainer.IsDeleted
            &&d.Trainer.IsActive)
                .Select(i => new TrainerCategoryDataGridVM()
            {
              TrainerCategoriesId=i.TrainerCategoriesId,
              TrainerName=i.Trainer.TrainerName,
              CategoryName=i.Category.CategoryName,
              BranchName=i.Category.Department.Branch.BranchName,
              DepartmentName=i.Category.Department.DepartmentName,
              IsActive=i.IsActive

            });

            return Json(await DataSourceLoader.LoadAsync(TrainerCategories, loadOptions));

        }
        }
    }
