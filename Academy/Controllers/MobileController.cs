using Academy.Data;
using Academy.DTO;
using Academy.Models;
using Academy.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace Academy.Controllers
{
    [Route("api/[controller]")]
    //[ApiKeyAuthorization] // Apply the attribute here
    [ApiController]
    public class MobileController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly AcademyContext _context;


        public MobileController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, AcademyContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
        }
        #region Login
        [HttpPost]
        [Route("Login")]
        public async Task<ActionResult<ApplicationUser>> Login([FromQuery] string login, [FromQuery] string password)
        {
            ApplicationUser user = null;

            // Determine if the input is an email or username
            if (login.Contains("@"))
            {
                // Assume it's an email
                user = await _userManager.FindByEmailAsync(login);
            }
            else
            {
                // Assume it's a username
                user = await _userManager.FindByNameAsync(login);
            }

            if (user != null)
            {
                // Check the password
                var result = await _signInManager.CheckPasswordSignInAsync(user, password, true);
                if (result.Succeeded)
                {
                    // Determine the user type and return the appropriate response
                    if (user.EntityId != null && user.EntityName == "Trainer")
                    {
                        var trainer = await _context.Trainers
                            .FirstOrDefaultAsync(e => e.TrainerId == user.EntityId);
                        return Ok(new { Status = true, Message = "User logged in successfully!", trainer });
                    }
                    if (user.EntityId != null && user.EntityName == "Parent")
                    {
                        var parent = await _context.Parents
                            .FirstOrDefaultAsync(e => e.ParentId == user.EntityId);
                        return Ok(new { Status = true, Message = "User logged in successfully!", parent });
                    }
                }
            }

            // Return an invalid response
            return Ok(new { Status = false, Message = "Invalid login credentials." });
        }

        #endregion
        #region GetDepartmetsById 
        [HttpGet]
        [Route("GetDepartmetsByBranchId")]
        public async Task<ActionResult<List<Department>>> GetDepartmetsByBranchId(int BranchId)
        {
            var departments = await _context.Departments.Where(e => e.BranchId == BranchId && e.IsActive).ToListAsync();
            return Ok(departments);
        }
        #endregion
        #region GetCategoriesByDepartmentId
        [HttpGet]
        [Route("GetCategoriesByDepartmentId")]
        public async Task<ActionResult<List<Category>>> GetCategoriesByDepartmentId(int DepartmentId)
        {
            try
            {
                var categories = await _context.Categories.Include(e => e.Department).Where(e => e.DepartmentId == DepartmentId && e.IsActive == true && e.Department.IsActive).Select(e => new
                {
                    e.CategoryId,
                    e.DepartmentId,
                    e.CategoryName,
                    e.CategoryDescription,
                    e.image,
                    e.IsActive
                }).ToListAsync();
                return Ok(new { Status = true, categories });
            }
            catch (Exception ex)
            {
                return Ok(new { Status = false, Message = ex.Message });
            }

        }
        #endregion
        #region GetDepartmentDetailsById
        [HttpGet]
        [Route("GetDepartmentDetailsById")]
        public async Task<ActionResult> GetDepartmentDetailsById(int DepartmentId)
        {
            try
            {
                var department = await _context.Departments.Where(e => e.DepartmentId == DepartmentId && e.IsActive).Select(e => new
                {
                    e.DepartmentId,
                    e.DepartmentName,
                    e.DepartmentDescription,
                    e.Image,
                    e.IsActive,
                    e.BranchId,
                    BranchName = _context.Branches.Where(a => a.IsActive && a.BranchId == e.BranchId).FirstOrDefault().BranchName,
                    BranchAddress = _context.Branches.Where(a => a.IsActive && a.BranchId == e.BranchId).FirstOrDefault().BranchAddress,
                    BranchPhone = _context.Branches.Where(a => a.IsActive && a.BranchId == e.BranchId).FirstOrDefault().Phone,
                    BranchImage = _context.Branches.Where(a => a.IsActive && a.BranchId == e.BranchId).FirstOrDefault().Image,
                    Categories = _context.Categories.Where(a => a.IsActive && a.DepartmentId == e.DepartmentId).Select(a => new { a.CategoryId, a.CategoryName, a.CategoryDescription, a.image, a.IsActive }).ToList()
                }).ToListAsync();
                return Ok(new { Status = true, department });
            }
            catch (Exception ex)
            {
                return Ok(new { Status = false, Message = ex.Message });
            }
        }
        #endregion
        #region GetCategoriesByBranchId
        [HttpGet]
        [Route("GetCategoriesByBranchId")]
        public async Task<ActionResult<List<Category>>> GetCategoriesByBranchId(int BranchId)
        {
            var categories = await _context.Categories.Include(e => e.Department).Where(e => e.Department.BranchId == BranchId && e.IsActive == true).ToListAsync();
            return Ok(categories);
        }
        #endregion
        #region GetCategoryDetailsById
        [HttpGet]
        [Route("GetCategoryDetailsById")]
        public async Task<ActionResult> GetCategoryDetailsById(int CategoryId)
        {
            try
            {
                var category = await _context.Categories.Where(e => e.CategoryId == CategoryId).Select(e => new
                {
                    e.CategoryId,
                    e.CategoryName,
                    e.CategoryDescription,
                    e.image,
                    e.IsActive,
                    e.DepartmentId,
                    DepartmentName = _context.Departments.Where(a => a.IsActive && a.DepartmentId == e.DepartmentId).FirstOrDefault().DepartmentName,

                }).FirstOrDefaultAsync();
                return Ok(new { Status = true, category });
            }
            catch (Exception ex) {
                return Ok(new { Status = false, Message = ex.Message });


            }
        }
        #endregion
        #region GetTrainersByBranchId
        //[HttpGet]
        //[Route("GetTrainersByBranchId")]
        //public async Task<ActionResult<List<Trainer>>> GetTrainersByBranchId(int BranchId)
        //{
        //    //var trainers = await _context.Trainers.Include(e=>e.Department).Where(e => e.Department.BranchId == BranchId && e.IsActive).ToListAsync();
        //    return Ok(trainers);
        //}
        #endregion
        #region GetTrainerDetailsById
        [HttpGet]
        [Route("GetTrainerDetailsById")]
        public async Task<ActionResult> GetTrainerDetailsById(int TrainerId)
        {
            try
            {
                var trainer = await _context.Trainers.Where(e => e.TrainerId == TrainerId && e.IsActive).Select(e => new
                {
                    e.TrainerId,
                    e.TrainerName,
                    e.TrainerEmail,
                    e.TrainerPhone,
                    e.Image,
                    e.IsActive,
                    e.HiringDate,
                    e.CurrentDepartment,
                    e.CurrentBranch,
                    BranchName = _context.Branches.Where(a => a.IsActive && a.BranchId == e.CurrentBranch).FirstOrDefault().BranchName,
                    BranchAddress = _context.Branches.Where(a => a.IsActive && a.BranchId == e.CurrentBranch).FirstOrDefault().BranchAddress,
                    BranchPhone = _context.Branches.Where(a => a.IsActive && a.BranchId == e.CurrentBranch).FirstOrDefault().Phone,
                    BranchImage = _context.Branches.Where(a => a.IsActive && a.BranchId == e.CurrentBranch).FirstOrDefault().Image,
                    DepartmentName = _context.Departments.Where(a => a.IsActive && a.DepartmentId == e.CurrentDepartment).FirstOrDefault().DepartmentName,
                    DepartmentDescription = _context.Departments.Where(a => a.IsActive && a.DepartmentId == e.CurrentDepartment).FirstOrDefault().DepartmentDescription,
                    DepartmentImage = _context.Departments.Where(a => a.IsActive && a.DepartmentId == e.CurrentDepartment).FirstOrDefault().Image,
                    Categories = _context.CategoryTrainers.Where(a => a.IsActive && a.TrainerId == e.TrainerId).Select(a => new { a.CategoryId, CategoryName = _context.Categories.Where(b => b.IsActive && b.CategoryId == a.CategoryId).FirstOrDefault().CategoryName, CategoryDescription = _context.Categories.Where(b => b.IsActive && b.CategoryId == a.CategoryId).FirstOrDefault().CategoryDescription, image = _context.Categories.Where(b => b.IsActive && b.CategoryId == a.CategoryId).FirstOrDefault().image, IsActive = _context.Categories.Where(b => b.IsActive && b.CategoryId == a.CategoryId).FirstOrDefault().IsActive }).ToList()
                }).FirstOrDefaultAsync();
                return Ok(new { Status = true, trainer });
            }
            catch (Exception ex)
            {
                return Ok(new { Status = false, Message = ex.Message });
            }
        }

        #endregion
        #region AddAttendance
        [HttpPost]
        [Route("AddAttendance")]
        public async Task<ActionResult> AddAttendance([FromBody] AttendanceDTO attendanceDTO)
        {
            try
            {
                var attendance = new Absence
                {
                    TrainerId = attendanceDTO.TrainerId,
                    AbsenceDate = attendanceDTO.AbsenceDate,
                    IsAbsent = attendanceDTO.IsAbsent,
                    SubscriptionId = attendanceDTO.SubscriptionId,
                    Type = attendanceDTO.Type,
                    IsDeleted = false,                    
                };
                _context.Abscenses.Add(attendance);
                await _context.SaveChangesAsync();
                return Ok(new { Status = true, Message = "Attendance added successfully!" });
            }
            catch (Exception ex)
            {
                return Ok(new { Status = false, Message = ex.Message });
            }
        }
        #endregion
        #region AddEvaluation
        [HttpPost]
        [Route("AddEvaluation")]
        public async Task<ActionResult> AddEvaluation([FromBody] EvaluationDTO evaluationDTO)
        {
            try
            {
                var evaluation = new Exam
                {
                    TrainerId = evaluationDTO.TrainerId,
                    ExamDate = evaluationDTO.ExamDate,
                    Review = evaluationDTO.Review,
                    Score = evaluationDTO.Score,
                    
                    SubscriptionId = evaluationDTO.SubscriptionId,
                    IsDeleted = false,
                };
                _context.Exams.Add(evaluation);
                await _context.SaveChangesAsync();
                return Ok(new { Status = true, Message = "Evaluation added successfully!" });
            }
            catch (Exception ex)
            {
                return Ok(new { Status = false, Message = ex.Message });
            }
        }
        #endregion
        #region GetAttendancesListByCategoryId
        [HttpGet]
        [Route("GetAttendancesListByCategoryId")]
        public async Task<ActionResult> GetAttendancesListByCategoryId(int CategoryId)
        {
            try
            {
                var attendances = await _context.Abscenses.Include(e=>e.Trainer).Where(e => e.Trainer.TrainerCategories.Any(e => e.CategoryId == CategoryId) && e.Trainer.IsActive).Select(e => new
                {
                    e.AbsenceId,
                    e.AbsenceDate,
                    e.IsAbsent,
                    e.Type,
                    e.SubscriptionId,
                    e.IsDeleted,
                    TrainerName = _context.Trainers.Where(a => a.IsActive && a.TrainerId == e.TrainerId).FirstOrDefault().TrainerName,
                    TrainerPhone = _context.Trainers.Where(a => a.IsActive && a.TrainerId == e.TrainerId).FirstOrDefault().TrainerPhone,
                    TrainerEmail = _context.Trainers.Where(a => a.IsActive && a.TrainerId == e.TrainerId).FirstOrDefault().TrainerEmail,
                    TrainerImage = _context.Trainers.Where(a => a.IsActive && a.TrainerId == e.TrainerId).FirstOrDefault().Image,
                    CategoryName = _context.Categories.Where(a => a.IsActive && a.CategoryId == e.Trainer.TrainerCategories.FirstOrDefault().CategoryId).FirstOrDefault().CategoryName,
                    CategoryDescription = _context.Categories.Where(a => a.IsActive && a.CategoryId == e.Trainer.TrainerCategories.FirstOrDefault().CategoryId).FirstOrDefault().CategoryDescription,
                    CategoryImage = _context.Categories.Where(a => a.IsActive && a.CategoryId == e.Trainer.TrainerCategories.FirstOrDefault().CategoryId).FirstOrDefault().image,
                }).ToListAsync();
                return Ok(new { Status = true, attendances });
            }
            catch (Exception ex)
            {
                return Ok(new { Status = false, Message = ex.Message });
            }
        }
        #endregion

        #region GetParentDetailsById
        [HttpGet]
        [Route("GetParentDetailsById")]
        public async Task<ActionResult> GetParentDetailsById(int ParentId)
        {
            try
            {
                var parent = await _context.Parents.Where(e => e.ParentId == ParentId && e.IsActive).Select(e => new
                {
                    e.ParentId,
                    e.ParentName,
                    e.ParentEmail,
                    e.ParentPhone,
                    e.Image,
                    e.IsActive,
                    e.UserName,
                    e.ParentAddress,
                    
                }).FirstOrDefaultAsync();
                return Ok(new { Status = true, parent });
            }
            catch (Exception ex)
            {
                return Ok(new { Status = false, Message = ex.Message });
            }
        }
        #endregion

        #region GetChildrenByParentId
        [HttpGet]
        [Route("GetChildrenByParentId")]
        public async Task<ActionResult> GetChildrenByParentId(int ParentId)
        {
            try
            {
                var children = await _context.Trainees.Where(e => e.ParentId == ParentId && e.IsActive).Select(e => new
                {
                    e.TraineeId,
                    e.TraineeName,
                    e.TraineePhone,
                    e.TraineeEmail,
                    e.UserName,
                    e.Image,
                    e.IsActive,
                    e.ParentId,
                    ParentName = _context.Parents.Where(a => a.IsActive && a.ParentId == e.ParentId).FirstOrDefault().ParentName,
                    ParentEmail = _context.Parents.Where(a => a.IsActive && a.ParentId == e.ParentId).FirstOrDefault().ParentEmail,
                    ParentPhone = _context.Parents.Where(a => a.IsActive && a.ParentId == e.ParentId).FirstOrDefault().ParentPhone,
                    ParentImage = _context.Parents.Where(a => a.IsActive && a.ParentId == e.ParentId).FirstOrDefault().Image,
                }).ToListAsync();
                return Ok(new { Status = true, children });
            }
            catch (Exception ex)
            {
                return Ok(new { Status = false, Message = ex.Message });
            }
        }
        #endregion

        #region GetChildDetailsById
        [HttpGet]
        [Route("GetChildDetailsById")]
        public async Task<ActionResult> GetChildDetailsById(int TraineeId)
        {
            try
            {
                var child = await _context.Trainees.Where(e => e.TraineeId == TraineeId && e.IsActive).Select(e => new
                {
                    e.TraineeId,
                    e.TraineeName,
                    e.TraineePhone,
                    e.TraineeEmail,
                    e.UserName,
                    e.Image,
                    e.IsActive,
                    e.ParentId,
                    ParentName = _context.Parents.Where(a => a.IsActive && a.ParentId == e.ParentId).FirstOrDefault().ParentName,
                    ParentEmail = _context.Parents.Where(a => a.IsActive && a.ParentId == e.ParentId).FirstOrDefault().ParentEmail,
                    ParentPhone = _context.Parents.Where(a => a.IsActive && a.ParentId == e.ParentId).FirstOrDefault().ParentPhone,
                    ParentImage = _context.Parents.Where(a => a.IsActive && a.ParentId == e.ParentId).FirstOrDefault().Image,
                }).FirstOrDefaultAsync();
                return Ok(new { Status = true, child });
            }
            catch (Exception ex)
            {
                return Ok(new { Status = false, Message = ex.Message });
            }
        }
        #endregion
        #region GetChildSubscriptionsByChildId
        [HttpGet]
        [Route("GetChildSubscriptionsByChildId")]
        public async Task<ActionResult> GetChildSubscriptionsByChildId(int TraineeId)
        {
            try
            {
                var today = DateOnly.FromDateTime(DateTime.Now);

                var subscriptions = await _context.Subscriptions.Where(e => e.TraineeId == TraineeId && e.IsActive && (e.EndDate == null || e.EndDate >today)).Select(e => new
                {
                    e.SubscriptionId,
                    e.EndDate,
                    e.IsActive,
                    e.TraineeId,
                    TraineeName = _context.Trainees.Where(a => a.IsActive && a.TraineeId == e.TraineeId).FirstOrDefault().TraineeName,
                    TraineePhone = _context.Trainees.Where(a => a.IsActive && a.TraineeId == e.TraineeId).FirstOrDefault().TraineePhone,
                    TraineeEmail = _context.Trainees.Where(a => a.IsActive && a.TraineeId == e.TraineeId).FirstOrDefault().TraineeEmail,
                    TraineeImage = _context.Trainees.Where(a => a.IsActive && a.TraineeId == e.TraineeId).FirstOrDefault().Image,
                    CategoryName = _context.Categories.Where(a => a.IsActive && a.CategoryId == e.CategoryId).FirstOrDefault().CategoryName,
                    CategoryDescription = _context.Categories.Where(a => a.IsActive && a.CategoryId == e.CategoryId).FirstOrDefault().CategoryDescription,
                    CategoryImage = _context.Categories.Where(a => a.IsActive && a.CategoryId == e.CategoryId).FirstOrDefault().image,
                                  }).ToListAsync();
                return Ok(new { Status = true, subscriptions });
            }
            catch (Exception ex)
            {
                return Ok(new { Status = false, Message = ex.Message });
            }
        }
        #endregion

        #region GetTrainersByDepartmentId
        //[HttpGet]
        //[Route("GetTrainersByDepartmentId")]
        //public async Task<ActionResult<List<Trainer>>> GetTrainersByDepartmentId(int DepartmentId)
        //{
        //    //var trainers = await _context.Trainers.Where(e => e.DepartmentId == DepartmentId && e.IsActive).ToListAsync();
        //    return Ok(trainers);
        //}
        #endregion
        #region GetTrainerCategoriesByTrainerId
        [HttpGet]
        [Route("GetTrainerCategoriesByTrainerId")]
        public async Task<ActionResult> GetTrainersByvCategoryId(int TrainerId)
        {
            try
            {
                var trainerCategories = await _context.CategoryTrainers.Where(e => e.TrainerId == TrainerId && e.IsActive).Select(e => new
                {
                    e.CategoryId,
                    CategoryName = _context.Categories.Where(a => a.IsActive && a.CategoryId == e.CategoryId).FirstOrDefault().CategoryName,
                    CategoryDescription = _context.Categories.Where(a => a.IsActive && a.CategoryId == e.CategoryId).FirstOrDefault().CategoryDescription,
                    image = _context.Categories.Where(a => a.IsActive && a.CategoryId == e.CategoryId).FirstOrDefault().image,
                    IsActive = _context.Categories.Where(a => a.IsActive && a.CategoryId == e.CategoryId).FirstOrDefault().IsActive,
                    Trainer = _context.Trainers.Where(a => a.IsActive && a.TrainerId == e.TrainerId).Select(a => new { a.TrainerId, a.TrainerName, a.TrainerEmail, a.TrainerPhone, a.Image, a.IsActive }).FirstOrDefault()
                }).ToListAsync();
                return Ok(new { Status = true, trainerCategories });

            }
            catch (Exception ex)
            {
                return Ok(new { Status = false, Message = ex.Message });
            }
        }
        #endregion
        #region GetTrainersByBranchIdAndCategoryId

        //[HttpGet]

        //[Route("GetTrainersByBranchIdAndCategoryId")]

        //public async Task<ActionResult<List<Trainer>>> GetTrainersByBranchIdAndCategoryId(int BranchId, int CategoryId)
        //{
        //    var trainers = await _context.Trainers.Include(e=>e.Department).Include(e => e.CategoryTrainers).Where(e => e.CategoryTrainers.Any(e => e.CategoryId == CategoryId) && e.Department.BranchId== BranchId && e.IsActive).ToListAsync();
        //    return Ok(trainers);
        //}
        #endregion
        #region GetDepartmentsByBranchId
        [HttpGet]
        [Route("GetDepartmentsByBranchId")]
        public async Task<ActionResult<List<Category>>> GetDepartmentsByBranchId(int BranchId)
        {
            try
            {
                var departments = await _context.Departments.Include(e => e.Branch).Where(e => e.BranchId == BranchId && e.IsActive == true && e.Branch.IsActive).ToListAsync();
                return Ok(new { Status = true, departments });
            }
            catch (Exception ex)
            {
                return Ok(new { Status = false, Message = ex.Message });
            }

        }
        #endregion
        #region GetBranchDetailsById
        [HttpGet]
        [Route("GetBranchDetailsById")]
        public async Task<ActionResult<Branch>> GetBranchDetailsById(int BranchId)
        {
            var branch = await _context.Branches.Where(e => e.BranchId == BranchId && e.IsActive).Include(e => e.Departments).Select(e => new
            {
                e.BranchId,
                e.BranchName,
                e.BranchAddress,
                e.Phone,
                e.Image,
                e.Departments,
            }).FirstOrDefaultAsync();
            return Ok(branch);
        }
        #endregion
        #region GetAllBranches
        [HttpGet]
        [Route("GetAllBranches")]
        public async Task<ActionResult<List<Branch>>> GetAllBranches()
        {
            try
            {
                var branches = await _context.Branches.Where(e => e.IsActive).ToListAsync();
                return Ok(new { Status = true, branches });
            }
            catch (Exception ex)
            {
                return Ok(new { Status = false, Message = ex.Message });

                #endregion







            }
        }
    }
}