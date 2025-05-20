using System.Security.Claims;
using Cinema.Models;
using Cinema.Models.ViewModels;
using Cinema.Repositories;
using Cinema.Repositories.IRepositories;
using Cinema.Utility;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages;
using NuGet.Common;

namespace Cinema.Areas.Identity.Controllers
{
    [Area("Identity")]
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IEmailSender _emailSender;
        //private readonly IOTPRepository _otp;
        private readonly IUnitOfWork _unitOfWork;


        public AccountController(
                        UserManager<ApplicationUser> userManager, 
                        SignInManager<ApplicationUser> signInManager,
                        RoleManager<IdentityRole> roleManager,
                        IEmailSender emailSender, IUnitOfWork unitOfWork)
        { 
            _userManager = userManager  ;
            _roleManager = roleManager ;
            _signInManager = signInManager;
            _emailSender = emailSender;
            //_otp = oTP;
            _unitOfWork = unitOfWork;
        }

        public async Task<IActionResult> RegisterAsync()
        {
            //must be in dB initializer not in any action
            if (!_roleManager.Roles.Any())
            {
               await _roleManager.CreateAsync(new IdentityRole(SD.SuperAdmin));
               await _roleManager.CreateAsync(new IdentityRole(SD.Admin));
               await _roleManager.CreateAsync(new IdentityRole(SD.Cinema));
               await _roleManager.CreateAsync(new IdentityRole(SD.Customer));
            }

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
                DoB = (DateOnly)registerVM.DoB!,
                
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

                    await _userManager.AddToRoleAsync(applicationUser, SD.Customer);

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
        public async Task<IActionResult> ResetPasswordRequestAsync(ResetPasswordRequestVM resetPasswordRequestVM)
        {
            if (!ModelState.IsValid)
            {
                return View(resetPasswordRequestVM);
            }

            var applicationUser = await _userManager.FindByNameAsync(resetPasswordRequestVM.UserNameOrEmail);

            if (applicationUser is null)
            {
                applicationUser = await _userManager.FindByEmailAsync(resetPasswordRequestVM.UserNameOrEmail);
            }

            if (applicationUser is null)
            {
                ModelState.AddModelError(string.Empty, "Username or Email does not exist!");

                return View(resetPasswordRequestVM);
            }
            else
            {
                if(resetPasswordRequestVM.ResetMethod == "OTP")
                {

                    //Using OTP
                    int GenOTP = new Random().Next(1000, 9999);

                    //Needed for ResetPassword
                    string token = await _userManager.GeneratePasswordResetTokenAsync(applicationUser);

                    var userLastOTP = _unitOfWork.OTP.Get(e=>e.ApplicationUserId==applicationUser.Id).LastOrDefault();

                    if ((DateTime.UtcNow - userLastOTP!.RequestDateTime).TotalMinutes > 30)
                    {

                        //Add OTP to Database
                        await _unitOfWork.OTP.CreateAsync(new OTP()
                        {
                            OTP_Number = GenOTP,
                            ApplicationUserId = applicationUser.Id,
                            RequestDateTime = DateTime.UtcNow,
                            ExpairationDateTime = DateTime.UtcNow.AddMinutes(30),
                            UsedByUser = false

                        });

                        await _unitOfWork.OTP.CommitAsync();

                        //Generating HTML OTP Message
                        string templatePathOTP = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "email-templates", "PasswordResetOTP.html");
                        string emailBodyOTP = await System.IO.File.ReadAllTextAsync(templatePathOTP);
                        emailBodyOTP = emailBodyOTP.Replace("{{UserName}}", applicationUser.UserName)
                                             .Replace("{{YourOTP}}", GenOTP.ToString());

                        //Sending Confirmation Email
                        await _emailSender.SendEmailAsync(applicationUser.Email!, "Reset Password Email", emailBodyOTP);

                        TempData["Notification"] = "Password Reset has been requested successfully!";

                        TempData["_validationToken"] = Guid.NewGuid().ToString();

                        return RedirectToAction("NewPasswordOTP", "Account", new { area = "Identity", Token = token, ApplicationUserId = applicationUser.Id });
                    }
                    else
                    {
                        var timeSinceLastOTP = DateTime.UtcNow - userLastOTP.RequestDateTime;

                        var remainingTime = TimeSpan.FromMinutes(30) - timeSinceLastOTP;

                        ModelState.AddModelError(string.Empty, $"You can use OTP after {remainingTime.ToString("mm\\:ss")} minutes!");
                        
                        return View(resetPasswordRequestVM);
                        
                    }
                    
                }

                else if (resetPasswordRequestVM.ResetMethod == "ConfirmationLink")
                {
                    //Using Token
                    string token = await _userManager.GeneratePasswordResetTokenAsync(applicationUser);

                    var ResetPasswordConfirmationLink = Url.Action("NewPassword", "Account", new { area = "Identity", Token = token, ApplicationUserId = applicationUser.Id }, Request.Scheme);

                    //Generating HTML Confirmation Message
                    string templatePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "email-templates", "PasswordResetLink.html");
                    string emailBody = await System.IO.File.ReadAllTextAsync(templatePath);
                    emailBody = emailBody.Replace("{{UserName}}", applicationUser.UserName)
                                         .Replace("{{ConfirmationLink}}", ResetPasswordConfirmationLink);

                    //Sending Confirmation Email
                    await _emailSender.SendEmailAsync(applicationUser.Email!, "Reset Password Email", emailBody);


                    TempData["Notification"] = "Password Reset has been requested successfully!";

                    //used to prevent accessing Password reset page from the link directly
                    TempData["_validationToken"] = Guid.NewGuid().ToString();

                }

                return View(nameof(CheckInbox));

            }
        }

        public IActionResult CheckInbox()
        {
            return View();
        }


        public IActionResult NewPassword()
        {
            if (TempData["_validationToken"] is not null)
            {
             return View();
            }
            return BadRequest();
        }

        [HttpPost]
        public async Task<IActionResult> NewPasswordAsync(NewPasswordVM newPasswordVM)
        {
            if (!ModelState.IsValid)
            {
                return View(newPasswordVM);
            }

            var applicationUser = await _userManager.FindByIdAsync(newPasswordVM.ApplicationUserId);

            if (applicationUser is not null)
            {
                var result = await _userManager.ResetPasswordAsync(applicationUser, newPasswordVM.Token, newPasswordVM.NewPassword);

                if (result.Succeeded)
                {
                    TempData["Notification"] = "Yor Password has been reset Successfully!";

                    return RedirectToAction("Index", "Home", new { area = "Guest" });

                }
                else
                {
                    TempData["Notification"] = string.Join(", ", result.Errors.Select(e => e.Description));
                }
            }

            return BadRequest();

        }


        public IActionResult NewPasswordOTP()
        {
            if (TempData["_validationToken"] is not null)
            {
                return View();
            }
            return BadRequest();
        }

        [HttpPost]
        public async Task<IActionResult> NewPasswordOTPAsync(NewPasswordOTPVM newPasswordOTPVM)
        {

            if (!ModelState.IsValid)
            {
                return View(newPasswordOTPVM);
            }

            var applicationUser = await _userManager.FindByIdAsync(newPasswordOTPVM.ApplicationUserId);

            if (applicationUser is not null)
            {
                var OTPinDB = _unitOfWork.OTP.Get(e => e.ApplicationUserId == newPasswordOTPVM.ApplicationUserId).LastOrDefault();

                if (OTPinDB != null && OTPinDB.OTP_Number == newPasswordOTPVM.OTP && DateTime.UtcNow < OTPinDB.ExpairationDateTime)
                {
                    var result = await _userManager.ResetPasswordAsync(applicationUser,newPasswordOTPVM.Token, newPasswordOTPVM.NewPassword);

                    if (result.Succeeded)
                    {
                        TempData["Notification"] = "Yor Password has been reset Successfully!";

                        OTPinDB.UsedByUser = true;
                        _unitOfWork.OTP.Update(OTPinDB);
                        await _unitOfWork.OTP.CommitAsync();

                        return RedirectToAction("Index", "Home", new { area = "Guest" });
                    }
                    else
                    {
                        TempData["Notification"] = string.Join(", ", result.Errors.Select(e => e.Description));
                    }

                       
                }
                else if(OTPinDB != null && OTPinDB.OTP_Number == newPasswordOTPVM.OTP && DateTime.UtcNow > OTPinDB.ExpairationDateTime)
                {
                    ModelState.AddModelError(string.Empty, "OTP Expired!");

                    return View(newPasswordOTPVM);
                }
                else if (OTPinDB != null && OTPinDB.OTP_Number != newPasswordOTPVM.OTP)
                {
                    ModelState.AddModelError(string.Empty, "Invalid OTP!");

                    return View(newPasswordOTPVM);
                }

            }

            return View(newPasswordOTPVM);

        }




    }
}
