using Academy.Data;
using Academy.Models;
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


        public MobileController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, AcademyContext context) { 
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
        }
        #region Login
        [HttpGet]
        [Route("Login")]
        public async Task<ActionResult<ApplicationUser>> Login([FromQuery] string Email, [FromQuery] string Password)
        {

            var user = await _userManager.FindByNameAsync(Email);

            if (user != null)
            {
                var result = await _signInManager.CheckPasswordSignInAsync(user, Password, true);
                if (result.Succeeded)
                {
                    if(user.EntityId != null && user.EntityName == "Trainer" )
                    {
                        var trainer = await _context.Trainers.Where(e=>e.TrainerId==user.EntityId).FirstOrDefaultAsync();
                        return Ok(new { Status = "Success", Message = "User Login successfully!", trainer });
                    }
                    if(user.EntityId != null && user.EntityName == "Parent")
                    {
                        var parent = await _context.Parents.Where(e => e.ParentId == user.EntityId).FirstOrDefaultAsync();
                        return Ok(new { Status = "Success", Message = "User Login successfully!", parent });
                    }
                    
                }
            }
            var invalidResponse = new { status = false };
            return Ok(invalidResponse);
        }

        #endregion

        #region GetDepartmetsById 
        [HttpGet]
        [Route("GetDepartmetsByBranchId")]
        public async Task<ActionResult<List<Department>>> GetDepartmetsByBranchId(int BranchId)
        {
            var departments = await _context.Departments.Where(e=>e.BranchId== BranchId && e.IsActive).ToListAsync();
            return Ok(departments);
        }
        #endregion

        #region GetCategoriesByDepartmentId
        [HttpGet]
        [Route("GetCategoriesByDepartmentId")]
        public async Task<ActionResult<List<Category>>> GetCategoriesByDepartmentId(int DepartmentId)
        {
            var categories = await _context.Categories.Where(e => e.DepartmentId == DepartmentId && e.IsActive==true).ToListAsync();
            return Ok(categories);
        }
        #endregion

        #region GetDepartmetsByDepartmentId
        [HttpGet]
        [Route("GetDepartmetsByDepartmentId")]
        public async Task<ActionResult<List<Department>>> GetDepartmetsByDepartmentId(int DepartmentId)
        {
            var department = await _context.Departments.Where(e => e.DepartmentId == DepartmentId && e.IsActive).ToListAsync();
            return Ok(department);
        }
        #endregion

        #region GetCategoriesByBranchId
        [HttpGet]
        [Route("GetCategoriesByBranchId")]
        public async Task<ActionResult<List<Category>>> GetCategoriesByBranchId(int BranchId)
        {
            var categories = await _context.Categories.Include(e=>e.Department).Where(e => e.Department.BranchId == BranchId && e.IsActive==true).ToListAsync();
            return Ok(categories);
        }
        #endregion
        #region GetTrainersByBranchId
        [HttpGet]
        [Route("GetTrainersByBranchId")]
        public async Task<ActionResult<List<Trainer>>> GetTrainersByBranchId(int BranchId)
        {
            var trainers = await _context.Trainers.Where(e => e.BranchId == BranchId && e.IsActive).ToListAsync();
            return Ok(trainers);
        }
        #endregion

        #region GetTrainersByDepartmentId
        [HttpGet]
        [Route("GetTrainersByDepartmentId")]
        public async Task<ActionResult<List<Trainer>>> GetTrainersByDepartmentId(int DepartmentId)
        {
            var trainers = await _context.Trainers.Where(e => e.DepartmentId == DepartmentId && e.IsActive).ToListAsync();
            return Ok(trainers);
        }
        #endregion
        #region GetTrainersByCategoryId
        [HttpGet]
        [Route("GetTrainersByCategoryId")]
        public async Task<ActionResult<List<Trainer>> > GetTrainersByCategoryId(int CategoryId)
        {
            var trainers = await _context.Trainers.Include(e=>e.CategoryTrainers).Where(e => e.CategoryTrainers.Any(e=>e.CategoryId== CategoryId) && e.IsActive).ToListAsync();
            return Ok(trainers);
        }
        #endregion
        #region GetTrainersByBranchIdAndCategoryId

        [HttpGet]

        [Route("GetTrainersByBranchIdAndCategoryId")]
        
        public async Task<ActionResult<List<Trainer>>> GetTrainersByBranchIdAndCategoryId(int BranchId, int CategoryId)
        {
            var trainers = await _context.Trainers.Include(e => e.CategoryTrainers).Where(e => e.CategoryTrainers.Any(e => e.CategoryId == CategoryId) && e.BranchId== BranchId && e.IsActive).ToListAsync();
            return Ok(trainers);
        }
        #endregion



    }
}
