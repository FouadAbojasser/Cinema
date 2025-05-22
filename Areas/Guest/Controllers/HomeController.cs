using System.Diagnostics;
using AspNetCoreGeneratedDocument;
using Cinema;
using Cinema.Models;
using Cinema.Models.ViewModels;
using Cinema.Repositories.IRepositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Cinema.Areas.Guest.Controllers;
[Area("Guest")]
public class HomeController : Controller
{

    //private readonly ApplicationDbContext _dbcontext = new ();
  
    //Inject UnitOfWork Only
    private readonly IUnitOfWork _unitOfWork;

    //private readonly IMovieRepository _movieRepository;

    //private readonly IActorRepository _actorRepository;

    //private readonly IGenreRepository _genreRepository;

    //private readonly IReviewRepository _reviewRepository;

    

    public HomeController(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
        //_movieRepository = movieRepository;
        //_actorRepository = actorRepository;
        //_genreRepository = genreRepository;
        //_reviewRepository = reviewRepository;
    }


    public IActionResult Index()
    {

        var today = DateOnly.FromDateTime(DateTime.Now);

        //var Latest = _dbcontext.Movies
        //    .Where(e => e.ProductionDate < today)
        //    .Include(e => e.Images)
        //    .OrderByDescending(e => e.ProductionDate)
        //    .Take(6);

        var Latest = _unitOfWork.Movie.Get(
            e => e.ReleaseDate < today, 
            [e => e.Images],
            false,
            e=>e.OrderByDescending(x=>x.ReleaseDate)
            ).Take(6);


        //var TopRated = _dbcontext.Movies
        //    .Include(e => e.Actors) 
        //    .Include(e => e.Images)
        //    .OrderByDescending(e => e.Rate) 
        //    .ThenByDescending(e => e.ProductionDate) 
        //    .Take(6);


        var TopRated = _unitOfWork.Movie.Get(
            null,
            [e => e.Actors, e => e.Images],
            false,
            q => q.OrderByDescending(m => m.Rate)
                   .ThenByDescending(m => m.ProductionDate)
            ).Take(6);


        //var CommingSoon = _dbcontext.Movies
        //    .Where(e => e.ReleaseDate >= today)
        //    .Include(e => e.Images)
        //    .OrderByDescending(e => e.ReleaseDate)
        //    .Take(6);


        var CommingSoon = _unitOfWork.Movie.Get(
            e => e.ReleaseDate >= today,
            [e => e.Images],
            true,
            e => e.OrderByDescending(e => e.ReleaseDate)).Take(6);


        //var RecentlyReleased = _dbcontext.Movies
        //    .Where(e => e.ProductionDate < today)
        //    .Include(e => e.Images)
        //    .OrderByDescending(e => e.ProductionDate)
        //    .Take(6);

        var RecentlyReleased = _unitOfWork.Movie.Get(
            e => e.ReleaseDate >= today.AddDays(-60) && e.ReleaseDate <= today,
            [e => e.Images],
            false,
            e => e.OrderByDescending(e => e.ProductionDate)).Take(6);


        //var TopTrailerSection = _dbcontext.Movies
        //    .Include(e => e.Genres)
        //    .Include(e => e.Images)
        //    .OrderByDescending(e => e.Rate)
        //    .ThenByDescending(e => e.ReleaseDate)
        //    .Take(4);

        var TopTrailerSection = _unitOfWork.Movie.Get(
            null,
            [g => g.Genres, m => m.Images],
            false,
            o => o.OrderByDescending(e => e.Rate).ThenByDescending(e => e.ReleaseDate)
            ).Take(4);


        var MoviesReturnData = new MoviesWithFiltersVM()
        {

            Latest = Latest.ToList(),
            TopRated = TopRated.ToList(),
            CommingSoon = CommingSoon.ToList(),
            RecentlyReleased = RecentlyReleased.ToList(),
            TopTrailerSection = TopTrailerSection.ToList(),

        };

        return View(MoviesReturnData);
    }



    public IActionResult MovieDetails(int id)
    {
        //var movie = _dbcontext.Movies
        // .Include(e => e.Images)
        // .Include(e => e.Genres)
        // .Include(e => e.Actors)
        // .Include(e => e.Director)
        // .Include(e => e.MovieTheaters)
        // .FirstOrDefault(e => e.Id == id);

        //GetOne لها الأن عدد 2 امبلمنتيشن واحد جاي من
        //الريبوزتري العام والثاني جاي من الريبوزتري الخاص بالموفي 

       
        var movie = _unitOfWork.Movie.GetOne(
            e => e.Id == id,
            query => query
            .Include(e => e.Images)
            .Include(e => e.Genres)
            .Include(e => e.Actors)
            .Include(e => e.Director)
            .Include(e => e.MovieTheater)
            .ThenInclude(mt => mt.Theater)
            .Include(r => r.MovieReviews)); //Deep Include of Navigation Property Movie->Review->ApplicationUser

        if (movie == null)
        {
            return RedirectToAction(nameof(Index));
        }

        // Get the current movie's genre IDs
        var movieGenreIds = movie.Genres.Select(g => g.Id).ToList();


        // Get similar movies based on at least 2 matching genres, and exclude the original movie
        //var similarMovies = _dbcontext.Movies
        //    .Where(m => m.Id != id)
        //    .Include(m => m.Genres)
        //    .Include(m => m.Images)
        //    .AsEnumerable() // Move to memory to do genre matching
        //    .Where(m => m.Genres.Select(g => g.Id).Intersect(movieGenreIds).Count() >= 2);


        var similarMovies = _unitOfWork.Movie.Get(
            e => e.Id != id && e.Genres.Select(g => g.Id).Intersect(movieGenreIds).Count() >= 2,
            query => query
            .Include(e => e.Images)
            .Include(e => e.MovieTheater)
                .ThenInclude(mt => mt.Theater)
);

        var movieWithSimilarMoviesWithReviews = new MovieWithSimilarMoviesWithReviewsVM
        {
            Movie = movie,

            SimilarMovies = similarMovies.ToList(),

            MovieReviews = movie.MovieReviews.ToList(),

        };


        return View(movieWithSimilarMoviesWithReviews);
    }



    public IActionResult ActorDetails(int id)
    {
        
        var actor = _unitOfWork.Actor.GetOne(
            e => e.Id == id,
            [e => e.Movies]
            );


        var actorMovies = _unitOfWork.Movie.Get(
            m => m.Actors.Any(a => a.Id == id),
            [m => m.Actors, e => e.Images]
            );

        var ActorWithMovies = new ActorWithMovies
        {
            Actor = actor,
            ActorMovies = actorMovies.ToList()
        };

        return View(ActorWithMovies);
    }


    public IActionResult Movies()
    {
        var movies = _unitOfWork.Movie.Get(null, [e => e.Genres, e => e.Images]);
          
        var geners = _unitOfWork.Genre.Get();

        var moviesWithGeners = new MoviesWithGenresVM
        {
            Movies = movies.ToList(),
            Genres = geners.ToList(),
        };

        return View(moviesWithGeners);
    }

    //public IActionResult _Comments(int MovieId, int skip = 0)
    //{
    //    var OneReviewAtATime = _reviewRepository
    //        .Get(m => m.Movie!.Id == MovieId, [u=>u.applicationUser!])
    //        .Skip(skip)
    //        .Take(1)
    //        .ToList();

    //    return PartialView("_Comment", OneReviewAtATime);
    //}




    public IActionResult Privacy()
    {
        return View();
    }


    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
