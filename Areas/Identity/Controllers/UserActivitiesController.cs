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
        public async Task<IActionResult> AddReviewAsync(MovieReviews review)
        {
            var UserReview = _unitOfWork.MovieReviews.GetOne(e => e.MovieId == review.MovieId && e.ApplicationUserName == review.ApplicationUserName);
            if (UserReview is null)
            {
                //New Comment to be Added
                review.CreatedAt = DateTime.UtcNow;
                await _unitOfWork.MovieReviews.CreateAsync(review);
                await _unitOfWork.MovieReviews.CommitAsync();
                TempData["SingleMovie"] = "Yor Comment has been Recorded!";
            }
            else
            {
                //Update Old Comment
                UserReview.Comment= review.Comment;
                review.CreatedAt = DateTime.UtcNow;
                _unitOfWork.MovieReviews.Update(UserReview);
                await _unitOfWork.MovieReviews.CommitAsync();
                TempData["SingleMovie"] = "Yor Comment has been Updated!";
            }

            return RedirectToAction("MovieDetails", "Home", new { area = "Guest", id = review.MovieId });


        }


        [HttpPost]
        public async Task<IActionResult> UserMovieRatesAsync(MovieRates movieRates)
        {
            var UserRateForThisMovie = _unitOfWork.MovieRates.GetOne(e => e.ApplicationUserName == movieRates.ApplicationUserName && e.MovieId == movieRates.MovieId);
            if(UserRateForThisMovie is null)
            {
                //New Rate to be Added
                await _unitOfWork.MovieRates.CreateAsync(movieRates);
                await _unitOfWork.MovieRates.CommitAsync();
                TempData["SingleMovie"] = "Yor Rate has been Recorded";
            }
            else
            {
                //user Rate Already Exist, Update it
                UserRateForThisMovie.ActorsRate = movieRates.ActorsRate;
                UserRateForThisMovie.DirectorRate = movieRates.DirectorRate;
                UserRateForThisMovie.GraphicsRate = movieRates.GraphicsRate;
                UserRateForThisMovie.OverallRate = movieRates.OverallRate;
                _unitOfWork.MovieRates.Update(UserRateForThisMovie);
                await _unitOfWork.MovieRates.CommitAsync();
                TempData["SingleMovie"] = "Yor Rate has been Updated";

            }

            //Update Movie Rate
            var MovieRates = _unitOfWork.MovieRates.Get(e => e.MovieId == movieRates.MovieId).Select(e=>e.OverallRate);
            var AverageRateOfMovie = Math.Round((double)MovieRates.Average()!, 1);
            var MovieToUpdateRate = _unitOfWork.Movie.GetOne(m=>m.Id== movieRates.MovieId);
            MovieToUpdateRate!.Rate = (double)AverageRateOfMovie!;
            _unitOfWork.Movie.Update(MovieToUpdateRate);
            await _unitOfWork.Movie.CommitAsync();

            return RedirectToAction("MovieDetails", "Home", new { area = "Guest", Id = movieRates.MovieId });
        }



    }
}
