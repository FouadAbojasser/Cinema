using Cinema;
using Cinema.Models;
using Cinema.Repositories;
using Cinema.Repositories.IRepositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Cinema.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class GenresController : Controller
    {
        //private readonly ApplicationDbContext _dBcontext = new(); 

        private readonly IUnitOfWork _unitOfWork;

        public GenresController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }


        public IActionResult Index()
        {
            var genres = _unitOfWork.Genre.Get(null,[e=>e.Movies]);

            return View(genres);
        }


        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Genre genre, IFormFile Img)
        {
            if (genre != null && Img != null && Img.Length > 0)
            {
                if (!ModelState.IsValid)
                {
                    return View(genre);
                }

                // Generate unique filename
                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(Img.FileName);

                // Define the folder path
                var folderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\img\\genres");

                // Create folder if it doesn't exist
                if (!Directory.Exists(folderPath))
                {
                    Directory.CreateDirectory(folderPath);
                }

                // Combine folder and filename to get full path
                var filePath = Path.Combine(folderPath, fileName);

                // Save the file
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    Img.CopyTo(stream);
                }

                // Save the image name to the database
                genre.Img = fileName;

                _unitOfWork.Genre.CreateAsync(genre);

                _unitOfWork.Genre.CommitAsync();

                TempData["SuccessMessage"] = "Created successfully";

                return RedirectToAction(nameof(Index));
            }

            return View(genre);
        }


        public IActionResult Edit(int id)
        {
            var genre = _unitOfWork.Genre.GetOne(e=>e.Id==id);

            if (genre != null)
            {
                return View(genre);
            }
            return RedirectToAction("NotFoundPage", "Home");
        }


        [HttpPost]
        public IActionResult Edit(Genre genre, IFormFile Img)
        {
            var oldGenreInDB = _unitOfWork.Genre.GetOne(e => e.Id == genre.Id);

            ModelState.Remove("Img");

            if (ModelState.IsValid && oldGenreInDB != null)
            {
                string folderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "img", "genres");

                // If new image is uploaded
                if (Img != null && Img.Length > 0)
                {
                    string newFileName = Guid.NewGuid().ToString() + Path.GetExtension(Img.FileName);

                    string newFilePath = Path.Combine(folderPath, newFileName);

                    // Ensure directory exists
                    if (!Directory.Exists(folderPath))
                    {
                        Directory.CreateDirectory(folderPath);
                    }

                    // Save new file
                    using (var stream = new FileStream(newFilePath, FileMode.Create))
                    {
                        Img.CopyTo(stream);
                    }

                    // Delete old file if it exists
                    if (!string.IsNullOrEmpty(oldGenreInDB.Img))
                    {
                        string oldFilePath = Path.Combine(folderPath, oldGenreInDB.Img);
                        try
                        {
                            if (System.IO.File.Exists(oldFilePath))
                            {
                                System.IO.File.Delete(oldFilePath);
                            }
                        }
                        catch (Exception ex)
                        {
                            // Log exception or notify admin if needed
                            Console.WriteLine($"Error deleting file: {ex.Message}");
                        }
                    }

                    genre.Img = newFileName; // Update to new image
                }
                else
                {
                    // No new image uploaded, retain old image
                    genre.Img = oldGenreInDB.Img;
                }

                _unitOfWork.Genre.Update(genre);

                _unitOfWork.Genre.CommitAsync();

                TempData["SuccessMessage"] = "Edited successfully";

                return RedirectToAction(nameof(Index));
            }

            return View(genre); // Return to edit form if model is invalid
        }



        public IActionResult Delete(int id)
        {
            var genre = _unitOfWork.Genre.GetOne(g => g.Id == id, [m => m.Movies]);

            if (genre == null)
            {
                return RedirectToAction("NotFoundPage", "Home");
            }

            if (!string.IsNullOrEmpty(genre.Img))
            {
                string folderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "img", "genres");

                string oldFilePath = Path.Combine(folderPath, genre.Img);
                try
                {
                    if (System.IO.File.Exists(oldFilePath))
                    {
                        System.IO.File.Delete(oldFilePath);
                    }
                }
                catch (Exception ex)
                {
                    // Log exception or notify admin if needed
                    Console.WriteLine($"Error deleting file: {ex.Message}");
                }
            }

            genre.Movies.Clear();

            _unitOfWork.Genre.Delete(genre);

            _unitOfWork.Genre.CommitAsync();

            TempData["SuccessMessage"] = "Deleted successfully";

            return RedirectToAction(nameof(Index));
        }
           
        

    }
}
