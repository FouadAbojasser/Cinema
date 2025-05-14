using System.Linq;
using Cinema.Data;
using Cinema.Models;
using Cinema.Models.ViewModels;
using Cinema.Repositories.IRepositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing.Constraints;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using NuGet.Common;

namespace Cinema.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class MoviesController : Controller
    {
        //private readonly ApplicationDbContext _dbcontext = new ();

        private readonly IActorRepository _actorRepository;
        private readonly IDirectorRepository _directorRepository;
        private readonly IGenreRepository _genreRepository;
        private readonly IMovieRepository _movieRepository;

        public MoviesController(IActorRepository actorRepository, IDirectorRepository directorRepository, IGenreRepository genreRepository, IMovieRepository movieRepository)
        {
            _actorRepository = actorRepository;
            _directorRepository = directorRepository;
            _genreRepository = genreRepository;
            _movieRepository = movieRepository;
        }

        public IActionResult Index()
        {
            var movies = _movieRepository.Get(
                null,
                [e => e.Genres, e => e.Director],
                false
                );
                
            return View(movies);
        }


        public IActionResult Create()
        {
            var genres = _genreRepository.Get();
            var actors = _actorRepository.Get();
            var directors = _directorRepository.Get();

            var MovieWithNeededData = new MovieGenresActorsDirectorsVM()
            {

                Genres = genres.ToList(),
                Actors = actors.ToList(),
                Directors = directors.ToList(),
            };
           
            return View(MovieWithNeededData);

        }


        [HttpPost]
        public async Task<IActionResult> CreateAsync(Movie movie, IFormFile PosterImage, List<IFormFile> SliderImages, List<int> SelectedGenreIds, List<int> SelectedActorIds, int SelectedDirectorId)
        {
            //movie.Genres = _dbcontext.Genres.Where(g => SelectedGenreIds.Contains(g.Id)).ToList();

            movie.Genres = _genreRepository.Get(
                g => SelectedGenreIds.Contains(g.Id)
                , null, false).ToList();

            //movie.Actors = _dbcontext.Actors.Where(a => SelectedActorIds.Contains(a.Id)).ToList();

            movie.Actors = _actorRepository.Get(
                a => SelectedActorIds.Contains(a.Id)
                , null, false).ToList();

            movie.DirectorId = SelectedDirectorId;

            await _movieRepository.CreateAsync(movie);

            await _movieRepository.CommitAsync();

            // Handling Single Image - Poster
            if (PosterImage != null && PosterImage.Length > 0)
            {
                // Generate New File name and Add file type extension on it
                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(PosterImage.FileName);

                // Define the folder path
                var folderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\img\\posters");

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
                    await PosterImage.CopyToAsync(stream);
                }

                // Save the image name to the Movie.Images List
                movie.Images.Add(new Image
                {
                    Name = fileName,
                    Type = "poster",
                    MovieId = movie.Id,
                    SeriesId = null,
                });

            }


            // Handling Multiple Images - Sliders
            foreach (var image in SliderImages)
            {
                if (image != null && image.Length > 0)
                {
                    var fileName = Guid.NewGuid().ToString() + Path.GetExtension(image.FileName);

                    var folderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\img\\sliders");

                    if (!Directory.Exists(folderPath))
                    {
                        Directory.CreateDirectory(folderPath);
                    }

                    var filePath = Path.Combine(folderPath, fileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await image.CopyToAsync(stream);
                    }

                    // Save the image name to the Movie.Images List
                    movie.Images.Add(new Image { 
                        Name = fileName, 
                        Type = "slider", 
                        MovieId = movie.Id, 
                        SeriesId = null,
                    });

                }
               
            }

            await _movieRepository.CommitAsync();

            TempData["SuccessMessage"] = "Created successfully";

            return RedirectToAction(nameof(Index));
        }



        public Task<IActionResult> Edit(int id)
        {
            //var movie = _dbcontext.Movies
            //    .Include(g=>g.Genres)
            //    .Include(a=>a.Actors)
            //    .Include(d=>d.Director)
            //    .Include(m=>m.Images)
            //    .FirstOrDefault(m=>m.Id == id);

            var movie = _movieRepository.GetOne(
                m => m.Id == id,
                [g => g.Genres, a => a.Actors, d => d.Director, m => m.Images]);


            if (movie == null)
            {
                return Task.FromResult<IActionResult>(RedirectToAction("NotFoundPage", "Home"));
            }

            var genres = _genreRepository.Get();
            var actors =_actorRepository.Get();
            var directors = _directorRepository.Get();

            var MovieWithNeededData = new MovieGenresActorsDirectorsVM()
            {
                movie = movie,
                Genres = genres.ToList(),
                Actors = actors.ToList(),
                Directors = directors.ToList(),
            };


            return Task.FromResult<IActionResult>(View(MovieWithNeededData));
        }


        [HttpPost]
        public async Task<IActionResult> EditAsync(Movie movie,
                        List<int> SelectedGenreIds,
                        List<int> SelectedActorIds,
                        int SelectedDirectorId,
                        string RemovedPosterUrl,
                        IFormFile PosterImage,
                        List<string> RemovedSlidersUrls,
                        List<IFormFile> SliderImages)
        {
            // Remove AsNoTracking() - we need change tracking
            //var existingMovie = _dbcontext.Movies
            //    .Include(m => m.Genres)
            //    .Include(m => m.Actors)
            //    .Include(m => m.Director)
            //    .Include(i => i.Images)
            //    .FirstOrDefault(m => m.Id == movie.Id);

            var existingMovie = _movieRepository.GetOne(
                m => m.Id == movie.Id,
                [m => m.Genres, m => m.Actors, m => m.Director, i => i.Images],
                false
                );

            if (existingMovie == null)
            {
                return RedirectToAction("NotFoundPage", "Home");
            }

            // Update simple properties
            //_dbcontext.Entry(existingMovie).CurrentValues.SetValues(movie);

            //=> Method 1 Execlude some proparites from Update
            //var entry = _dbcontext.Entry(existingMovie);
            //entry.CurrentValues.SetValues(movie);

            //// Exclude Rate (or any other property)
            //entry.Property(m => m.Rate).IsModified = false;

            //=> Method 2 Update only values from the Edit Form
            existingMovie.Title = movie.Title;
            existingMovie.Overview = movie.Overview;
            existingMovie.Trailer = movie.Trailer;
            existingMovie.ProductionDate = movie.ProductionDate;
            existingMovie.Country = movie.Country;
            existingMovie.ReleaseDate = movie.ReleaseDate;
            existingMovie.Duration = movie.Duration;
            existingMovie.Language = movie.Language;
            existingMovie.AgeRating = movie.AgeRating;



            // Update genres - use proper relationship management
            UpdateGenres(existingMovie, SelectedGenreIds);

            // Update actors - use proper relationship management
            UpdateActors(existingMovie, SelectedActorIds);

            // Update director
            existingMovie.DirectorId = SelectedDirectorId;

            // Handling Posters
            HandlingUpdatePosters(existingMovie, RemovedPosterUrl, PosterImage);


            // Handling Sliders
            await HandlingUpdateSlidersAsync(existingMovie, RemovedSlidersUrls, SliderImages);

            await _movieRepository.CommitAsync();

            TempData["SuccessMessage"] = "Edited successfully";

            return RedirectToAction(nameof(Index));
        }

       

        public async Task<IActionResult> DeleteAsync(int id)
        {
            //var movie = _dbcontext.Movies
            //.Include(m => m.Actors)
            //.FirstOrDefault(m => m.Id == id);

            var movie = _movieRepository.GetOne(
                m => m.Id == id,
                [m => m.Actors],
                false
                );


            if (movie == null)
            {
                return RedirectToAction("NotFoundPage", "Home");
            }

            //Remove Actors from Joint Entity
            movie.Actors.Clear(); // Removes entries from join table


            foreach (var image in movie.Images)
            {
                string folderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "img", "sliders");
                string oldFilePath = Path.Combine(folderPath, image.Name);
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

            _movieRepository.Delete(movie);

            await _movieRepository.CommitAsync();

            TempData["SuccessMessage"] = "Deleted successfully";

            return RedirectToAction(nameof(Index));

        }





        
        private void UpdateGenres(Movie movie, List<int> selectedGenreIds)
        {
            if (selectedGenreIds == null)
            {
                movie.Genres.Clear();
                return;
            }

            var selectedGenres = new HashSet<int>(selectedGenreIds);  //New Genres
            var currentGenres = new HashSet<int>(movie.Genres.Select(g => g.Id)); //Old Genres

            // Remove genres that are no longer selected
            foreach (var genre in movie.Genres.ToList())
            {
                if (!selectedGenres.Contains(genre.Id))
                {
                    movie.Genres.Remove(genre);
                }
            }

            // Add new genres that weren't previously selected
            foreach (var genreId in selectedGenres)
            {
                if (!currentGenres.Contains(genreId))
                {
                    var genre = _genreRepository.GetOne(
                        e=>e.Id == genreId
                        );

                    if (genre != null)
                    {
                        movie.Genres.Add(genre);
                    }
                }
            }
        }

        
        private void UpdateActors(Movie movie, List<int> selectedActorIds)
        {
            if (selectedActorIds == null)
            {
                movie.Actors.Clear();
                return;
            }

            var selectedActors = new HashSet<int>(selectedActorIds);
            var currentActors = new HashSet<int>(movie.Actors.Select(a => a.Id));

            // Remove actors that are no longer selected
            foreach (var actor in movie.Actors.ToList())
            {
                if (!selectedActors.Contains(actor.Id))
                {
                    movie.Actors.Remove(actor);
                }
            }

            // Add new actors that weren't previously selected
            foreach (var actorId in selectedActors)
            {
                if (!currentActors.Contains(actorId))
                {
                    var actor = _actorRepository.GetOne(
                        a => a.Id == actorId);

                    if (actor != null)
                    {
                        movie.Actors.Add(actor);
                    }
                }
            }
        }


        private void HandlingUpdatePosters(Movie movie, string removedPosterUrl, IFormFile posterImage)
        {

            // First handle removed images
            if (!removedPosterUrl.IsNullOrEmpty())
            {
               
                string fileName = Path.GetFileName(removedPosterUrl);
                var imageToRemove = movie.Images.FirstOrDefault(i => i.Name == fileName);

                if (imageToRemove != null)
                {
                    // Remove from database
                    movie.Images.Remove(imageToRemove);

                    // Delete physical file
                    string folderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "img", "posters");
                    string filePath = Path.Combine(folderPath, fileName);

                    try
                    {
                        if (System.IO.File.Exists(filePath))
                        {
                            System.IO.File.Delete(filePath);
                        }
                    }
                    catch (Exception ex)
                    {
                        // Log exception or notify admin
                        Console.WriteLine($"Error deleting file: {ex.Message}");
                    }
                }
               
            }

            // Then handle new image uploads
            if (posterImage != null && posterImage.Length > 0)
            {
                // Generate unique filename
                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(posterImage.FileName);
                var folderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "img", "posters");

                // Create folder if it doesn't exist
                if (!Directory.Exists(folderPath))
                {
                    Directory.CreateDirectory(folderPath);
                }

                // Save the file
                var filePath = Path.Combine(folderPath, fileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    posterImage.CopyTo(stream);
                }

                // Add to movie's images collection
                movie.Images.Add(new Image
                {
                    Name = fileName,
                    Type = "poster",
                    MovieId = movie.Id,
                    SeriesId = null
                });
            }
        }


        private async Task HandlingUpdateSlidersAsync(Movie movie, List<string> RemovedSlidersUrls, List<IFormFile> SliderImages)
        {
            // Handle removed images
            if (RemovedSlidersUrls != null && RemovedSlidersUrls.Count > 0)
            {
                foreach (var imageUrl in RemovedSlidersUrls)
                {
                    string fileName = Path.GetFileName(imageUrl);
                    var imageToRemove = movie.Images.FirstOrDefault(i => i.Name == fileName);

                    if (imageToRemove != null)
                    {
                        movie.Images.Remove(imageToRemove);

                        string folderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "img", "sliders");
                        string filePath = Path.Combine(folderPath, fileName);

                        try
                        {
                            if (System.IO.File.Exists(filePath))
                            {
                                System.IO.File.Delete(filePath);
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Error deleting file: {ex.Message}");
                        }
                    }
                }
            }

            // Handle new images
            if (SliderImages != null && SliderImages.Count > 0)
            {
                foreach (var image in SliderImages)
                {
                    if (image != null && image.Length > 0)
                    {
                        var fileName = Guid.NewGuid().ToString() + Path.GetExtension(image.FileName);
                        var folderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "img", "sliders");

                        if (!Directory.Exists(folderPath))
                        {
                            Directory.CreateDirectory(folderPath);
                        }

                        var filePath = Path.Combine(folderPath, fileName);
                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await image.CopyToAsync(stream);
                        }

                        movie.Images.Add(new Image
                        {
                            Name = fileName,
                            Type = "slider",
                            MovieId = movie.Id,
                            SeriesId = null
                        });
                    }
                }
            }
        }



    }
}
