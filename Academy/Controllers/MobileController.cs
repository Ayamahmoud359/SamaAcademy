using Academy.Data;
using Academy.DTO;
using Academy.Models;
using Academy.ViewModels;
//using DevExpress.XtraPrinting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using System;
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
        private readonly IWebHostEnvironment _hostEnvironment;


        public MobileController(UserManager<ApplicationUser> userManager, IWebHostEnvironment hostEnvironment, SignInManager<ApplicationUser> signInManager, AcademyContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
            _hostEnvironment = hostEnvironment;
        }
        #region Login
        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login([FromQuery] string login, [FromQuery] string password)
        {

            var user = await _userManager.FindByEmailAsync(login)
                  ?? await _userManager.FindByNameAsync(login);

            //ApplicationUser user = null;

            // Determine if the input is an email or username
            //if (login.Contains("@"))
            //{
            //    // Assume it's an email
            //    user = await _userManager.FindByEmailAsync(login);
            //}
            //else
            //{
            //    // Assume it's a username
            //    user = await _userManager.FindByNameAsync(login);
            //}

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
                        return Ok(new { status = true,userId = user.Id, type_id = 1, message = "User logged in successfully!", trainer });
                    }
                    if (user.EntityId != null && user.EntityName == "Parent")
                    {
                        var parent = await _context.Parents
                            .FirstOrDefaultAsync(e => e.ParentId == user.EntityId);
                        return Ok(new { status = true, userId = user.Id, type_id = 2, message = "User logged in successfully!", parent });
                    }
                }
            }

            // Return an invalid response
            return Ok(new { status = false, message = "Invalid login credentials." });
        }

        #endregion

        #region ChangePassword
        [HttpPost]
        [Route("ChangePassword")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordDTO changePasswordDTO)
        {
            var user = await _userManager.FindByIdAsync(changePasswordDTO.UserId);
            if (user == null)
            {
                return Ok(new { status = false, message = "User not found!" });
            }
            var result = await _userManager.ChangePasswordAsync(user, changePasswordDTO.OldPassword, changePasswordDTO.NewPassword);
            if (result.Succeeded)
            {
                return Ok(new { status = true, message = "Password changed successfully!" });
            }
            return Ok(new { status = false, message = "Password change failed!" });
        }
        #endregion
        #region GetDepartmetsById 
        [HttpGet]
        [Route("GetDepartmetsByBranchId")]
        public async Task<IActionResult> GetDepartmetsByBranchId(int BranchId)
        {
            var departments = await _context.Departments.Where(e => e.BranchId == BranchId && e.IsActive && !e.IsDeleted).ToListAsync();
            return Ok(new { status = true, data = departments });

        }
        #endregion
        #region UploadMedia
        private async Task<string> UploadImage(string folderPath, IFormFile file)
        {

            folderPath += Guid.NewGuid().ToString() + "_" + file.FileName;

            string serverFolder = Path.Combine(_hostEnvironment.WebRootPath, folderPath);

            var directory = Path.GetDirectoryName(serverFolder);
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            await file.CopyToAsync(new FileStream(serverFolder, FileMode.Create));

            return folderPath;
        }

        #endregion
        #region UpdateUserProfile
        [HttpPost]
        [Route("UpdateUserProfile")]
        public async Task<IActionResult> UpdateUserProfile([FromForm] UpdateUserProfileDTO updateUserProfileDTO, IFormFile? Pic)
        {
            // Find the user in the Identity system
            var user = await _userManager.FindByIdAsync(updateUserProfileDTO.UserId);
            if (user == null)
            {
                return Ok(new { status = false, message = "User not found!" });
            }

            // Update the profile based on the EntityName
            if (user.EntityName == "Trainer")
            {
                var trainer = await _context.Trainers.FirstOrDefaultAsync(e => e.TrainerId == user.EntityId);
                if (trainer == null)
                {
                    return Ok(new { status = false, message = "Trainer not found!" });
                }

                // Update Trainer fields
                if (updateUserProfileDTO.Name != null) {
                    trainer.TrainerName = updateUserProfileDTO.Name;
                }
                if (Pic != null)
                {
                    trainer.Image = await UploadImage("uploads/Users/", Pic);

                }

                if (updateUserProfileDTO.Email != null)
                {
                    trainer.TrainerEmail = updateUserProfileDTO.Email;
                    user.Email = updateUserProfileDTO.Email;
                }
                if (updateUserProfileDTO.Phone != null)
                {
                    trainer.TrainerPhone = updateUserProfileDTO.Phone;
                    user.PhoneNumber = updateUserProfileDTO.Phone;
                }
                if (updateUserProfileDTO.Address != null)
                {
                    trainer.TrainerAddress = updateUserProfileDTO.Address;
                }
                if(updateUserProfileDTO.Image != null)
                {
                    //trainer.Image = updateUserProfileDTO.Image;
                }
               
        
                // Save changes
                _context.Trainers.Update(trainer);
                var userUpdateResult = await _userManager.UpdateAsync(user);
                if (!userUpdateResult.Succeeded)
                {
                    return Ok(new { status = false, message = "Failed to update user profile in Identity!" });
                }

                await _context.SaveChangesAsync();
                return Ok(new { status = true, message = "Trainer profile updated successfully!" });
            }
            if (user.EntityName == "Parent")
            {
                var parent = await _context.Parents.FirstOrDefaultAsync(e => e.ParentId == user.EntityId);
                if (parent == null)
                {
                    return Ok(new { status = false, message = "Parent not found!" });
                }

                // Update Parent fields
                if(updateUserProfileDTO.Name != null)
                {
                    parent.ParentName = updateUserProfileDTO.Name;
                }
                if (Pic != null)
                {
                    parent.Image = await UploadImage("uploads/Users/", Pic);

                }
                if (updateUserProfileDTO.Email != null)
                {
                    parent.ParentEmail = updateUserProfileDTO.Email;
                    user.Email = updateUserProfileDTO.Email;
                }
                if (updateUserProfileDTO.Phone != null)
                {
                    parent.ParentPhone = updateUserProfileDTO.Phone;
                    user.PhoneNumber = updateUserProfileDTO.Phone;
                }
                if (updateUserProfileDTO.Address != null)
                {
                    parent.ParentAddress = updateUserProfileDTO.Address;
                }
                if (updateUserProfileDTO.Image != null)
                {
                    //parent.Image = updateUserProfileDTO.Image;
                }
               
               

                // Save changes
                _context.Parents.Update(parent);
                var userUpdateResult = await _userManager.UpdateAsync(user);
                if (!userUpdateResult.Succeeded)
                {
                    return Ok(new { status = false, message = "Failed to update user profile in Identity!" });
                }

                await _context.SaveChangesAsync();
                return Ok(new { status = true, message = "Parent profile updated successfully!" });
            }

            // Handle other entity types or unsupported types
            return Ok(new { status = false, message = "User type not found!" });
        }

        #endregion

        #region GetCategoriesByDepartmentId
        [HttpGet]
        [Route("GetCategoriesByDepartmentId")]
        public async Task<IActionResult> GetCategoriesByDepartmentId(int DepartmentId)
        {
            try
            {
                var categories = await _context.Categories.Include(e => e.Department).Where(e => e.DepartmentId == DepartmentId && e.IsActive == true && !e.IsDeleted && e.Department.IsActive && e.Department.IsDeleted == false).Select(e => new
                {
                    e.CategoryId,
                    e.DepartmentId,
                    e.CategoryName,
                    e.CategoryDescription,
                    e.image,
                    e.IsActive
                }).ToListAsync();
                return Ok(new { status = true, data = categories });
            }
            catch (Exception ex)
            {
                return Ok(new { status = false, message = ex.Message });
            }

        }
        #endregion
        
        #region GetDepartmentDetailsById
        [HttpGet]
        [Route("GetDepartmentDetailsById")]
        public async Task<IActionResult> GetDepartmentDetailsById(int DepartmentId)
        {
            try
            {
                var department = await _context.Departments.Where(e => e.DepartmentId == DepartmentId && e.IsActive && !e.IsDeleted).Select(e => new
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
                }).FirstOrDefaultAsync();
                return Ok(new { status = true, data = department });
            }
            catch (Exception ex)
            {
                return Ok(new { status = false, message = ex.Message });
            }
        }
        #endregion
       
        #region GetCategoriesByBranchId
        [HttpGet]
        [Route("GetCategoriesByBranchId")]
        public async Task<IActionResult> GetCategoriesByBranchId(int BranchId)
        {
            var categories = await _context.Categories.Include(e => e.Department).Where(e => e.Department.BranchId == BranchId && e.IsActive == true).ToListAsync();
            return Ok(categories);
        }
        #endregion
        
        #region GetCategoryDetailsById
        [HttpGet]
        [Route("GetCategoryDetailsById")]
        public async Task<IActionResult> GetCategoryDetailsById(int CategoryId)
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
                return Ok(new { status = true,data = category });
            }
            catch (Exception ex) {
                return Ok(new { status = false, message = ex.Message });


            }
        }
        #endregion

        #region GetTrainersByBranchId
        [HttpGet]
        [Route("GetTrainersByBranchId")]
        public async Task<IActionResult> GetTrainersByBranchId(int BranchId)
        {
            var branch = await _context.Branches.Where(e => e.BranchId == BranchId && e.IsActive).FirstOrDefaultAsync();
            if (branch == null)
            {
                return Ok(new { status = false, message = "Branch not found!" });
            }
            var trainers = await _context.Trainers.Where(e => e.CurrentBranch == BranchId && e.IsActive).ToListAsync();
            return Ok(new {status = true , data = trainers});
        }
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
                    e.TrainerAddress,
                    e.Image,
                    e.IsActive,
                    e.HiringDate,
                    e.CurrentDepartment,
                    e.CurrentBranch,
                    Branch = _context.Branches.Where(a => a.IsActive && a.BranchId == e.CurrentBranch).Select(a => new { a.BranchId, a.BranchName, a.BranchAddress, a.Phone, a.Image, a.IsActive }).FirstOrDefault(),
                    Department = _context.Departments.Where(a => a.IsActive && a.DepartmentId == e.CurrentDepartment).Select(a => new { a.DepartmentId, a.DepartmentName, a.DepartmentDescription, a.Image, a.IsActive }).FirstOrDefault(),
                    Categories = _context.CategoryTrainers.Where(a => a.IsActive && a.TrainerId == e.TrainerId).Select(a => new { a.CategoryId, CategoryName = _context.Categories.Where(b => b.IsActive && b.CategoryId == a.CategoryId).FirstOrDefault().CategoryName, CategoryDescription = _context.Categories.Where(b => b.IsActive && b.CategoryId == a.CategoryId).FirstOrDefault().CategoryDescription, image = _context.Categories.Where(b => b.IsActive && b.CategoryId == a.CategoryId).FirstOrDefault().image, IsActive = _context.Categories.Where(b => b.IsActive && b.CategoryId == a.CategoryId).FirstOrDefault().IsActive }).ToList()
                }).FirstOrDefaultAsync();
                return Ok(new { status = true, data = trainer });
            }
            catch (Exception ex)
            {
                return Ok(new { status = false, message = ex.Message });
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
                return Ok(new { status = true, message = "Attendance added successfully!" });
            }
            catch (Exception ex)
            {
                return Ok(new { status = false, message = ex.Message });
            }
        }

        [HttpPost]
        [Route("AddAttendanceToCategorySubscriptions")]
        public async Task<ActionResult> AddAttendanceToCategorySubscriptions([FromBody] List<AddAttendanceDTO> attendanceList)
        {
            try
            {
                if (attendanceList == null || !attendanceList.Any()) { 
                    return Ok(new { status = false, message = "Attendance list is empty!" });
                }
                foreach (var record in attendanceList)
                {
                    var attendance = new Absence
                    {
                        TrainerId = record.TrainerId,
                        AbsenceDate = record.Date,
                        IsAbsent = record.IsAbsent,
                        SubscriptionId = record.SubscriptionId,
                        Type = record.Type,
                        IsDeleted = false,
                    };
                    _context.Abscenses.Add(attendance);
                }
                    
                
                await _context.SaveChangesAsync();
                return Ok(new { status = true, message = "Attendance added successfully!" });
            }
            catch (Exception ex)
            {
                return Ok(new { status = false, message = ex.Message });
            }
        }


        [HttpGet]
        [Route("GetAttendanceToCategorySubscriptions")]
        public async Task<ActionResult> GetAttendanceToCategorySubscriptions(int categoryId)
        {
            try
            {
                // Check if the category exists and is active
                var category = await _context.Categories
                    .Where(e => e.CategoryId == categoryId && e.IsActive)
                    .FirstOrDefaultAsync();

                if (category == null)
                {
                    return Ok(new { status = false, message = "Category not found!" });
                }

                // Fetch attendance records for the given category
                var attendances = await _context.Abscenses
                    .Include(e => e.Subscription) // Include related subscription
                    .Where(e => e.Subscription != null && e.Subscription.CategoryId == categoryId)
                    .GroupBy(a => a.AbsenceDate) // Group by date
                    .Select(group => new
                    {
                        Date = group.Key,
                        Records = group.Select(a => new
                        {
                            a.AbsenceId,
                            a.AbsenceDate,
                            a.IsAbsent,
                            a.Type,
                            a.SubscriptionId,
                            a.IsDeleted,
                            a.TrainerId,
                            
                            //Subscription = new
                            //{
                            //    a.Subscription!.SubscriptionId,
                            //    a.Subscription.StartDate,
                            //    a.Subscription.EndDate,
                            //    a.Subscription.IsActive,
                            //    a.Subscription.CategoryId,
                            //    a.Subscription.TraineeId
                            //},
                            
                            //Child = _context.Trainees
                            //    .Where(t => t.IsActive && t.TraineeId == a.Subscription!.TraineeId)
                            //    .Select(t => new
                            //    {
                            //        t.TraineeId,
                            //        t.TraineeName,
                            //        t.TraineePhone,
                            //        t.TraineeEmail,
                            //        t.Image,
                            //        t.IsActive
                            //    })
                            //    .FirstOrDefault(),
                            
                            //Trainer = _context.Trainers
                            //    .Where(t => t.IsActive && t.TrainerId == a.TrainerId)
                            //    .Select(t => new
                            //    {
                            //        t.TrainerId,
                            //        t.TrainerName,
                            //        t.TrainerEmail,
                            //        t.TrainerPhone,
                            //        t.Image,
                            //        t.IsActive
                            //    })
                            //    .FirstOrDefault()
                       
                        }).ToList()
                    })
                    .OrderBy(summary => summary.Date)
                    .ToListAsync();

                return Ok(new { status = true, attendances });
            }
            catch (Exception ex)
            {
                // Return error details
                return Ok(new { status = false, message = ex.Message });
            }
        }


        // Get attendance records for a specific subscription
        [HttpGet("GetAttendanceBySubscriptionId")]
        public async Task<IActionResult> GetAttendanceBySubscription(int subscriptionId)
        {
            try
            {
                // Validate if the subscription exists
                var subscription = await _context.Subscriptions
                    .Where(s => s.SubscriptionId == subscriptionId && s.IsActive && s.IsDeleted == false)
                    .FirstOrDefaultAsync();

                if (subscription == null)
                {
                    return Ok(new { status = false, message = "Subscription not found!" });
                }

                // Fetch attendance records for the given subscription
                var attendanceRecords = await _context.Abscenses
                    .Where(a => a.SubscriptionId == subscriptionId)
                    .OrderBy(a => a.AbsenceDate) // Order by date
                    .Select(a => new
                    {
                        a.AbsenceId,
                        a.AbsenceDate,
                        a.IsAbsent,
                        a.Type,
                        a.TrainerId,
                        Trainer = _context.Trainers
                            .Where(t => t.IsActive && t.TrainerId == a.TrainerId)
                            .Select(t => new
                            {
                                t.TrainerId,
                                t.TrainerName,
                                t.TrainerEmail,
                                t.TrainerPhone,
                                t.Image
                            })
                            .FirstOrDefault()
                    })
                    .ToListAsync();

                return Ok(new { status = true, attendanceRecords });
            }
            catch (Exception ex)
            {
                return Ok(new { status = false, message = ex.Message });
            }
        }


        #endregion

        #region AddEvaluation
        [HttpPost]
        [Route("AddEvaluation")]
        public async Task<ActionResult> AddEvaluation([FromBody] AddEvaluationDTO evaluationDTO)
        {
            try
            {
                var evaluation = new Exam
                {
                    TrainerId = evaluationDTO.TrainerId,
                    ExamDate = evaluationDTO.EvaluationDate,
                    Review = evaluationDTO.Review,
                    Score = evaluationDTO.Score,
                    
                    SubscriptionId = evaluationDTO.SubscriptionId,
                    IsDeleted = false,
                };
                _context.Exams.Add(evaluation);
                await _context.SaveChangesAsync();
                return Ok(new { status = true, message = "Evaluation added successfully!" });
            }
            catch (Exception ex)
            {
                return Ok(new { status = false, message = ex.Message });
            }
        }

        [HttpGet]
        [Route("GetEvaluationListBySubscriptionId")]
        public async Task<ActionResult> GetEvaluationListBySubscriptionId(int SubscriptionId)
        {
            try
            {
               var evaluationList = await _context.Exams.Where(e => e.SubscriptionId == SubscriptionId && e.IsDeleted == false).Select(e => new
               {
                    e.ExamId,
                    e.ExamDate,
                    e.Score,
                    e.Review,
                    e.SubscriptionId,
                    e.IsDeleted,
                    Trainer = _context.Trainers.Where(a => a.IsActive && a.TrainerId == e.TrainerId).Select(a => new { a.TrainerId, a.TrainerName, a.TrainerEmail, a.TrainerPhone, a.Image, a.IsActive }).FirstOrDefault(),
                    Category = _context.Categories.Where(a => a.IsActive && a.CategoryId == _context.Subscriptions.Where(b => b.IsActive && b.SubscriptionId == e.SubscriptionId).FirstOrDefault().CategoryId).Select(a => new { a.CategoryId, a.CategoryName, a.CategoryDescription, a.image, a.IsActive }).FirstOrDefault(),
                }).ToListAsync();
                return Ok(new { status = true, data = evaluationList });
            }
            catch (Exception ex)
            {
                return Ok(new { status = false, message = ex.Message });
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
                return Ok(new { status = true,data = attendances });
            }
            catch (Exception ex)
            {
                return Ok(new { status = false, message = ex.Message });
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
                return Ok(new { status = true, data = parent });
            }
            catch (Exception ex)
            {
                return Ok(new { status = false, message = ex.Message });
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
                    e.BirthDate,
                    e.Nationality,
                    e.TraineeAddress,
                    e.ResidencyNumber,
                    Parent = _context.Parents.Where(a => a.IsActive && a.ParentId == e.ParentId).Select(a => new { a.ParentId, a.ParentName, a.ParentEmail, a.ParentPhone, a.Image, a.IsActive }).FirstOrDefault(),
                }).ToListAsync();
                return Ok(new { status = true, data = children });
            }
            catch (Exception ex)
            {
                return Ok(new { status = false, message = ex.Message });
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
                    e.BirthDate,
                    e.ResidencyNumber,
                    e.Nationality,
                    e.TraineeAddress,
                    Parent = _context.Parents.Where(a => a.IsActive && a.ParentId == e.ParentId).Select(a => new { a.ParentId, a.ParentName, a.ParentEmail, a.ParentPhone, a.Image, a.IsActive }).FirstOrDefault(),
                }).FirstOrDefaultAsync();
                return Ok(new { status = true,data = child });
            }
            catch (Exception ex)
            {
                return Ok(new { status = false, message = ex.Message });
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
                    e.StartDate,
                    e.IsActive,
                    e.TraineeId,
                    Trainee = _context.Trainees.Where(a => a.IsActive && a.TraineeId == e.TraineeId).Select(a => new { a.TraineeId, a.TraineeName, a.TraineePhone, a.TraineeEmail, a.Image, a.IsActive }).FirstOrDefault(),
                    Category = _context.Categories.Where(a => a.IsActive && a.CategoryId == e.CategoryId).Select(a => new { a.CategoryId, a.CategoryName, a.CategoryDescription, a.image, a.IsActive,
                        Department = _context.Departments.Where(b => b.IsActive && b.DepartmentId == a.DepartmentId).Select(b => new { b.DepartmentId, b.DepartmentName, b.DepartmentDescription, b.Image, b.IsActive }).FirstOrDefault(),
                    }).FirstOrDefault(),
                }).ToListAsync();
                return Ok(new { status = true, subscriptions });
            }
            catch (Exception ex)
            {
                return Ok(new { status = false, message = ex.Message });
            }
        }

        [HttpGet]
        [Route("GetChildCompetitionTeamsByChildId")]
        public async Task<ActionResult> GetChildCompetitionTeamsByChildId(int TraineeId)
        {
            try
            {
               var teams = await _context.TraineeCompetitionTeams.Where(e => e.TraineeId == TraineeId && e.IsActive).Select(e => new
               {
                    e.Id,
                    e.IsActive,
                    e.TraineeId,
                    e.CompetitionTeamId,
                    CompetitionTeam = _context.CompetitionTeam.Where(a => a.IsActive && a.Id == e.CompetitionTeamId).Select(a => new { 
                        a.Id, a.Name, a.Image, a.IsActive,
                        Department = _context.CompetitionDepartment.Where(b => b.IsActive && b.Id == a.CompetitionDepartmentId).Select(b => new { b.Id, b.Name, b.Image, b.IsActive }).FirstOrDefault(),
                        Trainer = _context.Trainers.Where(b => b.IsActive && b.TrainerId == a.TrainerId).Select(b => new { b.TrainerId, b.TrainerName, b.TrainerEmail, b.TrainerPhone, b.Image, b.IsActive }).FirstOrDefault(),
                    }).FirstOrDefault(),
                }).ToListAsync();
                return Ok(new { status = true, teams });
               
            }
            catch (Exception ex)
            {
                return Ok(new { status = false, message = ex.Message });
            }
        }

        #endregion

        #region GetTrainersByDepartmentId
        [HttpGet]
        [Route("GetTrainersByDepartmentId")]
        public async Task<IActionResult> GetTrainersByDepartmentId(int DepartmentId)
        {
            var department = await _context.Departments.Where(e => e.DepartmentId == DepartmentId && e.IsActive).FirstOrDefaultAsync();
            if (department == null)
            {
                return Ok(new { status = false, message = "Department not found!" });
            }
            var trainers = await _context.Trainers.Where(e => e.CurrentDepartment == DepartmentId && e.IsActive).ToListAsync();
            return Ok(new {status = true , data = trainers});
        }
        #endregion

        #region GetTrainerCategoriesByTrainerId
        [HttpGet]
        [Route("GetTrainerCategoriesByTrainerId")]
        public async Task<ActionResult> GetTrainerCategoriesByTrainerId(int TrainerId)
        {
            try
            {
                var trainerCategories = await _context.CategoryTrainers.Where(e => e.TrainerId == TrainerId && e.IsActive).Select(e => new
                {
                    e.CategoryId,
                    Category = _context.Categories.Where(a => a.IsActive && a.CategoryId == e.CategoryId).Select(a => new { a.CategoryId, a.CategoryName, a.CategoryDescription, a.image, a.IsActive }).FirstOrDefault(),
                    Trainer = _context.Trainers.Where(a => a.IsActive && a.TrainerId == e.TrainerId).Select(a => new { a.TrainerId, a.TrainerName, a.TrainerEmail, a.TrainerPhone, a.Image, a.IsActive }).FirstOrDefault()
                }).ToListAsync();
                return Ok(new { status = true, trainerCategories });

            }
            catch (Exception ex)
            {
                return Ok(new { status = false, message = ex.Message });
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
        public async Task<IActionResult> GetDepartmentsByBranchId(int BranchId)
        {
            try
            {
                var departments = await _context.Departments.Include(e => e.Branch).Where(e => e.BranchId == BranchId && e.IsActive == true && e.Branch.IsActive).ToListAsync();
                return Ok(new { status = true, departments });
            }
            catch (Exception ex)
            {
                return Ok(new { status = false, message = ex.Message });
            }

        }
        #endregion
        
        #region GetBranchDetailsById
        [HttpGet]
        [Route("GetBranchDetailsById")]
        public async Task<IActionResult> GetBranchDetailsById(int BranchId)
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
            return Ok(new {status = true , branch});
        }
        #endregion
        
        #region GetAllBranches
        [HttpGet]
        [Route("GetAllBranches")]
        public async Task<IActionResult> GetAllBranches()
        {
            try
            {
                var branches = await _context.Branches.Where(e => e.IsActive).ToListAsync();
                return Ok(new { status = true, branches });
            }
            catch (Exception ex)
            {
                return Ok(new { status = false, message = ex.Message });


            }
        }
        #endregion

        #region Champions
        [HttpGet]
        [Route("GetAllChampions")]
        public async Task<IActionResult> GetAllChampions()
        {
            try
            {
                var champions = await _context.Champions.Where(e => e.IsDeleted == false).ToListAsync();
                return Ok(new { status = true, champions });
            }
            catch (Exception ex)
            {
                return Ok(new { status = false, message = ex.Message });


            }
        }

        #endregion

        #region Competitions 
        [HttpGet]
        [Route("GetCompetitionDepartments")]
        public async Task<IActionResult> GetCompetitionDepartments()
        {
            try
            {
                var CompetitionDepartments = await _context.CompetitionDepartment.Where(e => e.IsActive && !e.IsDeleted).ToListAsync();
                return Ok(new { status = true, CompetitionDepartments });
            }
            catch (Exception ex)
            {

               return Ok(new { status = false, message = ex.Message });


            }
        }

        [HttpGet]
        [Route("GetCompetitionTeamsByDepartmentId")]
        public async Task<IActionResult> GetCompetitionTeamsByDepartmentId(int departmentId)
        {
            try
            {
                var competitionDepartment = await _context.CompetitionDepartment.Where(e => e.IsActive && !e.IsDeleted && e.Id == departmentId).FirstOrDefaultAsync();
                if (competitionDepartment == null)
                {
                    return Ok(new { status = false, message = "Department not found!" });
                }
                var teams = await _context.CompetitionTeam.Where(e => e.IsActive && !e.IsDeleted && e.CompetitionDepartmentId == departmentId).ToListAsync();
                return Ok(new { status = true, teams });
            }
            catch (Exception ex)
            {

                return Ok(new { status = false, message = ex.Message });


            }
        }

        [HttpGet]
        [Route("GetCompetitionTeamsByTrainerId")]
        public async Task<IActionResult> GetCompetitionTeamsByTrainerId(int trainerId)
        {
            try
            {
                var trainer = await _context.Trainers.Where(e => e.IsActive && e.TrainerId == trainerId).FirstOrDefaultAsync();
                if (trainer == null)
                {
                    return Ok(new { status = false, message = "trainer not found!" });
                }
                var teams = await _context.CompetitionTeam.Where(e => e.IsActive && !e.IsDeleted && e.TrainerId == trainerId)
                    .Select(e => new
                    {
                        e.Id,
                        e.Name,
                        e.Image,
                        e.IsActive,
                        e.IsDeleted,
                        e.TrainerId,
                        e.CompetitionDepartmentId,
                        Trainer = _context.Trainers.Where(a => a.IsActive && a.TrainerId == e.TrainerId).Select(a => new { a.TrainerId, a.TrainerName, a.TrainerEmail, a.TrainerPhone, a.Image, a.IsActive }).FirstOrDefault(),
                        Department = _context.CompetitionDepartment.Where(a => a.IsActive && a.Id == e.CompetitionDepartmentId).Select(a => new { a.Id, a.Name, a.Image, a.IsActive }).FirstOrDefault(),
                    }).ToListAsync();
                    
                return Ok(new { status = true, teams });
            }
            catch (Exception ex)
            {

                return Ok(new { status = false, message = ex.Message });


            }
        }

        [HttpGet]
        [Route("GetTeamChildrenByTeamId")]
        public async Task<IActionResult> GetTeamChildrenByTeamId(int teamId)
        {
            try
            {
                var team = await _context.CompetitionTeam.Where(e => e.IsActive && !e.IsDeleted && e.Id == teamId).FirstOrDefaultAsync();
                if (team == null)
                {
                    return Ok(new { status = false, message = "team not found!" });
                }
                var teamChildren = await _context.TraineeCompetitionTeams.Where(e => e.IsActive && !e.IsDeleted && e.CompetitionTeamId == teamId).ToListAsync();
                return Ok(new { status = true, teamChildren });
            }
            catch (Exception ex)
            {

                return Ok(new { status = false, message = ex.Message });


            }
        }
        
        [HttpGet]
        [Route("GetAllTeamDetailsByTeamId")]
        public async Task<IActionResult> GetAllTeamDetailsByTeamId(int teamId)
        {
            try
            {
                var team = await _context.CompetitionTeam.Where(e => e.IsActive && !e.IsDeleted && e.Id == teamId).FirstOrDefaultAsync();
                if (team == null)
                {
                    return Ok(new { status = false, message = "team not found!" });
                }
                var teamDepartment = await _context.CompetitionDepartment.Where(e => e.IsActive && !e.IsDeleted && e.Id == team.CompetitionDepartmentId).FirstOrDefaultAsync();
                var trainer = await _context.Trainers.Where(e => e.IsActive && e.TrainerId == team.TrainerId).FirstOrDefaultAsync();
                var teamChildren = await _context.TraineeCompetitionTeams.Where(e => e.IsActive && !e.IsDeleted && e.CompetitionTeamId == teamId).ToListAsync();
               
                var teamDetails = new
                {
                    team,
                    teamDepartment,
                    trainer,
                    teamChildren
                };
                return Ok(new { status = true, teamDetails });
            }
            catch (Exception ex)
            {

                return Ok(new { status = false, message = ex.Message });


            }
        }


        #endregion
        

        #region CategoryTeams
        [HttpGet]
        [Route("GetAllSubscriptionsByCategoryId")]
        public async Task<ActionResult> GetAllSubscriptionsByCategoryId(int CategoryId)
        {
            try
            {
                var subscriptions = await _context.Subscriptions.Where(e => e.CategoryId == CategoryId && e.IsActive)
                    .Select(e => new
                    {
                        e.SubscriptionId,
                        e.StartDate,
                        e.EndDate,
                        e.IsActive,
                        e.TraineeId,
                        Trainee = _context.Trainees.Where(a => a.IsActive && a.TraineeId == e.TraineeId).Select(a => new { a.TraineeId, a.TraineeName, a.TraineePhone, a.TraineeEmail, a.Image, a.IsActive , a.BirthDate , a.Nationality , a.TraineeAddress }).FirstOrDefault(),
                    }).ToListAsync();
                   
                return Ok(new { status = true, subscriptions });
            }
            catch (Exception ex)
            {
                return Ok(new { status = false, message = ex.Message });
            }
                
               
        }
           
        

        #endregion




    }
}