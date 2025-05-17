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
        private readonly IReviewRepository _reviewRepository;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IMovieRepository _movieRepository;

        public UserActivitiesController(IReviewRepository reviewRepository, UserManager<ApplicationUser> userManager, IMovieRepository movieRepository)
        {
            _reviewRepository = reviewRepository;
            _userManager = userManager;
            _movieRepository = movieRepository;
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
            var movie =  _movieRepository.GetOne(e => e.Id == reviewVM.MovieId,null,false);

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

            await _reviewRepository.CreateAsync(userReview);
            await _reviewRepository.CommitAsync();

            return RedirectToAction("MovieDetails", "Home", new { area = "Guest", id = movie.Id });
        }



    }
}
