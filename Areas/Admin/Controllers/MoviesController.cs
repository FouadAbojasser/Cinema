using Cinema.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Cinema.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class MoviesController : Controller
    {
        private readonly ApplicationDbContext _dbcontext = new ApplicationDbContext();
        public IActionResult Index()
        {
            var movies = _dbcontext.Movies
                 .Include(e => e.Genres)
                 .Include(e => e.Director);

            return View(movies.ToList());
        }
    }
}
