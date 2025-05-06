using Cinema.Data;
using Cinema.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Cinema.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class DirectorsController : Controller
    {
        private readonly ApplicationDbContext _dBcontext = new();
        public IActionResult Index()
        {
            var directors = _dBcontext.Directors;

            return View(directors.ToList());
        }


        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Director director, IFormFile Img)
        {
            if (director != null && Img != null && Img.Length > 0)
            {
                if (!ModelState.IsValid)
                {
                    return View(director);
                }

                // Generate unique filename
                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(Img.FileName);

                // Define the folder path
                var folderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\img\\cast\\avatar");

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
                director.Img = fileName;
                _dBcontext.Directors.Add(director);
                _dBcontext.SaveChanges();

                return RedirectToAction(nameof(Index));
            }

            return View(director);
        }



        public IActionResult Edit(int id)
        {
            var director = _dBcontext.Directors.Find(id);
            if (director != null)
            {
                return View(director);
            }
            return View("NotFoundPage");
        }


        [HttpPost]
        public IActionResult Edit(Director director, IFormFile Img)
        {
            var oldDirectorInDB = _dBcontext.Directors.AsNoTracking().FirstOrDefault(e => e.Id == director.Id);
            ModelState.Remove("Img");
            if (ModelState.IsValid && oldDirectorInDB != null)
            {
                string folderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "img", "cast", "avatar");

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
                    if (!string.IsNullOrEmpty(oldDirectorInDB.Img))
                    {
                        string oldFilePath = Path.Combine(folderPath, oldDirectorInDB.Img);
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

                    director.Img = newFileName; // Update to new image
                }
                else
                {
                    // No new image uploaded, retain old image
                    director.Img = oldDirectorInDB.Img;
                }

                _dBcontext.Directors.Update(director);
                _dBcontext.SaveChanges();

                return RedirectToAction(nameof(Index));
            }

            return View(director); // Return to edit form if model is invalid
        }



        public IActionResult Delete(int id)
        {
            var director = _dBcontext.Directors.Find(id);

            if (director != null)
            {
                if (!string.IsNullOrEmpty(director.Img))
                {
                    string folderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "img", "cast", "avatar");

                    string oldFilePath = Path.Combine(folderPath, director.Img);
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

                _dBcontext.Directors.Remove(director);

                _dBcontext.SaveChanges();

            }
            else
            {
                return View("NotFoundPage");
            }

            // Delete old file if it exists
            return RedirectToAction(nameof(Index));
        }
    }
}
