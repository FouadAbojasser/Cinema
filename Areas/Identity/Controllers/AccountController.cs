using Cinema.Models;
using Cinema.Models.ViewModels;
using Cinema.Utility;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
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
        private readonly IEmailSender _emailSender;
        public AccountController(UserManager<ApplicationUser> userManager, 
            SignInManager<ApplicationUser> signInManager,
            IEmailSender emailSender)
        {
            _userManager = userManager  ;
            _signInManager = signInManager;
            _emailSender = emailSender;
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
                if (!applicationUser.EmailConfirmed)
                {
                    //Generating User Token
                    string userToken = await _userManager.GenerateEmailConfirmationTokenAsync(applicationUser);

                    //Generating Confirmation Link
                    var link = Url.Action("ConfirmEmail", "Account", new { area = "Identity" , applicationUser.Id, userToken }, Request.Scheme);

                    //Generating HTML Confirmation Message
                    string templatePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "email-templates", "confirm.html");
                    string emailBody = await System.IO.File.ReadAllTextAsync(templatePath);
                    emailBody = emailBody.Replace("{{UserName}}", applicationUser.UserName)
                                         .Replace("{{ConfirmationLink}}", link);

                    //Sending Confirmation Email
                    await _emailSender.SendEmailAsync(applicationUser.Email, "Confirmation Email", emailBody);

                    TempData["Notification"] = "User Registered Successfully!, Please Confirm Your Email";

                    return RedirectToAction("Index", "Home", new { area = "Guest" });
                }
                else
                {

                  TempData["Notification"] = "You Have Perviuosly Confirmed Your Email Address!!";

                }


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
                    // Correct Password
                    // Check if the Email is Confirmed ot not?
                    if (applicationUser.EmailConfirmed)
                    {
                        await _signInManager.SignInAsync(applicationUser, loginVM.RememberMe);

                        TempData["Notification"] = "Login Successfully";

                        return RedirectToAction("Index", "Home", new { area = "Guest" });
                    }
                    else
                    {
                        TempData["ConfirmEmail"] = "Please Confirm Your Email First!";

                        return RedirectToAction("Index", "Home", new { area = "Guest" });
                    }
                  
                }
                else
                {
                    //Wrong Password
                    ModelState.AddModelError("Password", "Invalid Password!");
                    return View(loginVM);
                }
                            
            }

                ModelState.AddModelError("UserNameOrEmail", "Invalid User Name or Email!");

                return View(loginVM);
        }

        public async Task<IActionResult> LogoutAsync()
        {
            await _signInManager.SignOutAsync();

            TempData["Notification"] = "Logout Successfully!";

            return RedirectToActionPermanent("Index", "Home", new { area = "Guest" });
        }

        public async Task<IActionResult> ConfirmEmailAsync(string Id , string UserToken)
        {
           var applicationUser = await _userManager.FindByIdAsync(Id);
           
            
            if (applicationUser is null)
            {
                //User Not Found
                return RedirectToAction("NotFoundPage", "Home", new {area = "Admin"});

            }

            if (applicationUser.EmailConfirmed)
            {
                TempData["Notification"] = "Your Email is Already Confirmed!";

                return RedirectToAction("Index", "Home", new { area = "Guest" });

            }
            var result = await _userManager.ConfirmEmailAsync(applicationUser, UserToken);

            if (result.Succeeded)
            {
                TempData["Notification"] = "Email Confirmed Successfully!";

                return RedirectToAction("Index", "Home", new { area = "Guest" });

            }
            else
            {
                TempData["Notification"] = string.Join(", ",result.Errors.Select(e=>e.Description));
            }

         return BadRequest();

        }



        public IActionResult ResetPasswordRequest()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ResetPasswordRequestAsync(string UserNameOrEmail)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            var applicationUser = await _userManager.FindByNameAsync(UserNameOrEmail);

            if (applicationUser is null)
            {
                applicationUser = await _userManager.FindByEmailAsync(UserNameOrEmail);
            }

           return View(applicationUser);

        }





    }
}
