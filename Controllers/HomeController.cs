using System.Diagnostics;
using Cinema.Data;
using Cinema.Models;
using Cinema.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Cinema.Controllers
{
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




        public IActionResult MovieDetails()
        {
            return View();
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
}
