using Azure.Identity;
using Cinema.Models;
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
        public UsersController(IUnitOfWork unitOfWork, UserManager<ApplicationUser> userManager)
        {
            //_unitOfWork.ApplicationUser = userRepository;
            _unitOfWork = unitOfWork;
            _userManager = userManager;
        }



        public async Task<IActionResult> IndexAsync()
        {
            var AllUsers = _unitOfWork.ApplicationUser.Get();

            Dictionary<ApplicationUser, string> UserRolesDic = new();

            foreach (var user in AllUsers)
            {
                var Roles = await _userManager.GetRolesAsync(user);
                UserRolesDic.Add(user, string.Join(",", Roles));
            }


            return View(UserRolesDic);
        }
    }
}
