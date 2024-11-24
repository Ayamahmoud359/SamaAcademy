using Academy.Data;
using Academy.DTO;
using Academy.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using NToastNotify;

namespace Academy.Areas.Admin.Pages.Banners
{
    public class IndexModel : PageModel
    {
       
        private readonly AcademyContext _context;
        [BindProperty]
        public int BannerId { set; get; }
        private readonly IToastNotification _toastNotification;
        public IndexModel(AcademyContext context, IToastNotification toastNotification)
        {
            _context = context;
            _toastNotification = toastNotification;
        }
        public void OnGet()
        {

        }

     
        public async Task<IActionResult> OnPostDeleteBanner()
        {
            try
            {
                var banner = _context.Banners.Find(BannerId);
                if (banner != null)
                {
                    
                    
                   
                    
                    banner.IsActive = false;
                    banner.IsDeleted = true;

                
                    _context.Attach(banner).State = EntityState.Modified;
                    await _context.SaveChangesAsync();
                    _toastNotification.AddSuccessToastMessage("Deleted Successfull");

                    return RedirectToPage("Index");
                }
               
                    
            }
            catch (Exception)

            {
               
            }
            _toastNotification.AddErrorToastMessage("Something went wrong");
        

            return RedirectToPage("Index");

        }
    }
}
