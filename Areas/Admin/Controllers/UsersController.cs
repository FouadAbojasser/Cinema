using System.Data;
using Azure.Identity;
using Cinema.Models;
using Cinema.Models.ViewModels;
using Cinema.Repositories;
using Cinema.Repositories.IRepositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;


namespace Cinema.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class UsersController : Controller
    {
        //private readonly IApplicationUserRepository _unitOfWork.ApplicationUser;
        private readonly IUnitOfWork _unitOfWork;

        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        public UsersController(IUnitOfWork unitOfWork, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            //_unitOfWork.ApplicationUser = userRepository;
            _unitOfWork = unitOfWork;
            _userManager = userManager;
            _roleManager = roleManager;
        }



        public async Task<IActionResult> IndexAsync()
        {
            var AllUsers = _unitOfWork.ApplicationUser.Get();

            List<ApplicationUserVM> allUsersVM = new();

            //Dictionary<ApplicationUser, string> UserRolesDic = new();

            foreach (var user in AllUsers)
            {
                allUsersVM.Add(new()
                {
                    ApplicationUser = user,

                    UserRoles = (List<string>)await _userManager.GetRolesAsync(user)

                });

                //var Roles = await _userManager.GetRolesAsync(user);
                //UserRolesDic.Add(user, string.Join(",", Roles));
            }

            return View(allUsersVM.ToList());
        }


        public async Task<IActionResult> EditAsync(string id)
        {
            var user = _unitOfWork.ApplicationUser.GetOne(e=>e.Id == id);

            var listOfAllRoles = _roleManager.Roles.Select(r => r.Name).ToList();

            if (user is not null)
            {
               var userRoles = await _userManager.GetRolesAsync(user);
               
                var applicationUser = new ApplicationUserVM
                {
                    ApplicationUser = user,

                    UserRoles= userRoles.ToList(),

                    AvailableRoles = listOfAllRoles!
                };

                return View(applicationUser);
            }

            return RedirectToAction("NotFoundPage", "Home");

        }



        [HttpPost]
        public async Task<IActionResult> EditAsync(ApplicationUserVM applicationUserVM)
        {
            var userInDb = _unitOfWork.ApplicationUser.GetOne(e=>e.Id==applicationUserVM.ApplicationUser.Id,null,false);

            if (userInDb is not null)
            {
                var currentRoles = await _userManager.GetRolesAsync(userInDb);

                // Add roles that are in newRoles but not in oldRoles
                var rolesToAdd = applicationUserVM.AvailableRoles.Except(currentRoles);
                await _userManager.AddToRolesAsync(userInDb, rolesToAdd);

                // Remove roles that are in oldRoles but not in newRoles
                var rolesToRemove = currentRoles.Except(applicationUserVM.AvailableRoles);
                await _userManager.RemoveFromRolesAsync(userInDb, rolesToRemove);

                TempData["SuccessMessage"] = "User Editted Successfully!";

                return RedirectToAction(nameof(Index));
            }

            return RedirectToAction("NotFoundPage", "Home");

        }


        public async Task<IActionResult> Delete(string id)
        {

            var userInDb = _unitOfWork.ApplicationUser.GetOne(a => a.Id == id);

            if (userInDb is null)
            {
                return RedirectToAction("NotFoundPage", "Home");
            }

            if (!string.IsNullOrEmpty(userInDb.Image))
            {
                string folderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "img", "users");

                string oldFilePath = Path.Combine(folderPath, userInDb.Image);
                try
                {
                    if (System.IO.File.Exists(oldFilePath))
                    {
                        System.IO.File.Delete(oldFilePath);
                    }
                }
                catch (Exception ex)
                {
                    // Log exception or notify admin if needed
                    Console.WriteLine($"Error deleting file: {ex.Message}");
                }
            }

            await _userManager.DeleteAsync(userInDb);

            TempData["SuccessMessage"] = "Deleted successfully";

            return RedirectToAction(nameof(Index));

        }




    }
}
