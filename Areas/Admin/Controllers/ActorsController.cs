using Cinema.Data;
using Cinema.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Cinema.Repositories;
using System.Threading.Tasks;
using Cinema.Repositories.IRepositories;

namespace Cinema.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ActorsController : Controller
    {
        //private readonly ApplicationDbContext _dBcontext = new();
        private readonly IActorRepository _repository; //= new ActorRepository();
       
        public ActorsController(IActorRepository repository)
        {
            _repository = repository;
           
        }

        public IActionResult Index()
        {
            var actors = _repository.Get(null, [m => m.Movies]);
           
            foreach (var actor in actors)
            {
             actor.NoOfMovies = actor.Movies.Count();
            }

            return View(actors);
        }



        public IActionResult Create()
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> CreateAsync(Actor actor, IFormFile Img)
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
                    await Img.CopyToAsync(stream);
                }

                // Save the image name to the database
                actor.Img = fileName;

                await _repository.CreateAsync(actor);

                await _repository.CommitAsync();
               
                TempData["SuccessMessage"] = "Created successfully";

                return RedirectToAction(nameof(Index));
            }

            return View(actor);
        }


        public IActionResult Edit(int id)
        {
            var actor = _repository.GetOne(e=>e.Id==id);
            if (actor != null)
            {
                return View(actor);
            }
            return RedirectToAction("NotFoundPage", "Home");
        }


        [HttpPost]
        public async Task<IActionResult> EditAsync(Actor actor, IFormFile Img)
        {
            var oldActorInDB = _repository.GetOne(e => e.Id == actor.Id,null);
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
                        await Img.CopyToAsync(stream);
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

                _repository.Update(actor);

                await _repository.CommitAsync();

                TempData["SuccessMessage"] = "Edited successfully";

                return RedirectToAction(nameof(Index));
            }

            return View(actor); // Return to edit form if model is invalid
        }


        public async Task<IActionResult> Delete(int id)
        {
            
            var actor = _repository.GetOne(a => a.Id == id, [a => a.Movies]);

            if (actor == null)
            {
                return RedirectToAction("NotFoundPage", "Home");
            }

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

            actor.Movies.Clear();

            _repository.Delete(actor);

            await _repository.CommitAsync();

            TempData["SuccessMessage"] = "Deleted successfully";

            return RedirectToAction(nameof(Index));

            }
            


    }
}
