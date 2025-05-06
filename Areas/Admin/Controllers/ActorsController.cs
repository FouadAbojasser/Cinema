using Cinema.Data;
using Cinema.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Cinema.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ActorsController : Controller
    {
        private readonly ApplicationDbContext _dBcontext = new();
        public IActionResult Index()
        {
            var actors = _dBcontext.Actors;

            return View(actors.ToList());
        }


        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Actor actor, IFormFile Img)
        {
            if (actor != null && Img != null && Img.Length > 0)
            {
                if (!ModelState.IsValid)
                {
                    return View(actor);
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
                actor.Img = fileName;
                _dBcontext.Actors.Add(actor);
                _dBcontext.SaveChanges();

                return RedirectToAction(nameof(Index));
            }

            return View(actor);
        }



        public IActionResult Edit(int id)
        {
            var actor = _dBcontext.Actors.Find(id);
            if (actor != null)
            {
                return View(actor);
            }
            return View("NotFoundPage");
        }


        [HttpPost]
        public IActionResult Edit(Actor actor, IFormFile Img)
        {
            var oldActorInDB = _dBcontext.Actors.AsNoTracking().FirstOrDefault(e => e.Id == actor.Id);
            ModelState.Remove("Img");
            if (ModelState.IsValid && oldActorInDB != null)
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
                    if (!string.IsNullOrEmpty(oldActorInDB.Img))
                    {
                        string oldFilePath = Path.Combine(folderPath, oldActorInDB.Img);
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

                    actor.Img = newFileName; // Update to new image
                }
                else
                {
                    // No new image uploaded, retain old image
                    actor.Img = oldActorInDB.Img;
                }

                _dBcontext.Actors.Update(actor);
                _dBcontext.SaveChanges();

                return RedirectToAction(nameof(Index));
            }

            return View(actor); // Return to edit form if model is invalid
        }



        public IActionResult Delete(int id)
        {
            var actor = _dBcontext.Actors.Find(id);

            if (actor != null)
            {
                if (!string.IsNullOrEmpty(actor.Img))
                {
                    string folderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "img", "cast", "avatar");

                    string oldFilePath = Path.Combine(folderPath, actor.Img);
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

                _dBcontext.Actors.Remove(actor);

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
