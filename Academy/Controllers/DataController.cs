using Academy.Data;
using Academy.DataGridVM;
using Academy.DTO;
using Academy.Models;
using Academy.ViewModels;
using DevExtreme.AspNet.Data;
using DevExtreme.AspNet.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Academy.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class DataController : Controller
    {
        private readonly AcademyContext _context;

        private readonly UserManager<ApplicationUser> _UserManger;

        public DataController(AcademyContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _UserManger = userManager;
        }
        [HttpGet]
        public async Task<IActionResult> GetDepartments(DataSourceLoadOptions loadOptions)
        {
            IQueryable<DepartmentDataGridVM> Depts = _context.Departments.Include(d => d.Branch).Where(d => !d.IsDeleted && !d.Branch.IsDeleted && d.Branch.IsActive).Select(i => new DepartmentDataGridVM()
            {
                DepartmentName = i.DepartmentName,
                DepartmentId = i.DepartmentId,
                BranchName = i.Branch == null ? "" : i.Branch.BranchName,
                IsActive = i.IsActive,
                image = i.Image

            });


            return Json(await DataSourceLoader.LoadAsync(Depts, loadOptions));


        }
        [HttpGet]
        public async Task<IActionResult> GetUsers(DataSourceLoadOptions loadOptions)
        {
            IQueryable<UsersDataGridVM> users = _UserManger.Users.Select(i => new UsersDataGridVM()
            {
                Id = i.Id,
                FullName = i.FullName,
                Email = i.Email,
                Phone = i.PhoneNumber,
                UserName= i.UserName

            });
            return Json(await DataSourceLoader.LoadAsync(users, loadOptions));

        }
        [HttpGet]
        public async Task<IActionResult> BranchsLookup(DataSourceLoadOptions loadOptions)
        {


            var lookup = from i in _context.Branches
                         where !i.IsDeleted && i.IsActive
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
                i.IsActive,
                i.Image

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
                i.ParentEmail,
                i.Image

            });
            return Json(await DataSourceLoader.LoadAsync(barents, loadOptions));


        }

        [HttpGet]
        public async Task<IActionResult> GetTrainees(DataSourceLoadOptions loadOptions)
        {
            IQueryable<TraineeDataGridVM> trainees = _context.Trainees.Include(e => e.Parent).Where(d => !d.IsDeleted && !d.Parent.IsDeleted && d.Parent.IsActive).Select(i => new TraineeDataGridVM()
            {
                TraineeName = i.TraineeName,
                TraineeAddress = i.TraineeAddress,
                ParentName = i.Parent.IsDeleted ? "" : i.Parent.ParentName,
                TraineeEmail = i.TraineeEmail,
                IsActive = i.IsActive,
                TraineePhone = i.TraineePhone,
                Nationality = i.Nationality,
                BirthDate = i.BirthDate,
                ResidencyNumber = i.ResidencyNumber,
                TraineeId = i.TraineeId,
                image = i.Image


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

            var Allsubscriptions = _context.Subscriptions.Where(d => !d.IsDeleted).ToList();
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
                !d.IsDeleted
                && !d.Trainee.IsDeleted && d.Trainee.IsActive
                && !d.Category.IsDeleted && d.Category.IsActive
                && !d.Category.Department.IsDeleted && d.Category.Department.IsActive
                && !d.Category.Department.Branch.IsDeleted && d.Category.Department.Branch.IsActive
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

            IQueryable<CategoryDataGridVM> Categories = _context.Categories.Include(s => s.Department)
                .ThenInclude(c => c.Branch)
                .Where(d => !d.IsDeleted
                && !d.Department.IsDeleted && d.Department.IsActive
                && !d.Department.Branch.IsDeleted && d.Department.Branch.IsActive).Select(i => new CategoryDataGridVM()
                {
                    CategoryName = i.CategoryName,
                    CategoryId = i.CategoryId,
                    DepartmentName = i.Department.DepartmentName,
                    BranchName = i.Department.Branch.BranchName,
                    IsActive = i.IsActive,
                    image = i.image


                });

            return Json(await DataSourceLoader.LoadAsync(Categories, loadOptions));


        }

        [HttpGet]
        public async Task<IActionResult> GetTrainers(DataSourceLoadOptions loadOptions)
        {

            IQueryable<TrainerDataGridVM> Trainers = _context.Trainers
                 .Where(d => !d.IsDeleted).Select(i => new TrainerDataGridVM()
                 {
                     TrainerName = i.TrainerName,
                     TrainerId = i.TrainerId,
                     TrainerPhone = i.TrainerPhone,
                     HiringDate = i.HiringDate,
                     TrainerAddress = i.TrainerAddress,
                     TrainerEmail = i.TrainerEmail,
                     CurrentBranchName = _context.Branches.FirstOrDefault(b => b.BranchId == i.CurrentBranch) == null ? "" : _context.Branches.FirstOrDefault(b => b.BranchId == i.CurrentBranch).BranchName,
                     CurrentDepartmentName = _context.Departments.FirstOrDefault(d => d.DepartmentId == i.CurrentDepartment) == null ? "" : _context.Departments.FirstOrDefault(d => d.DepartmentId == i.CurrentDepartment).DepartmentName,
                     IsActive = i.IsActive,
                     image = i.Image

                 });

            return Json(await DataSourceLoader.LoadAsync(Trainers, loadOptions));


        }

        [HttpGet]
        public async Task<IActionResult> GetTrainerCategories(DataSourceLoadOptions loadOptions)
        {

            IQueryable<TrainerCategoryDataGridVM> TrainerCategories =
                _context.CategoryTrainers.Include(d => d.Trainer)
                .Include(d => d.Category).ThenInclude(d => d.Department)
                .ThenInclude(d => d.Branch)
                .Where(d => !d.IsDeleted && d.IsActive && !d.Category.IsDeleted
                && d.Category.IsActive && !d.Category.Department.IsDeleted
                && d.Category.Department.IsActive && !d.Category.Department.Branch.IsDeleted
            && d.Category.Department.Branch.IsActive && !d.Trainer.IsDeleted
            && d.Trainer.IsActive)
                .Select(i => new TrainerCategoryDataGridVM()
                {
                    TrainerCategoriesId = i.TrainerCategoriesId,
                    TrainerName = i.Trainer.TrainerName,
                    CategoryName = i.Category.CategoryName,
                    BranchName = i.Category.Department.Branch.BranchName,
                    DepartmentName = i.Category.Department.DepartmentName,
                    IsActive = i.IsActive

                });

            return Json(await DataSourceLoader.LoadAsync(TrainerCategories, loadOptions));

        }

        [HttpGet]
        public async Task<IActionResult> GetApsences(DataSourceLoadOptions loadOptions)
        {
            var Allsubscriptions = _context.Subscriptions.Where(d => !d.IsDeleted).ToList();
            foreach (var item in Allsubscriptions)
            {
                if (item.EndDate < DateOnly.FromDateTime(DateTime.Now))
                {
                    item.IsActive = false;
                }


            }
            _context.SaveChanges();

            IQueryable<AbsenceDataGridVM> apsences =
                _context.Abscenses.Include(d => d.Trainer).Include(d => d.Subscription)
                .ThenInclude(d => d.Trainee).Include(d => d.Subscription)
                .ThenInclude(d => d.Category).ThenInclude(d => d.Department)
                .ThenInclude(d => d.Branch)
                .Where(d => !d.IsDeleted
                && !d.Subscription.IsDeleted
                &&d.Subscription.IsActive
                && !d.Subscription.Trainee.IsDeleted
                && d.Subscription.Trainee.IsActive
                && !d.Subscription.Category.IsDeleted
                && d.Subscription.Category.IsActive
                && !d.Subscription.Category.Department.IsDeleted
                && d.Subscription.Category.Department.IsActive
                && !d.Subscription.Category.Department.Branch.IsDeleted
                && d.Subscription.Category.Department.Branch.IsActive)
                .Select(i => new AbsenceDataGridVM()
                {
                    AbsenceId = i.AbsenceId,
                    AbsenceDate = i.AbsenceDate,
                    IsAbsent = i.IsAbsent,
                    StartDate = i.Subscription.StartDate,
                    EndDate = i.Subscription.EndDate,
                    SubscriptionId = i.SubscriptionId,
                    Type = i.Type,
                    TrainerId = i.TrainerId,
                    TrainerName = i.Trainer.IsDeleted ? "" : i.Trainer.TrainerName,
                    CategoryName = i.Subscription.Category.CategoryName,
                    BranchName = i.Subscription.Category.Department.Branch.BranchName,
                    DepartmentName = i.Subscription.Category.Department.DepartmentName,
                    TraineeName = i.Subscription.Trainee.TraineeName

                });

            return Json(await DataSourceLoader.LoadAsync(apsences, loadOptions));

        }
        [HttpGet]
        public async Task<IActionResult> GetExams(DataSourceLoadOptions loadOptions)
        {
            var Allsubscriptions = _context.Subscriptions.Where(d => !d.IsDeleted).ToList();
            foreach (var item in Allsubscriptions)
            {
                if (item.EndDate < DateOnly.FromDateTime(DateTime.Now))
                {
                    item.IsActive = false;
                }


            }
            _context.SaveChanges();

            IQueryable<ExamDataGridVM> Evaluations =
                _context.Exams.Include(d => d.Trainer).Include(d => d.Subscription)
                .ThenInclude(d => d.Trainee).Include(d => d.Subscription)
                .ThenInclude(d => d.Category).ThenInclude(d => d.Department)
                .ThenInclude(d => d.Branch)
                .Where(d => !d.IsDeleted
                && !d.Subscription.IsDeleted
                &&d.Subscription.IsActive 
                && !d.Subscription.Trainee.IsDeleted
                && d.Subscription.Trainee.IsActive
                && !d.Subscription.Category.IsDeleted
                && d.Subscription.Category.IsActive
                && !d.Subscription.Category.Department.IsDeleted
                && d.Subscription.Category.Department.IsActive
                && !d.Subscription.Category.Department.Branch.IsDeleted
                && d.Subscription.Category.Department.Branch.IsActive)
                .Select(i => new ExamDataGridVM()
                {
                    ExamId = i.ExamId,
                    ExamDate = i.ExamDate,
                    Score = i.Score,
                    StartDate = i.Subscription.StartDate,
                    EndDate = i.Subscription.EndDate,
                    SubscriptionId = i.SubscriptionId,
                    Review = i.Review,
                    TrainerId = i.TrainerId,
                    TrainerName = i.Trainer.IsDeleted?"":i.Trainer.TrainerName,
                    CategoryName = i.Subscription.Category.CategoryName,
                    BranchName = i.Subscription.Category.Department.Branch.BranchName,
                    DepartmentName = i.Subscription.Category.Department.DepartmentName,
                    TraineeName=i.Subscription.Trainee.TraineeName

                });

            return Json(await DataSourceLoader.LoadAsync(Evaluations, loadOptions));

        }
        [HttpGet]
        public async Task<IActionResult> GetChampions(DataSourceLoadOptions loadOptions)
        {

            IQueryable<ChampionDataGridVM> champions = _context.Champions

                .Where(d => !d.IsDeleted).Select(i => new ChampionDataGridVM()
                {
                    ChampionName = i.ChampionName,
                   ChampionId = i.ChampionId,
                   ChampionDate=i.ChampionDate,
                   ChampionDescription=i.ChampionDescription
                  
                });

            return Json(await DataSourceLoader.LoadAsync(champions, loadOptions));


        }

        [HttpGet]
        public async Task<IActionResult> GetCompetitionDepartments(DataSourceLoadOptions loadOptions)
        {

            var CompetitionDepartments = _context.CompetitionDepartment

                .Where(d => !d.IsDeleted).Select(i => new 
                {
                    i.Name,
                    i.Id,
                    i.Description,
                    i.Image,
                    i.IsActive

                });

            return Json(await DataSourceLoader.LoadAsync(CompetitionDepartments, loadOptions));



        }

        [HttpGet]
        public async Task<IActionResult> GetCompetitionTeams(DataSourceLoadOptions loadOptions)
        {

           
            IQueryable<CompetitionTeamDataGridVM> CompetitionTeams = _context.CompetitionTeam
                .Include(e => e.CompetitionDepartment).Include(e => e.Trainer)

                .Where(d => !d.IsDeleted && !d.CompetitionDepartment.IsDeleted && d.CompetitionDepartment.IsActive)
                .Select(i => new CompetitionTeamDataGridVM()
             {
                    Id=i.Id,
                    TrainerId=i.TrainerId,
                    CompetitionDepartmentId=i.CompetitionDepartmentId,
                    TeamName=i.Name,
                    TrainerName=i.Trainer.IsDeleted?"":i.Trainer.TrainerName,
                    CompetitionDepartmentName=i.CompetitionDepartment.Name,
                    IsActive=i.IsActive,
                    Image=i.Image

             });

           
            return Json(await DataSourceLoader.LoadAsync(CompetitionTeams, loadOptions));



        }

        [HttpGet]
        public async Task<IActionResult> GetTraineeCompetitionTeams(DataSourceLoadOptions loadOptions)
        {


            IQueryable<TraineeCompetitionTeamDataGridVM> TraineeCompetitionTeams = _context.TraineeCompetitionTeams
                .Include(e => e.Trainee).Include(e => e.CompetitionTeam)
                .ThenInclude(a=> a.CompetitionDepartment)
                 .Include(e => e.CompetitionTeam)
        .ThenInclude(a => a.Trainer)
                .Where(d => !d.IsDeleted&&d.IsActive
                &&!d.Trainee.IsDeleted
                &&d.Trainee.IsActive
                && !d.CompetitionTeam.IsDeleted
                && d.CompetitionTeam.IsActive&&
                !d.CompetitionTeam.CompetitionDepartment.IsDeleted
                &&d.CompetitionTeam.CompetitionDepartment.IsActive)
                .Select(i => new TraineeCompetitionTeamDataGridVM()
                {
                    Id = i.Id,
                    TraineeId = i.TraineeId,
                    TraineeName=i.Trainee.TraineeName,
                    CompetitionTeamId = i.CompetitionTeamId,
                    CompetitionTeam = i.CompetitionTeam.Name,
                    Trainer = i.CompetitionTeam.Trainer.IsDeleted ? "" : i.CompetitionTeam.Trainer.TrainerName,
                    CompetitionDepartment = i.CompetitionTeam.CompetitionDepartment.Name,
                    IsActive = i.IsActive,
                  
                });


            return Json(await DataSourceLoader.LoadAsync(TraineeCompetitionTeams, loadOptions));



        }

    }
}
