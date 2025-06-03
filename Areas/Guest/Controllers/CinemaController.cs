using System.Linq;
using Cinema.Models;
using Cinema.Models.ViewModels;
using Cinema.Repositories.IRepositories;
using Microsoft.AspNetCore.Mvc;

namespace Cinema.Areas.Guest.Controllers
{
    [Area("Guest")]
    public class CinemaController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public CinemaController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }


        public IActionResult Index(int id)
        {
           
            var CinemaScheduel = _unitOfWork.TheaterSchedule.Get(e => e.TheaterId == id,
                                                            [m=>m.Movie!,
                                                             t=>t.Theater!,
                                                             s=>s.Movie!.Images,
                                                             g=>g.Movie!.Genres]);

            var AllCinemas = _unitOfWork.Theater.Get();

            var selectedTheater = _unitOfWork.Theater.GetOne(t=>t.Id == id);

            if (selectedTheater is null)
            {
                return NotFound();
            }

            var TheatersWithShcedules = new TheatersWithScheduleVM
            {
                Theater = selectedTheater!,
                TheaterSchedules = CinemaScheduel.ToList(),
                ListOfTheater = AllCinemas.ToList(),
            };
            
            return View(TheatersWithShcedules);
        }


        public async Task<IActionResult> AddMovieToCinemaAsync(int MovieId, int CinemaId)
        {
            var movie = _unitOfWork.Movie.Get(m => m.Id== MovieId, [m => m.Theaters],false).FirstOrDefault(m => m.Id == MovieId);

            var theater = _unitOfWork.Theater.Get(t => t.Id == CinemaId, [t => t.Movies],false).FirstOrDefault(t => t.Id == CinemaId);

            if (movie is not null && theater is not null)
            {
                // Check if the relationship already exists
                bool alreadyExists = movie.Theaters.Any(t => t.Id == CinemaId);
                bool alreadyExists2 = theater.Movies.Any(m => m.Id == MovieId);

                if (!alreadyExists && !alreadyExists2)
                {
                    movie.Theaters.Add(theater);
                    _unitOfWork.Movie.Update(movie);
                    await _unitOfWork.Movie.CommitAsync();

                    theater.Movies.Add(movie);
                    _unitOfWork.Theater.Update(theater);
                    await _unitOfWork.Theater.CommitAsync();

                    ViewData["Notification"] = "Movie Added Successfully!";
                }
                else
                {
                    ViewData["Notification-error"] = "Movie Already Exists!";
                }

                return RedirectToAction("MovieDetails", "Home", new { Area = "Guest", Id = MovieId });
            }

            return BadRequest();
        }




    }
}
