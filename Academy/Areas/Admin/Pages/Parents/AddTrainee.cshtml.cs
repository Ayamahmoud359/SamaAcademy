using Academy.Data;
using Academy.DTO;
using Academy.Models;
using Azure;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using NToastNotify;
using System.ComponentModel.DataAnnotations;

namespace Academy.Areas.Admin.Pages.Parents
{
    public class AddTraineeModel : PageModel
    {
        [BindProperty]
        public TraineeVM trainee { get; set; }

        [BindProperty]
        [Required(ErrorMessage = "User Name is required")]
      
        [Display(Name = "User Name")]
        [Remote("IsEmailAvailable", "Functions", ErrorMessage = "This User Name is already taken.")]
        public string UserName { get; set; }

        public List<SelectListItem> Nationalities { get; set; }


        private readonly AcademyContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IToastNotification _toastNotification;
        private readonly IWebHostEnvironment _hostEnvironment;

        public AddTraineeModel(AcademyContext context
            , UserManager<ApplicationUser> userManager
            , SignInManager<ApplicationUser> signInManager
            ,IToastNotification toastNotification
            ,IWebHostEnvironment hostEnvironment)
        {
            trainee = new TraineeVM();

            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
            _toastNotification = toastNotification;
           _hostEnvironment = hostEnvironment;

      

        }
 

public class Country
{
    public Name Name { get; set; }
    public string Cca2 { get; set; }
}

public class Name
{
    public string Common { get; set; }
}

public async Task<IActionResult> OnGet(int id)
        {
            try
            {

                if (id != 0)
                {
                    trainee.ParentId = id;
                    using (var client = new HttpClient())
                    {
                        var response = await client.GetStringAsync("https://restcountries.com/v3.1/all");
                        var countries = JsonConvert.DeserializeObject<List<Country>>(response);

                        Nationalities = countries.Select(c => new SelectListItem
                        {
                            Text = c.Name.Common,
                            Value = c.Cca2
                        }).ToList();
                        
                    }
                    return Page();
                }

                
                
                return RedirectToPage("../Error");
            }
            catch (Exception e)
            {
                return RedirectToPage("../Error");
            }


        }
    

            public async Task<IActionResult> OnPostAsync(IFormFile? fileUpload)
            {
           
           
                try
                {
                using (var client = new HttpClient())
                {
                    var response = await client.GetStringAsync("https://restcountries.com/v3.1/all");
                    var countries = JsonConvert.DeserializeObject<List<Country>>(response);

                    Nationalities = countries.Select(c => new SelectListItem
                    {
                        Text = c.Name.Common,
                        Value = c.Cca2
                    }).ToList();

                }
                if (!ModelState.IsValid)
                {
                    _toastNotification.AddErrorToastMessage("Something Went Wrong");
                    return Page();
                }
                var newtrainee = new Trainee
                {
                    TraineeName = trainee.TraineeName,
                    TraineePhone = trainee.TraineePhone,
                    TraineeAddress = trainee.TraineeAddress,
                    TraineeEmail = trainee.TraineeEmail,
                    BirthDate = trainee.BirthDate,
                    ParentId = trainee.ParentId,
                    Nationality = trainee.Nationality,
                    ResidencyNumber = trainee.ResidencyNumber,
                    UserName = UserName,
                    IsActive = true
                };
                if (fileUpload != null && fileUpload.Length > 0)
                {
                    string folder = "uploads/Trainees/";
                    newtrainee.Image = await UploadImage(folder, fileUpload);

                }
                _context.Trainees.Add(newtrainee);
                    await _context.SaveChangesAsync();
              
                    var user = new ApplicationUser
                    {
                        UserName = UserName,
                        Email = trainee.TraineeEmail==null?"":trainee.TraineeEmail,
                        PhoneNumber = trainee.TraineePhone,
                        EntityId = newtrainee.TraineeId,
                        EntityName = "Trainee"

                    };


                    var result = await _userManager.CreateAsync(user, trainee.Password);

                    if (result.Succeeded)
                    {
                    _toastNotification.AddSuccessToastMessage("Trainee Added Successfully");
                    return Redirect("~/Admin/Trainees/Index");

                    }
                   
                    _context.Trainees.Remove(newtrainee);
                    _context.SaveChanges();
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                    


                }
                catch (Exception e)
                {
                    ModelState.AddModelError(string.Empty, e.Message);
                  
                }
            _toastNotification.AddErrorToastMessage("Something Went Wrong");
            return Page();
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
