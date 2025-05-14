using Cinema.Models;
using Cinema.Models.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Cinema.Areas.Identity.Controllers
{
    [Area("Identity")]
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public AccountController(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager  ;
        }

        public IActionResult Register()
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> RegisterAsync(RegisterVM registerVM)
        {
            if (!ModelState.IsValid)
            {
                return View(registerVM);
            }

            ApplicationUser applicationUser = new ApplicationUser()
            {
                UserName = registerVM.UserName,
                Email = registerVM.Email,
                DoB = (DateOnly)registerVM.DoB,
                
            };

            var result = await _userManager.CreateAsync(applicationUser,registerVM.Password);

            if (result.Succeeded)
            {
                TempData["Notification"] = "User Registered Successfully!";

                return RedirectToAction("Index", "Home" , new { area = "Guest"});
            }
            else
            {
                foreach(var err in result.Errors)
                {

                    ModelState.AddModelError(string.Empty, err.Description);
                }
            }

           return View();

        }











        public IActionResult Login()
        {

            return View();
        }
    }
}
