using Academy.Data;
using Academy.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using NToastNotify;

namespace Academy.Areas.Admin.Pages.Trainees
{
    public class EditModel : PageModel
    {
        private readonly AcademyContext _context;
        private readonly IToastNotification _toastNotification;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ApplicationDbContext _applicationDb;
        public List<SelectListItem> Nationalities { get; set; }
        public EditModel(AcademyContext context
            ,IToastNotification toastNotification
             , ApplicationDbContext applicationDb
            , UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _toastNotification = toastNotification;
            _applicationDb = applicationDb;
            _userManager = userManager;

        }
        [BindProperty]

        public Trainee Trainee { get; set; }

        public class Country
        {
            public Name Name { get; set; }
            public string Cca2 { get; set; }
        }

        public class Name
        {
            public string Common { get; set; }
        }
        public async Task<IActionResult> OnGetAsync(int? id)
        {

            try
            {

               Trainee= await _context.Trainees.FirstOrDefaultAsync(m => m.TraineeId == id);
                if (Trainee != null)
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
                    return Page();
                }
                return RedirectToPage("../NotFound");
            }
            catch (Exception e)
            {
                return RedirectToPage("../Error");
            }



        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
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
           

            try
            {
                var TraineeToEdit = await _context.Trainees.FirstOrDefaultAsync(m => m.TraineeId == Trainee.TraineeId);
                if (Trainee.UserName != TraineeToEdit.UserName)
                {
                    var checkuser = await _userManager.FindByNameAsync(Trainee.UserName);
                    if (checkuser != null)
                    {
                        ModelState.AddModelError(string.Empty, "User Name alredy Token");
                        _toastNotification.AddErrorToastMessage("SomeThing Went Wrong");
                        return Page();
                    }
                    var ExistedUser = await _userManager.FindByNameAsync(TraineeToEdit.UserName);
                    if (ExistedUser != null)
                    {
                        ExistedUser.UserName = Trainee.UserName;
                        var result = await _userManager.UpdateAsync(ExistedUser);
                        if (result.Succeeded)
                        {
                            TraineeToEdit.UserName = Trainee.UserName;

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
                TraineeToEdit.TraineeName = Trainee.TraineeName;
                TraineeToEdit.TraineeAddress = Trainee.TraineeAddress;
                TraineeToEdit.TraineePhone = Trainee.TraineePhone;
                TraineeToEdit.Nationality = Trainee.Nationality;
                TraineeToEdit.ResidencyNumber = Trainee.ResidencyNumber;
                TraineeToEdit.IsActive = Trainee.IsActive;
                TraineeToEdit.BirthDate = Trainee.BirthDate;
                _context.Attach(TraineeToEdit).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                _toastNotification.AddSuccessToastMessage("Trainee Information Edited Successfully");
                return RedirectToPage("Index");

            }
            catch (Exception e)
            {
                ModelState.AddModelError(string.Empty, e.Message);
                _toastNotification.AddErrorToastMessage("SomeThing Went Error");
                return Page();
            }

        }
    }
}
