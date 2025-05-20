using Cinema.Areas.Guest.Controllers;
using Cinema.Models;
using Cinema.Models.ViewModels;
using Cinema.Repositories;
using Cinema.Repositories.IRepositories;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.IdentityModel.Tokens;

namespace Cinema.Areas.Identity.Controllers
{
    [Area("Identity")]
    public class UserActivitiesController : Controller
    {
        //private readonly IReviewRepository _reviewRepository;
        //private readonly IMovieRepository _movieRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<ApplicationUser> _userManager;
        

        public UserActivitiesController(IUnitOfWork unitOfWork, UserManager<ApplicationUser> userManager)
        {
            //_reviewRepository = reviewRepository;
            //_movieRepository = movieRepository;
            _unitOfWork = unitOfWork;
            _userManager = userManager;
        }

        // This is moving comment navigation to the server side.
        //[HttpGet]
        //public IActionResult GetReviewsByPage(int movieId, int page = 1, int pageSize = 1)
        //{
        //    var reviews = _reviewRepository
        //        .Get(r => r.Movie!.Id == movieId, [u=>u.applicationUser!])
        //        .OrderByDescending(r => r.CreatedAt)
        //        .Skip((page - 1) * pageSize)
        //        .Take(pageSize)
        //        .ToList();

        //    return PartialView("_ReviewPartial", reviews);
        //}



        [HttpPost]
        public async Task<IActionResult> AddReviewAsync(AddReviewVM reviewVM)
        {
            var user = await _userManager.GetUserAsync(User);
            var movie =  _unitOfWork.Movie.GetOne(e => e.Id == reviewVM.MovieId,null,false);

            if (user == null || movie == null)
            {
                return NotFound();
            }

            var userReview = new Review()
            {
                Comment = reviewVM.Comment,
                UserRate = reviewVM.UserRate,
                CreatedAt = DateTime.Now,
                applicationUser = user,
                Movie = movie,
                
                
            };

            await _unitOfWork.Review.CreateAsync(userReview);
            await _unitOfWork.Review.CommitAsync();

            return RedirectToAction("MovieDetails", "Home", new { area = "Guest", id = movie.Id });
        }



    }
}
