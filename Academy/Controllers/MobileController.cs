﻿using Academy.Data;
using Academy.DTO;
using Academy.Helpers;
using Academy.Models;
using Academy.ViewModels;
//using DevExpress.XtraPrinting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.Operations;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using System;
using System.Linq;
using System.Reflection;

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
        private readonly ApplicationDbContext _db;
        private readonly IWebHostEnvironment _hostEnvironment;


        public MobileController(UserManager<ApplicationUser> userManager, ApplicationDbContext dbContext, IWebHostEnvironment hostEnvironment, SignInManager<ApplicationUser> signInManager, AcademyContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
            _hostEnvironment = hostEnvironment;
            _db = dbContext;
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
                    // 1- Trainer
                    if (user.EntityName == "Trainer")
                    {
                        var trainer = await _context.Trainers
                            .FirstOrDefaultAsync(e => e.TrainerId == user.EntityId);
                        return Ok(new { status = true,userId = user.Id, type_id = 1, message = "User logged in successfully!", trainer });
                    }
                    // 2- Parent
                    if (user.EntityName == "Parent")
                    {
                        var parent = await _context.Parents
                            .FirstOrDefaultAsync(e => e.ParentId == user.EntityId);
                        return Ok(new { status = true, userId = user.Id, type_id = 2, message = "User logged in successfully!", parent });
                    }

                    // 3- Trainee
                    if (user.EntityName == "Trainee")
                    {
                        var trainee = await _context.Trainees
                            .FirstOrDefaultAsync(e => e.TraineeId == user.EntityId);
                        return Ok(new { status = true, userId = user.Id, type_id = 3, message = "User logged in successfully!", trainee });
                    }

                    // 4- Admin
                    if (user.EntityName == "Admin")
                    {
                        return Ok(new { status = true, userId = user.Id, type_id = 4, message = "User logged in successfully!", user });
                    }

                    // 5- GeneralManager
                    if (user.EntityName == "GeneralManager")
                    {
                       return Ok(new { status = true, userId = user.Id, type_id = 5, message = "User logged in successfully!", user });
                    }
                    // 6- BranchManager
                    if (user.EntityName == "BranchManager")
                    {
                        return Ok(new { status = true, userId = user.Id, type_id = 6, message = "User logged in successfully!", user });
                    }
                    // 7- BranchAccountant
                    if (user.EntityName == "BranchAccountant")
                    {
                        return Ok(new { status = true, userId = user.Id, type_id = 7, message = "User logged in successfully!", user });
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

        #region ForgetPassword
        [HttpPost]
        [Route("ForgetPassword")]
        public async Task<IActionResult> ForgetPassword(string login, string newPassword, string confirmNewPassword)
        {

            var user = await _userManager.FindByEmailAsync(login)
                  ?? await _userManager.FindByNameAsync(login);

           
            if (user != null)
            {
                if(newPassword != confirmNewPassword)
                {
                    return Ok(new { status = false, message = "Passwords do not match!" });
                }
                // Update the password
                var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                var result = await _userManager.ResetPasswordAsync(user, token, newPassword);
                if (result.Succeeded)
                {
                    return Ok(new { status = true, message = "Password reset successfully!" });
                }


                
            }

            // Return an invalid response
            return Ok(new { status = false, message = "Invalid user" });
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
                //if(updateUserProfileDTO.Image != null)
                //{
                //    //trainer.Image = updateUserProfileDTO.Image;
                //}
               
        
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
                //if (updateUserProfileDTO.Image != null)
                //{
                //    //parent.Image = updateUserProfileDTO.Image;
                //}
               
               

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


        [HttpPost]
        [Route("UpdateIdentityUserProfile")]
        public async Task<IActionResult> UpdateIdentityUserProfile([FromForm] UpdateUserProfileDTO updateUserProfileDTO, IFormFile? Pic)
        {
            // Find the user in the Identity system
            var user = await _userManager.FindByIdAsync(updateUserProfileDTO.UserId);
            if (user == null)
            {
                return Ok(new { status = false, message = "User not found!" });
            }

            // Update the profile based on the EntityName
            if (user != null)
            {
               

                // Update Trainer fields
                if (updateUserProfileDTO.Name != null)
                {
                    user.FullName = updateUserProfileDTO.Name;
                }
                if (Pic != null)
                {
                    user.Image = await UploadImage("uploads/Users/", Pic);

                }

                if (updateUserProfileDTO.Email != null)
                {
                   
                    user.Email = updateUserProfileDTO.Email;
                }
                if (updateUserProfileDTO.Phone != null)
                {
                    user.PhoneNumber = updateUserProfileDTO.Phone;
                }
                if (updateUserProfileDTO.Address != null)
                {
                    user.Address = updateUserProfileDTO.Address;
                }
              

                // Save changes
               
                var userUpdateResult = await _userManager.UpdateAsync(user);
                if (!userUpdateResult.Succeeded)
                {
                    return Ok(new { status = false, message = "Failed to update user profile in Identity!" });
                }

                await _context.SaveChangesAsync();
                return Ok(new { status = true, message = "User profile updated successfully!" });
            }
            
          
            return Ok(new { status = false, message = "User not found!" });
        }




        [HttpPost]
        [Route("UpdateChildData")]
        public async Task<IActionResult> UpdateChildData([FromForm] UpdateChildProfileDTO updateChildProfileDTO, IFormFile? Pic)
        {
            try
            {
                var child = await _context.Trainees.FirstOrDefaultAsync(e => e.TraineeId == updateChildProfileDTO.TraineeId);

                if (child == null)
                {
                    return Ok(new { status = false, message = "Child not found!" });
                }
                if(updateChildProfileDTO.TraineeName != null)
                {
                    child.TraineeName = updateChildProfileDTO.TraineeName;

                }
                if (updateChildProfileDTO.TraineePhone != null)
                {
                    child.TraineePhone = updateChildProfileDTO.TraineePhone;

                }
                if (updateChildProfileDTO.BirthDate != null)
                {
                    child.BirthDate = updateChildProfileDTO.BirthDate;

                }
                if (updateChildProfileDTO.TraineeAddress != null)
                {
                    child.TraineeAddress = updateChildProfileDTO.TraineeAddress;

                }
                if (Pic != null)
                {
                    child.Image = await UploadImage("uploads/Users/", Pic);

                }
                _context.Trainees.Update(child);
                await _context.SaveChangesAsync();
                return Ok(new { status = true, message = "Child profile updated successfully!" });

            }
            catch (Exception ex)
            {
                return Ok(new { status = false, message = ex.Message });
            }

           
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
                    Trainers = _context.CategoryTrainers
                    .Where(a => a.IsActive && a.CategoryId == e.CategoryId)
                    .Select(a => new 
                        {    a.TrainerId,
                            TrainerData = _context.Trainers.Where(b => b.IsActive && b.TrainerId == a.TrainerId).Select(b => new { b.TrainerId, b.TrainerName, b.TrainerEmail, b.TrainerPhone, b.Image, b.IsActive }).FirstOrDefault()
                        }).ToList()


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

                            Child = _context.Trainees
                                .Where(t => t.IsActive && t.TraineeId == a.Subscription!.TraineeId)
                                .Select(t => new
                                {
                                    t.TraineeId,
                                    t.TraineeName,
                                    t.TraineePhone,
                                    t.TraineeEmail,
                                    t.Image,
                                    t.IsActive
                                })
                                .FirstOrDefault(),

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

                        }).ToList(),
                        trainer = _context.Trainers
                            .Where(t => t.IsActive && t.TrainerId == group.FirstOrDefault().TrainerId)
                            .Select(t => new
                            {
                                t.TrainerId,
                                t.TrainerName,
                                t.TrainerEmail,
                                t.TrainerPhone,
                                t.Image,
                                t.IsActive
                            })
                            .FirstOrDefault()
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

                var subscriptions = await _context.Subscriptions.Where(e => e.TraineeId == TraineeId && e.IsActive && (e.EndDate == null || e.EndDate > today)).Select(e => new
                {
                    e.SubscriptionId,
                    e.EndDate,
                    e.StartDate,
                    e.IsActive,
                    e.TraineeId,
                   
                    Trainee = _context.Trainees.Where(a => a.IsActive && a.TraineeId == e.TraineeId).Select(a => new { a.TraineeId, a.TraineeName, a.TraineePhone, a.TraineeEmail, a.Image, a.IsActive }).FirstOrDefault(),
                    Category = _context.Categories.Where(a => a.IsActive && a.CategoryId == e.CategoryId).Select(a => new { a.CategoryId, a.CategoryName, a.CategoryDescription, a.image, a.IsActive,
                        Department = _context.Departments.Where(b => b.IsActive && b.DepartmentId == a.DepartmentId).Select(b => new { b.DepartmentId, b.DepartmentName, b.DepartmentDescription, b.Image, b.IsActive,b.BranchId }).FirstOrDefault(),
                        Branch = _context.Branches.Where(b => b.IsActive && b.BranchId == a.Department.BranchId).Select(b => new { b.BranchId, b.BranchName, b.BranchAddress, b.Phone, b.Image, b.IsActive }).FirstOrDefault(),
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
        [Route("GetAllChildSubscriptionsByChildId")]
        public async Task<ActionResult> GetAllChildSubscriptionsByChildId(int TraineeId)
        {
            try
            {
                
                var subscriptions = await _context.Subscriptions.Where(e => e.TraineeId == TraineeId && !e.IsDeleted).Select(e => new
                {
                    e.SubscriptionId,
                    e.EndDate,
                    e.StartDate,
                    e.IsActive,
                    e.TraineeId,
                    Trainee = _context.Trainees.Where(a => a.IsActive && a.TraineeId == e.TraineeId).Select(a => new { a.TraineeId, a.TraineeName, a.TraineePhone, a.TraineeEmail, a.Image, a.IsActive }).FirstOrDefault(),
                    Category = _context.Categories.Where(a => a.IsActive && a.CategoryId == e.CategoryId).Select(a => new {
                        a.CategoryId,
                        a.CategoryName,
                        a.CategoryDescription,
                        a.image,
                        a.IsActive,
                        Department = _context.Departments.Where(b => b.IsActive && b.DepartmentId == a.DepartmentId).Select(b => new { b.DepartmentId, b.DepartmentName, b.DepartmentDescription, b.Image, b.IsActive,b.BranchId }).FirstOrDefault(),
                        Branch = _context.Branches.Where(b => b.IsActive && b.BranchId == a.Department.BranchId).Select(b => new { b.BranchId, b.BranchName, b.BranchAddress, b.Phone, b.Image, b.IsActive }).FirstOrDefault(),
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
                var teams = await _context.CompetitionTeam.Where(e => e.IsActive && !e.IsDeleted && e.CompetitionDepartmentId == departmentId)
                    .Select(e=> new
                    {
                        e.Id,
                        e.Name,
                        e.TrainerId,
                        e.Image,
                        e.IsActive,
                        e.IsDeleted,
                        e.CompetitionDepartmentId,
                        Trainer = _context.Trainers.Where(t=> t.TrainerId == e.TrainerId && t.IsActive && !t.IsDeleted)
                        .Select(tt=> new
                        {
                            tt.TrainerId,
                            tt.Image,
                            tt.IsActive,
                            tt.IsDeleted,
                            tt.TrainerName,
                            tt.TrainerPhone,
                            tt.TrainerEmail,
                            tt.TrainerAddress,
                        }).FirstOrDefault()
                    })
                    .ToListAsync();
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
                var teamChildren = await _context.TraineeCompetitionTeams.Where(e => e.IsActive && !e.IsDeleted && e.CompetitionTeamId == teamId)
                    .Select(e => new
                    {
                        e.Id,
                        e.IsActive,
                        e.TraineeId,
                        e.CompetitionTeamId,
                        Trainee = _context.Trainees.Where(a => a.IsActive && a.TraineeId == e.TraineeId).Select(a => new { a.TraineeId, a.TraineeName, a.TraineePhone, a.TraineeEmail, a.Image, a.IsActive , a.TraineeAddress, a.BirthDate }).FirstOrDefault(),
                    })
                    .ToListAsync();
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


        #region Banners 
        [HttpGet]
        [Route("GetAllBanners")]
        public async Task<IActionResult> GetAllBanners()
        {
            var banners = await _context.Banners.Where(e => e.IsActive && !e.IsDeleted).ToListAsync();
            return Ok(new { status = true, data = banners });

        }
        #endregion


        #region CompetitionTeams
        [HttpGet]
        [Route("GetAllChildrenByTeamId")]
        public async Task<ActionResult> GetAllChildrenByTeamId(int TeamId)
        {
            try
            {
                var children = await _context.TraineeCompetitionTeams.Where(e => e.CompetitionTeamId == TeamId && e.IsActive)
                    .Select(e => new
                    {
                        e.Id,
                        e.IsActive,
                        e.TraineeId,
                        e.CompetitionTeamId,
                        Trainee = _context.Trainees.Where(a => a.IsActive && a.TraineeId == e.TraineeId).Select(a => new { a.TraineeId, a.TraineeName, a.TraineePhone, a.TraineeEmail, a.Image, a.IsActive }).FirstOrDefault(),
                    }).ToListAsync();

                return Ok(new { status = true, children });


            }
            catch (Exception ex)
            {
                return Ok(new { status = false, message = ex.Message });
            }


        }

        #endregion



        #region CompetitionTeamEvaluation
        [HttpPost]
        [Route("AddCompetitionTeamEvaluationToChild")]
        public async Task<ActionResult> AddCompetitionTeamEvaluationToChild([FromBody] AddCompetitionTeamEvaluationDTO evaluationDTO)
        {
            try
            {
                var evaluation = new CompetitionTeamEvaluation
                {
                    TrainerId = evaluationDTO.TrainerId,
                    Date = evaluationDTO.EvaluationDate,
                    Review = evaluationDTO.Review,
                    Score = evaluationDTO.Score,
                    TraineeCompetitionTeamId = evaluationDTO.TraineeCompetitionTeamId,
                    IsDeleted = false,
                };
                _context.CompetitionTeamEvaluations.Add(evaluation);
                await _context.SaveChangesAsync();
                return Ok(new { status = true, message = "Evaluation added successfully!" });
            }
            catch (Exception ex)
            {
                return Ok(new { status = false, message = ex.Message });
            }
        }

        [HttpGet]
        [Route("GetEvaluationListByTraineeTeamId")]
        public async Task<ActionResult> GetEvaluationListByTraineeTeamId(int childTeamId)
        {
            try
            {
                var evaluationList = await _context.CompetitionTeamEvaluations.Where(e => e.TraineeCompetitionTeamId == childTeamId && e.IsDeleted == false).Select(e => new
                {
                    e.CompetitionTeamEvaluationId,
                    e.Date,
                    e.Score,
                    e.Review,
                    e.TraineeCompetitionTeamId,
                    e.IsDeleted,
                    Trainer = _context.Trainers.Where(a => a.IsActive && a.TrainerId == e.TrainerId).Select(a => new { a.TrainerId, a.TrainerName, a.TrainerEmail, a.TrainerPhone, a.Image, a.IsActive }).FirstOrDefault(),
                    Team = _context.CompetitionTeam.Where(a => a.IsActive && a.Id == _context.TraineeCompetitionTeams.Where(b => b.IsActive && b.Id == e.TraineeCompetitionTeamId).FirstOrDefault().CompetitionTeamId).Select(a => new { a.Id, a.Name, a.Image, a.IsActive }).FirstOrDefault(),
                }).ToListAsync();
                return Ok(new { status = true, data = evaluationList });
            }
            catch (Exception ex)
            {
                return Ok(new { status = false, message = ex.Message });
            }
        }

        #endregion



        #region CompetitionTeamAttendance

        [HttpPost]
        [Route("AddAttendanceToTeamChildren")]
        public async Task<ActionResult> AddAttendanceToTeamChildren([FromBody] List<AddCompetitionTeamAttendanceDTO> attendanceList)
        {
            try
            {
                if (attendanceList == null || !attendanceList.Any())
                {
                    return Ok(new { status = false, message = "Attendance list is empty!" });
                }
                foreach (var record in attendanceList)
                {
                    var attendance = new CompetitionTeamAbsence
                    {
                        TrainerId = record.TrainerId,
                        AbsenceDate = record.Date,
                        IsAbsent = record.IsAbsent,
                        TraineeCompetitionTeamId = record.TraineeCompetitionTeamId,
                        Type = record.Type,
                        IsDeleted = false,
                    };
                    _context.CompetitionTeamAbsences.Add(attendance);
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
        [Route("GetAttendanceToTeamChildren")]
        public async Task<ActionResult> GetAttendanceToTeamChildren(int teamId)
        {
            try
            {
                // Check if the team exists and is active
                var team = await _context.CompetitionTeam.Where(e => e.Id == teamId && e.IsActive)
                    .FirstOrDefaultAsync();

                if (team == null)
                {
                    return Ok(new { status = false, message = "Team not found!" });
                }

                // Fetch attendance records for the given category
                var attendances = await _context.CompetitionTeamAbsences
                    .Include(e => e.TraineeCompetitionTeam) // Include related subscription
                    .Where(e => e.TraineeCompetitionTeam != null && e.TraineeCompetitionTeam.CompetitionTeamId == teamId)
                    .GroupBy(a => a.AbsenceDate) // Group by date
                    .Select(group => new
                    {
                        Date = group.Key,
                        Records = group.Select(a => new
                        {
                            a.CompetitionTeamAbsenceId,
                            a.AbsenceDate,
                            a.IsAbsent,
                            a.Type,
                            a.TraineeCompetitionTeamId,
                            a.IsDeleted,
                            a.TrainerId,

                            Child = _context.Trainees
                                .Where(t => t.IsActive && t.TraineeId == a.TraineeCompetitionTeam.TraineeId)
                                .Select(t => new
                                {
                                    t.TraineeId,
                                    t.TraineeName,
                                    t.TraineePhone,
                                    t.TraineeEmail,
                                    t.Image,
                                    t.IsActive
                                })
                                .FirstOrDefault(),

                           

                          

                        }).ToList(),
                        trainer = _context.Trainers
                            .Where(t => t.IsActive && t.TrainerId == group.FirstOrDefault().TrainerId)
                            .Select(t => new
                            {
                                t.TrainerId,
                                t.TrainerName,
                                t.TrainerEmail,
                                t.TrainerPhone,
                                t.Image,
                                t.IsActive
                            })
                            .FirstOrDefault()
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
        [HttpGet("GetAttendanceByTraineeCompetitionTeamId")]
        public async Task<IActionResult> GetAttendanceByTraineeCompetitionTeamId(int traineeCompetitionTeamId)
        {
            try
            {
                // Validate if the subscription exists
                var child = await _context.TraineeCompetitionTeams
                    .Where(s => s.Id == traineeCompetitionTeamId && s.IsActive && s.IsDeleted == false)
                    .FirstOrDefaultAsync();

                if (child == null)
                {
                    return Ok(new { status = false, message = "Child not found!" });
                }

                // Fetch attendance records for the given subscription
                var attendanceRecords = await _context.CompetitionTeamAbsences
                    .Where(a => a.TraineeCompetitionTeamId == traineeCompetitionTeamId)
                    .OrderBy(a => a.AbsenceDate) // Order by date
                    .Select(a => new
                    {
                        a.CompetitionTeamAbsenceId,
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


        #region GetUserDetailsById
        [HttpGet]
        [Route("GetUserDetailsById")]
        public async Task<ActionResult> GetUserDetailsById(string userId)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(userId);
                if (user == null)
                {
                    return Ok(new { status = false, message = "User not found!" });
                }
                //var userRoles = await _userManager.GetRolesAsync(user);
                var userDetails = new
                {
                    user.Id,
                    user.UserName,
                    user.Email,
                    user.PhoneNumber,
                    user.Address,
                    user.FullName,
                    user.EntityName,
                    user.BranchId,
                    user.Image,
                  //  userRoles
                };
                return Ok(new { status = true, data = userDetails });
            }
            catch (Exception ex)
            {
                return Ok(new { status = false, message = ex.Message });
            }
                
           
           
        }

        #endregion


        #region Parents 
        [HttpGet]
        [Route("GetAllParents")]
        public async Task<IActionResult> GetAllParents()
        {
            var parents = await _context.Parents.Where(e => e.IsActive && !e.IsDeleted).ToListAsync();
            return Ok(new { status = true, data = parents });
        }
        #endregion

        #region Trainers 
        [HttpGet]
        [Route("GetAllTrainers")]
        public async Task<IActionResult> GetAllTrainers()
        {
            var trainers = await _context.Trainers.Where(e => e.IsActive && !e.IsDeleted).ToListAsync();
            return Ok(new { status = true, data = trainers });
        }
        #endregion

        [HttpPost]
        [Route("EditChildSubscription")]
        public async Task<IActionResult> EditChildSubscription(EditSubscriptionVM SubscriptionVM)
        {
            try
            {
                var oldSubscription = _context.Subscriptions.FirstOrDefault(e => e.SubscriptionId == SubscriptionVM.SubscriptionId);
                if (oldSubscription != null)
                {
                    if (SubscriptionVM.EndDate == SubscriptionVM.StartDate || SubscriptionVM.EndDate < SubscriptionVM.StartDate)
                    {
                        return Ok(new { status = false, message = "EndDate must be greater than StartDate" });

                    }


                    if (SubscriptionVM.StartDate != null)
                    {
                        oldSubscription.StartDate = SubscriptionVM.StartDate;
                    }
                    if (SubscriptionVM.EndDate != null)
                    {
                        oldSubscription.EndDate = SubscriptionVM.EndDate;

                    }
                    if (SubscriptionVM.IsActive != null)
                    {
                        var oldActiveValue = oldSubscription.IsActive;
                        oldSubscription.IsActive = SubscriptionVM.IsActive ?? oldActiveValue;

                    }
                    _context.Attach(oldSubscription).State = EntityState.Modified;
                    await _context.SaveChangesAsync();
                    return Ok(new { status = true, message = "Subscription updated successfully" });
                }

                return Ok(new { status = false, message = "Subscription not found" });

            }
            catch (Exception ex)
            {
                return Ok(new { status = false, message = ex.Message });

            }


        }


        #region ChildrenSearch

        //[HttpGet]
        //[Route("GetBranchChildrenSearch")]
        //public async Task<IActionResult> GetBranchChildrenSearch(int BranchId, string Search)
        //{
        //    try
        //    {
        //        var branch = _context.Branches.FirstOrDefault(e => e.BranchId == BranchId);
        //        if (branch == null)
        //        {
        //            return Ok(new { status = false, message = "Branch not found!" });
        //        }

        //        var children = await _context.Subscriptions
        //                            .Where(ch => ch.IsActive && ch.IsDeleted == false && ch.Category.Department.BranchId == BranchId &&
        //                                         (string.IsNullOrEmpty(Search) || ch.Trainee.TraineeName.ToLower().Contains(Search.ToLower()))

        //                            ).Include(ch => ch.Trainee)
        //                            .Include(ch => ch.Category)

        //                            .ThenInclude(c => c.Department)
        //                            .Select(e=> new
        //                            {
        //                                e.SubscriptionId,
        //                                e.StartDate,
        //                                e.EndDate,
        //                                e.IsActive,
        //                                e.TraineeId,
        //                                Trainee = new
        //                                {
        //                                    e.Trainee.TraineeId,
        //                                    e.Trainee.TraineeName,
        //                                    e.Trainee.TraineePhone,
        //                                    e.Trainee.TraineeEmail,
        //                                    e.Trainee.Image,
        //                                    e.Trainee.IsActive,
        //                                    e.Trainee.ParentId,
        //                                    e.Trainee.BirthDate,
        //                                }
        //                            })
        //                            .ToListAsync();
        //        return Ok(new { status = true, data = children });
        //    }
        //    catch (Exception ex)
        //    {
        //        return Ok(new { status = false, message = ex.Message });

        //    }



        //}


        [HttpGet]
        [Route("GetBranchChildrenSearch")]
        public async Task<IActionResult> GetBranchChildrenSearch(int BranchId, string Search)
        {
            try
            {
                var branch = await _context.Branches.FirstOrDefaultAsync(e => e.BranchId == BranchId);
                if (branch == null)
                {
                    return Ok(new { status = false, message = "Branch not found!" });
                }

                var subscriptions = await _context.Subscriptions
                    .Where(ch => ch.IsActive && !ch.IsDeleted && ch.Category.Department.BranchId == BranchId)
                    .Include(ch => ch.Trainee)
                    .Include(ch => ch.Category)
                    .ThenInclude(c => c.Department)
                    .Select(e => new
                    {
                        e.SubscriptionId,
                        e.StartDate,
                        e.EndDate,
                        e.IsActive,
                        e.TraineeId,
                        Trainee = new
                        {
                            e.Trainee.TraineeId,
                            e.Trainee.TraineeName,
                            e.Trainee.TraineePhone,
                            e.Trainee.TraineeEmail,
                            e.Trainee.Image,
                            e.Trainee.IsActive,
                            e.Trainee.ParentId,
                            e.Trainee.BirthDate,
                        }
                    })
                    .ToListAsync(); // Fetch data into memory

                if (!string.IsNullOrEmpty(Search))
                {
                    var input = AppFunctions.NormalizeArabicText(Search.ToLower());
                    subscriptions = subscriptions
                        .Where(sub => AppFunctions.NormalizeArabicText(sub.Trainee.TraineeName).ToLower().Contains(input))
                        .ToList(); // Apply filtering in memory
                }

                return Ok(new { status = true, data = subscriptions });
            }
            catch (Exception ex)
            {
                return Ok(new { status = false, message = ex.Message });
            }
        }



        //[HttpGet]
        //[Route("GetCategoryChildrenSearch")]
        //public async Task<IActionResult> GetCategoryChildrenSearch(int CategoryId, string Search)
        //{
        //    try
        //    {
        //        var category = _context.Categories.FirstOrDefault(e => e.CategoryId == CategoryId);
        //        if (category == null)
        //        {
        //            return Ok(new { status = false, message = "Category not found!" });
        //        }
        //        var children = await _context.Subscriptions
        //                            .Where(ch => ch.CategoryId == CategoryId && ch.IsActive && ch.IsDeleted == false &&
        //                                         (string.IsNullOrEmpty(Search) || ch.Trainee.TraineeName.ToLower().Contains(Search.ToLower())))

        //                            .Include(ch => ch.Trainee)
        //                            .Select(e => new
        //                            {
        //                                e.SubscriptionId,
        //                                e.StartDate,
        //                                e.EndDate,
        //                                e.IsActive,
        //                                e.TraineeId,
        //                                Trainee = new
        //                                {
        //                                    e.Trainee.TraineeId,
        //                                    e.Trainee.TraineeName,
        //                                    e.Trainee.TraineePhone,
        //                                    e.Trainee.TraineeEmail,
        //                                    e.Trainee.Image,
        //                                    e.Trainee.IsActive,
        //                                    e.Trainee.ParentId,
        //                                    e.Trainee.BirthDate,
        //                                }
        //                            })
        //                            .ToListAsync();
        //        return Ok(new { status = true, data = children });
        //    }
        //    catch (Exception ex)
        //    {
        //        return Ok(new { status = false, message = ex.Message });

        //    }



        //}


        [HttpGet]
        [Route("GetCategoryChildrenSearch")]
        public async Task<IActionResult> GetCategoryChildrenSearch(int CategoryId, string Search)
        {
            try
            {
                var category = await _context.Categories.FirstOrDefaultAsync(e => e.CategoryId == CategoryId);
                if (category == null)
                {
                    return Ok(new { status = false, message = "Category not found!" });
                }

                var subscriptions = await _context.Subscriptions
                    .Where(ch => ch.CategoryId == CategoryId && ch.IsActive && !ch.IsDeleted)
                    .Include(ch => ch.Trainee)
                    .Select(e => new
                    {
                        e.SubscriptionId,
                        e.StartDate,
                        e.EndDate,
                        e.IsActive,
                        e.TraineeId,
                        Trainee = new
                        {
                            e.Trainee.TraineeId,
                            e.Trainee.TraineeName,
                            e.Trainee.TraineePhone,
                            e.Trainee.TraineeEmail,
                            e.Trainee.Image,
                            e.Trainee.IsActive,
                            e.Trainee.ParentId,
                            e.Trainee.BirthDate,
                        }
                    })
                    .ToListAsync(); // Fetch data into memory

                if (!string.IsNullOrEmpty(Search))
                {
                    var input = AppFunctions.NormalizeArabicText(Search.ToLower());
                    subscriptions = subscriptions
                        .Where(sub => AppFunctions.NormalizeArabicText(sub.Trainee.TraineeName).ToLower().Contains(input))
                        .ToList(); // Apply filtering in memory
                }

                return Ok(new { status = true, data = subscriptions });
            }
            catch (Exception ex)
            {
                return Ok(new { status = false, message = ex.Message });
            }
        }




        //[HttpGet]
        //[Route("GetPublicChildrenSearch")]
        //public async Task<IActionResult> GetPublicChildrenSearch(string Search)
        //{
        //    try
        //    {
        //        var query = _context.Trainees
        //            .Where(ch => ch.IsActive && !ch.IsDeleted);

        //        if (!string.IsNullOrEmpty(Search))
        //        {
        //            var input = AppFunctions.NormalizeArabicText(Search.ToLower());
        //            query = query.Where(ch => AppFunctions.NormalizeArabicText(ch.TraineeName).ToLower().Contains(input));
        //        }



        //        var children = await query


        //                            .Select(e => new
        //                            {

        //                                e.IsActive,
        //                                e.TraineeId,
        //                                Trainee = new
        //                                {
        //                                    e.TraineeId,
        //                                    e.TraineeName,
        //                                    e.TraineePhone,
        //                                    e.TraineeEmail,
        //                                    e.Image,
        //                                    e.IsActive,
        //                                    e.ParentId,
        //                                    e.BirthDate,
        //                                }
        //                            })
        //                            .ToListAsync();
        //        return Ok(new { status = true, data = children });
        //    }
        //    catch (Exception ex)
        //    {
        //        return Ok(new { status = false, message = ex.Message });

        //    }



        //}


        [HttpGet]
        [Route("GetPublicChildrenSearch")]
        public async Task<IActionResult> GetPublicChildrenSearch(string Search)
        {
            try
            {
                var query = _context.Trainees
                    .Where(ch => ch.IsActive && !ch.IsDeleted);

                var children = await query
                    .Select(e => new
                    {
                        e.IsActive,
                        e.TraineeId,
                        Trainee = new
                        {
                            e.TraineeId,
                            e.TraineeName,
                            e.TraineePhone,
                            e.TraineeEmail,
                            e.Image,
                            e.IsActive,
                            e.ParentId,
                            e.BirthDate,
                        }
                    })
                    .ToListAsync(); // Fetch data into memory

                if (!string.IsNullOrEmpty(Search))
                {
                    var input = AppFunctions.NormalizeArabicText(Search.ToLower());
                    children = children
                        .Where(ch => AppFunctions.NormalizeArabicText(ch.Trainee.TraineeName).ToLower().Contains(input))
                        .ToList(); // Filter in-memory
                }

                return Ok(new { status = true, data = children });
            }
            catch (Exception ex)
            {
                return Ok(new { status = false, message = ex.Message });
            }
        }



        #endregion


        #region CategoryTeamEvaluation
        [HttpPost]
        [Route("AddCategoryTeamEvaluation")]
        public async Task<ActionResult> AddCategoryTeamEvaluation([FromForm] AddCategoryTeamEvaluationDTO evaluationDTO, IFormFile Image)
        {
            try
            {
                var category = _context.Categories.FirstOrDefault(e => e.CategoryId == evaluationDTO.CategoryId);

                if (category == null)
                {
                    return Ok(new { status = false, message = "Category not found!" });
                }
               
                var evaluation = new CategoryTeamEvaluation
                {
                    TrainerId = evaluationDTO.TrainerId,
                    EvaluationDate = evaluationDTO.EvaluationDate,
                    CategoryId = evaluationDTO.CategoryId,
                    IsDeleted = false,
                };
                if (Image != null && Image.Length > 0)
                {
                    string folder = "uploads/CategoryEvaluations/";
                    evaluation.EvaluationImage = await UploadImage(folder, Image);

                }
                _context.CategoryTeamEvaluation.Add(evaluation);
                await _context.SaveChangesAsync();
                return Ok(new { status = true, message = "Evaluation added successfully!" });
            }
            catch (Exception ex)
            {
                return Ok(new { status = false, message = ex.Message });
            }
        }

        [HttpGet]
        [Route("GetCategoryTeamEvaluationListByCategoryId")]
        public async Task<ActionResult> GetCategoryTeamEvaluationListByCategoryId(int CategoryId)
        {
            try
            {
                var evaluationList = await _context.CategoryTeamEvaluation.Where(e => e.CategoryId == CategoryId && e.IsDeleted == false).Select(e => new
                {
                    e.CategoryTeamEvaluationId,
                    e.EvaluationDate,
                    e.EvaluationImage,
                    e.CategoryId,
                    e.IsDeleted,
                    Trainer = _context.Trainers.Where(a => a.IsActive && a.TrainerId == e.TrainerId).Select(a => new { a.TrainerId, a.TrainerName, a.TrainerEmail, a.TrainerPhone, a.Image, a.IsActive }).FirstOrDefault(),
                    //Category = _context.Categories.Where(a => a.IsActive && a.CategoryId == CategoryId).Select(a => new { a.CategoryId, a.CategoryName, a.CategoryDescription, a.image, a.IsActive }).FirstOrDefault(),
                }).ToListAsync();
                return Ok(new { status = true, data = evaluationList });
            }
            catch (Exception ex)
            {
                return Ok(new { status = false, message = ex.Message });
            }
        }


        #endregion





        #region AllCompetitionTeamEvaluation
        [HttpPost]
        [Route("AddCompetitionTeamEvaluation")]
        public async Task<ActionResult> AddCompetitionTeamEvaluation([FromForm] AddAllCompetitionTeamEvaluationDTO evaluationDTO, IFormFile Image)
        {
            try
            {
                var team = _context.CompetitionTeam.FirstOrDefault(e => e.Id == evaluationDTO.CompetitionTeamId);

                if (team == null)
                {
                    return Ok(new { status = false, message = "Competition Team not found!" });
                }

                var evaluation = new AllCompetitionTeamEvaluation
                {
                    TrainerId = evaluationDTO.TrainerId,
                    EvaluationDate = evaluationDTO.EvaluationDate,
                    CompetitionTeamId = evaluationDTO.CompetitionTeamId,
                    IsDeleted = false,
                };
                if (Image != null && Image.Length > 0)
                {
                    string folder = "uploads/CompetitionTeamEvaluations/";
                    evaluation.EvaluationImage = await UploadImage(folder, Image);

                }
                _context.AllCompetitionTeamEvaluations.Add(evaluation);
                await _context.SaveChangesAsync();
                return Ok(new { status = true, message = "Evaluation added successfully!" });
            }
            catch (Exception ex)
            {
                return Ok(new { status = false, message = ex.Message });
            }
        }

        [HttpGet]
        [Route("GetCompetitionTeamEvaluationListByTeamId")]
        public async Task<ActionResult> GetCompetitionTeamEvaluationListByTeamId(int CompetitionTeamId)
        {
            try
            {
                var evaluationList = await _context.AllCompetitionTeamEvaluations.Where(e => e.CompetitionTeamId == CompetitionTeamId && e.IsDeleted == false).Select(e => new
                {
                    e.AllCompetitionTeamEvaluationId,
                    e.EvaluationDate,
                    e.EvaluationImage,
                    e.CompetitionTeamId,
                    e.IsDeleted,
                    Trainer = _context.Trainers.Where(a => a.IsActive && a.TrainerId == e.TrainerId).Select(a => new { a.TrainerId, a.TrainerName, a.TrainerEmail, a.TrainerPhone, a.Image, a.IsActive }).FirstOrDefault(),
                }).ToListAsync();
                return Ok(new { status = true, data = evaluationList });
            }
            catch (Exception ex)
            {
                return Ok(new { status = false, message = ex.Message });
            }
        }


        #endregion

        #region DeleteAccount
        [HttpPost]
        [Route("DeleteAccount")]

        public IActionResult DeleteAccount(string Profile)
        {
            try
            {
                var profile = _userManager.Users.Where(e=>e.Id==Profile).FirstOrDefault();
                if (profile == null)
                {
                    return Ok(new { status = false, Message = "User Not Found" });
                }
                if(profile.EntityName== "Parent")
                {
                    var children = _context.Trainees.Where(e => e.ParentId == profile.EntityId).ToList();
                    var childrenIds = children.Select(e => e.TraineeId);
                    _context.RemoveRange(children);
                    var parent = _context.Parents.Where(e => e.ParentId == profile.EntityId).FirstOrDefault();
                    if (parent != null)
                    {
                        _context.Remove(parent);
                    }
                    
                    var childrenUsers = _db.Users.Where(e => childrenIds.Contains(e.EntityId)).ToList();
                    _db.RemoveRange(childrenUsers);
                }
                if(profile.EntityName== "Trainer")
                {
                    var trainer = _context.Trainers.Where(e => e.TrainerId == profile.EntityId).FirstOrDefault();
                    if (trainer != null)
                    {
                        _context.Remove(trainer);
                    }
                }
                 _db.Users.Remove(profile);
                _db.SaveChanges();
                return Ok(new { status = true, Message = "Account Deleted Successfully" });


            }

            catch (Exception e)
            {
                return Ok(new { status = false, Message = e.Message });

            }

        }

        #endregion



    }
}