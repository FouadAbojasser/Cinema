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


        public IActionResult Index(int theaterId)
        {
            theaterId = 1;

            var Cinema1Scheduel = _unitOfWork.ShowTime.Get(e => e.TheaterId == theaterId,
                                                            [m=>m.MovieTheater.Movie,
                                                             s=>s.MovieTheater.Movie.Images,
                                                             g=>g.MovieTheater.Movie.Genres]);
            
            return View(Cinema1Scheduel.ToList());
        }


        public async Task<IActionResult> AddMovieToCinemaAsync(DateOnly StartDate, DateOnly EndDate, int TotalTickets, int MovieId, int CinemaId)
        {
            var MovieTheater = new MovieTheater
            {
                MovieId = MovieId,
                TheaterId = CinemaId,
                StartDate = StartDate,
                EndDate = EndDate,
                TotalNumberOfTickets = TotalTickets,

            };

            await _unitOfWork.MovieTheater.CreateAsync(MovieTheater);
            await _unitOfWork.MovieTheater.CommitAsync();

            return RedirectToAction("MovieDetails", "Home", new {Area = "Guest", Id =  MovieId});
        }



    }
}
