using System.Diagnostics;
using Cinema.Data;
using Cinema.Models;
using Cinema.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Cinema.Areas.Guest.Controllers;
[Area("Guest")]
public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly ApplicationDbContext _dbcontext = new ApplicationDbContext();

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }






    public IActionResult Index()
    {
        var today = DateOnly.FromDateTime(DateTime.Now);

        var Latest = _dbcontext.Movies
            .Where(e => e.ProductionDate < today)
            .Include(e => e.Images)
            .OrderByDescending(e => e.ProductionDate)
            .Take(6);


        var TopRated = _dbcontext.Movies
            .Include(e => e.Actors) 
            .Include(e => e.Images)
            .OrderByDescending(e => e.Rate) 
            .ThenByDescending(e => e.ProductionDate) 
            .Take(6);



        var CommingSoon = _dbcontext.Movies
            .Where(e => e.ReleaseDate >= today)
            .Include(e => e.Images)
            .OrderByDescending(e => e.ReleaseDate)
            .Take(6);



        var RecentlyReleased = _dbcontext.Movies
            .Where(e => e.ProductionDate < today)
            .Include(e => e.Images)
            .OrderByDescending(e => e.ProductionDate)
            .Take(6);


        var TopTrailerSection = _dbcontext.Movies
            .Include(e => e.Genres)
            .Include(e => e.Images)
            .OrderByDescending(e => e.Rate)
            .ThenByDescending(e => e.ReleaseDate)
            .Take(4);


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
        var movie = _dbcontext.Movies
         .Include(e => e.Images)
         .Include(e => e.Genres)
         .Include(e => e.Actors)
         .Include(e => e.Director)
         .Include(e => e.MovieTheaters)
             .ThenInclude(mt => mt.Theater)
         .FirstOrDefault(e => e.Id == id);

        if (movie == null)
        {
            return RedirectToAction(nameof(Index));
        }

        // Get the current movie's genre IDs
        var movieGenreIds = movie.Genres.Select(g => g.Id).ToList();

        // Get similar movies based on at least 2 matching genres, and exclude the original movie
        var similarMovies = _dbcontext.Movies
            .Where(m => m.Id != id)
            .Include(m => m.Genres)
            .Include(m => m.Images)
            .AsEnumerable() // Move to memory to do genre matching
            .Where(m => m.Genres.Select(g => g.Id).Intersect(movieGenreIds).Count() >= 2);

        var movieWithSimilarMovies = new MovieWithSimilarMovies
        {
            Movie = movie,
            SimilarMovies = similarMovies.ToList(),
        };


        return View(movieWithSimilarMovies);
    }



    public IActionResult ActorDetails(int id)
    {
        var actor = _dbcontext.Actors
            .Include(e => e.Movies)
            .FirstOrDefault(e => e.Id == id);

        var actorMovies = _dbcontext.Movies
            .Include(e => e.Images)
            .Where(m => m.Actors.Any(a => a.Id == id));
            

        var ActorWithMovies = new ActorWithMovies
        {
            Actor = actor,
            ActorMovies = actorMovies.ToList()
        };

        return View(ActorWithMovies);
    }


    public IActionResult Movies()
    {
        var movies = _dbcontext.Movies
            .Include(e => e.Genres)
            .Include(e => e.Images);

        var geners = _dbcontext.Genres;

        var moviesWithGeners = new MoviesWithGenres
        {
            Movies = movies.ToList(),
            Genres = geners.ToList(),
        };

        return View(moviesWithGeners);
    }












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
