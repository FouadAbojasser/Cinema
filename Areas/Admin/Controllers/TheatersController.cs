using Cinema.Models;
using Cinema.Models.ViewModels;
using Cinema.Repositories.IRepositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Cinema.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class TheatersController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public TheatersController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            var AllTheaters = _unitOfWork.Theater.Get(null, [t => t.Movies]);
            return View(AllTheaters);
        }

        public IActionResult Create()
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> CreateAsync(Theater theater)
        {
            if (theater != null)
            {
                if (!ModelState.IsValid)
                {
                    return View(theater);
                }

                await _unitOfWork.Theater.CreateAsync(theater);

                await _unitOfWork.Theater.CommitAsync();

                TempData["SuccessMessage"] = "Created Successfully";

                return RedirectToAction(nameof(Index));
            }

            return View(theater);
        }



        public IActionResult Edit(int id)
        {
            var theater = _unitOfWork.Theater.GetOne(e => e.Id == id);
            if (theater != null)
            {
                return View(theater);
            }
            return RedirectToAction("NotFoundPage", "Home");
        }



        [HttpPost]
        public async Task<IActionResult> EditAsync(Theater theater)
        {
            var oldTheaterInDB = _unitOfWork.Theater.GetOne(e => e.Id == theater.Id, null);
            
            if (ModelState.IsValid && oldTheaterInDB != null)
            {
                theater.Rate= oldTheaterInDB.Rate;

                _unitOfWork.Theater.Update(theater);

                await _unitOfWork.Theater.CommitAsync();

                TempData["SuccessMessage"] = "Edited Successfully";

                return RedirectToAction(nameof(Index));
            }

            return View(theater); // Return to edit form if model is invalid
        }


        public async Task<IActionResult> Delete(int id)
        {

            var theater = _unitOfWork.Theater.GetOne(a => a.Id == id, [a => a.Movies]);

            if (theater == null)
            {
                return RedirectToAction("NotFoundPage", "Home");
            }

            theater.Movies.Clear();

            _unitOfWork.Theater.Delete(theater);

            await _unitOfWork.Theater.CommitAsync();

            TempData["SuccessMessage"] = "Deleted successfully";

            return RedirectToAction(nameof(Index));

        }


        public IActionResult Schedule(int id)
        {
            var TheaterSchedule = _unitOfWork.TheaterSchedule.Get(e=>e.TheaterId == id, [m=>m.Movie!, m=>m.Movie!.Images]);

            if(!TheaterSchedule.Any())
            {
                return RedirectToAction(nameof(AddSchedule), new { id = id });
            }
            return View(TheaterSchedule);
        }

        public IActionResult AddSchedule (int id)
        {
            var Theater = _unitOfWork.Theater.GetOne(e=>e.Id==id, [m=>m.Movies]);

            var TheaterWithSchedule = new TheaterWithScheduleVM
            {
                Theater = Theater!,

            };

            return View(TheaterWithSchedule);
        }

        [HttpPost]
        public async Task<IActionResult> AddScheduleAsync(TheaterWithScheduleVM theaterWithSchedule)
        {
            if (!ModelState.IsValid && theaterWithSchedule is null)
            {
                return View(theaterWithSchedule);
            }

            var movie = _unitOfWork.Movie.GetOne(m => m.Id == theaterWithSchedule.TheaterSchedule!.MovieId);

            TheaterSchedule theaterSchedule = new();

            theaterSchedule.TheaterId = theaterWithSchedule.Theater!.Id;

            theaterSchedule.MovieId = theaterWithSchedule.TheaterSchedule!.MovieId;

            theaterSchedule.ShowDate=theaterWithSchedule.TheaterSchedule.ShowDate;

            theaterSchedule.ShowTimeFrom = theaterWithSchedule.TheaterSchedule.ShowTimeFrom;

            if (movie is not null)
            {
                theaterSchedule.ShowTimeTo = theaterWithSchedule.TheaterSchedule.ShowTimeFrom.AddMinutes(movie.Duration + 15);
            }

            await _unitOfWork.TheaterSchedule.CreateAsync(theaterSchedule);

            await _unitOfWork.TheaterSchedule.CommitAsync();

            TempData["SuccessMessage"] = "Added Successfully";

            return RedirectToAction(nameof(Schedule), new { Id = theaterSchedule.TheaterId});

        }

        public async Task<IActionResult> DeleteScheduleAsync(int Id)
        {
            var TheaterSchedule = _unitOfWork.TheaterSchedule.GetOne(s=>s.Id == Id);
            

            if (TheaterSchedule is null)
            {
                return NotFound();
            }
            var theaterId = TheaterSchedule.TheaterId;

            _unitOfWork.TheaterSchedule.Delete(TheaterSchedule);

            await _unitOfWork.TheaterSchedule.CommitAsync();

            TempData["SuccessMessage"] = "Deleted Successfully";

            return RedirectToAction(nameof(Schedule), new { id = theaterId });

        }


        public IActionResult EditSchedule(int Id)
        {
            var TheaterSchedule = _unitOfWork.TheaterSchedule.GetOne(s => s.Id == Id, [t=>t.Theater!]);

            if (TheaterSchedule is null)
            {
                return NotFound();
            }

            var Theater = _unitOfWork.Theater.GetOne(e => e.Id == TheaterSchedule.TheaterId, [m => m.Movies]);

            TheaterWithScheduleVM theaterWithScheduleVM = new TheaterWithScheduleVM
            {
                Theater = Theater,
                TheaterSchedule = TheaterSchedule
            };

            return View(theaterWithScheduleVM);
        }


        [HttpPost]
        public async Task<IActionResult> EditScheduleAsync(TheaterSchedule theaterSchedule)
        {
            var TheaterScheduleInDb = _unitOfWork.TheaterSchedule.GetOne(s => s.Id == theaterSchedule.Id);

            var movie = _unitOfWork.Movie.GetOne(m => m.Id == theaterSchedule.MovieId);

            if (TheaterScheduleInDb is null)
            {
                return NotFound();
            }

            TheaterScheduleInDb.ShowDate = theaterSchedule.ShowDate;
            TheaterScheduleInDb.ShowTimeFrom = theaterSchedule.ShowTimeFrom;
            TheaterScheduleInDb.MovieId = theaterSchedule.MovieId;

            if (movie is not null)
            {
                TheaterScheduleInDb.ShowTimeTo=theaterSchedule.ShowTimeFrom.AddMinutes(movie.Duration + 15);
                TheaterScheduleInDb.Movie = movie;
            }

            _unitOfWork.TheaterSchedule.Update(TheaterScheduleInDb);
            await _unitOfWork.TheaterSchedule.CommitAsync();

            TempData["SuccessMessage"] = "Editied Successfully";

            return RedirectToAction(nameof(Schedule),new {Id = TheaterScheduleInDb.TheaterId });
        }


    }
}
