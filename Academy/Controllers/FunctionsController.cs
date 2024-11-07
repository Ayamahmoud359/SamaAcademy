using Academy.Data;
using Academy.Data.Migrations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Academy.Controllers
{
    public class FunctionsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public FunctionsController(ApplicationDbContext context)
        {
          
            _context = context;
            
            
        }
     
        [HttpGet]
        public JsonResult IsEmailAvailable(string Email )
        {
            // Check if the email already exists in the database
            var emailExists = _context.Users.Any(e=>e.Email==Email);
           
            
                return new JsonResult(!emailExists);
            
            // Return true if email does not exist, false if it exists
           
        }

    }
}
