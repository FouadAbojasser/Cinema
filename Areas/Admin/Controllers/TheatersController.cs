using Cinema.Models;
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
            var AllTheaters = _unitOfWork.Theater.Get(null, [t => t.MovieTheater]);
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

                TempData["SuccessMessage"] = "Created successfully";

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

                TempData["SuccessMessage"] = "Edited successfully";

                return RedirectToAction(nameof(Index));
            }

            return View(theater); // Return to edit form if model is invalid
        }


        public async Task<IActionResult> Delete(int id)
        {

            var theater = _unitOfWork.Theater.GetOne(a => a.Id == id, [a => a.MovieTheater]);

            if (theater == null)
            {
                return RedirectToAction("NotFoundPage", "Home");
            }

            theater.MovieTheater.Clear();

            _unitOfWork.Theater.Delete(theater);

            await _unitOfWork.Theater.CommitAsync();

            TempData["SuccessMessage"] = "Deleted successfully";

            return RedirectToAction(nameof(Index));

        }













    }
}
