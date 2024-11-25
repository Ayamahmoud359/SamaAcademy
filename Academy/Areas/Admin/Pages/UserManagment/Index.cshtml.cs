using Academy.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Academy.Areas.Admin.Pages.UserManagment
{
    public class IndexModel : PageModel
    {
        [BindProperty]
        public AddUserVM userVM { get; set; }
        public void OnGet()
        {
        }
    }
}
