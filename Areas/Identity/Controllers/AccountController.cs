using Cinema.Models;
using Cinema.Models.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages;

namespace Cinema.Areas.Identity.Controllers
{
    [Area("Identity")]
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager  ;
            _signInManager = signInManager;
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

        [HttpPost]
        public async Task<IActionResult> LoginAsync(LoginVM loginVM)
        {
            if (!ModelState.IsValid)
            {
                return View(loginVM);
            }

            var applicationUser = await _userManager.FindByNameAsync(loginVM.UserNameOrEmail);

            if (applicationUser is null)
            {
                applicationUser = await _userManager.FindByEmailAsync(loginVM.UserNameOrEmail);
            }

            if(applicationUser is not null)
            {
                //UserName or Email Found
                //Check Password
                var result = await _userManager.CheckPasswordAsync(applicationUser, loginVM.Password);

                if (result) 
                {
                    //Correct Password

                    await _signInManager.SignInAsync(applicationUser, loginVM.RememberMe);

                    TempData["Notification"] = "Login Successfully";

                    return RedirectToActionPermanent("Index", "Home", new { area = "Guest" });
                }
                else
                {
                    //Wrong Password
                    ModelState.AddModelError("Password", "Invalid Password!");
                    return View(loginVM);
                }
                            
            }

                ModelState.AddModelError("UserNameOrEmail", "Invalid User Name and Email!");

                return View(loginVM);
        }

      public async Task<IActionResult> LogoutAsync()
        {
            await _signInManager.SignOutAsync();

            TempData["Notification"] = "Logout Successfully!";

            return RedirectToActionPermanent("Index", "Home", new { area = "Guest" });
        }








    }
}
