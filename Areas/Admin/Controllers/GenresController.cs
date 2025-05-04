using Cinema.Data;
using Microsoft.AspNetCore.Mvc;

namespace Cinema.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class GenresController : Controller
    {
        private readonly ApplicationDbContext _dBcontext = new ApplicationDbContext(); 
        public IActionResult Index()
        {
            var genres = _dBcontext.Genres;

            return View(genres.ToList());
        }
    }
}
