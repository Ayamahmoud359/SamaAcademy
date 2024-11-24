using Academy.Data;
using Academy.DTO;
using Academy.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using NToastNotify;
using System.ComponentModel.DataAnnotations;

namespace Academy.Areas.Admin.Pages.Trainers
{
    public class EditModel : PageModel
    {
        [BindProperty]
        public Trainer Trainer { get; set; }
       
        

        private readonly AcademyContext _context;
        private readonly IToastNotification _toastNotification;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IWebHostEnvironment _hostEnvironment;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ApplicationDbContext _applicationDb;
        public EditModel(AcademyContext context
            ,IToastNotification toastNotification
              , ApplicationDbContext applicationDb
            , UserManager<ApplicationUser> userManager
              , IWebHostEnvironment hostEnvironment)
        {
            _context = context;
           _toastNotification = toastNotification;
            _applicationDb = applicationDb;
            _userManager = userManager;
            _hostEnvironment = hostEnvironment;
        }


        public async Task<IActionResult> OnGetAsync(int? id) { 
            try
            {
             Trainer=  _context.Trainers.FirstOrDefault(t => t.TrainerId == id);
                if (Trainer!= null)
                {
                    
                    return Page();
                }

                return RedirectToPage("../NotFound");
            }
            catch
            {
                return RedirectToPage("../Error");
            }
         
            
        }


        public async Task<IActionResult> OnPostAsync(IFormFile? fileUpload)
        {
           
        
            try
            {
                var TrainerToEdit = await _context.Trainers.FirstOrDefaultAsync(m => m.TrainerId == Trainer.TrainerId);
                if (Trainer.UserName!= TrainerToEdit.UserName)
                {
                    var checkuser = await _userManager.FindByNameAsync(Trainer.UserName);
                    if (checkuser != null)
                    {
                        ModelState.AddModelError(string.Empty, "User Name alredy Token");
                        _toastNotification.AddErrorToastMessage("SomeThing Went Wrong");
                        return Page();
                    }
                    var ExistedUser = await _userManager.FindByNameAsync(TrainerToEdit.UserName);
                    if (ExistedUser != null)
                    {
                        ExistedUser.UserName = Trainer.UserName;
                        var result = await _userManager.UpdateAsync(ExistedUser);
                        if (result.Succeeded)
                        {
                            TrainerToEdit.UserName = Trainer.UserName;

                        }
                        else
                        {
                            foreach (var error in result.Errors)
                            {
                                ModelState.AddModelError(string.Empty, error.Description);
                            }
                            _toastNotification.AddErrorToastMessage("SomeThing Went Wrong");
                            return Page();
                        }
                    }

                }
                TrainerToEdit.TrainerName = Trainer.TrainerName;
                TrainerToEdit.TrainerAddress = Trainer.TrainerAddress;
                TrainerToEdit.TrainerPhone = Trainer.TrainerPhone;
                TrainerToEdit.HiringDate = Trainer.HiringDate;
                TrainerToEdit.IsActive = Trainer.IsActive;

                if (fileUpload != null && fileUpload.Length > 0)
                {
                    //if (TrainerToEdit.Image != null)
                    //{
                    //    var ImagePath = Path.Combine(_hostEnvironment.WebRootPath, TrainerToEdit.Image);
                    //    if (System.IO.File.Exists(ImagePath))
                    //    {
                    //        System.IO.File.Delete(ImagePath);
                    //    }
                    //}
                    string folder = "uploads/Trainers/";
                    TrainerToEdit.Image = await UploadImage(folder, fileUpload);

                }
                _context.Attach(TrainerToEdit).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                _toastNotification.AddSuccessToastMessage("Trainer Information Edited Suuccessfully");
                return Redirect("Index");

            }
            catch (Exception e)
            {
                ModelState.AddModelError(string.Empty, e.Message);
                _toastNotification.AddErrorToastMessage("SomeThing Went Wrong");
                return Page();
            }

        }


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

    }
}
