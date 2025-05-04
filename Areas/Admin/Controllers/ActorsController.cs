using Cinema.Data;
using Cinema.Models;
using Microsoft.AspNetCore.Mvc;

namespace Cinema.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ActorsController : Controller
    {
        private readonly ApplicationDbContext _dBcontext = new ApplicationDbContext();
        public IActionResult Index()
        {
            var Actors = _dBcontext.Actors;

            return View(Actors.ToList());
        }


        public IActionResult Create()
        {
            
            return View();
        }

        [HttpPut]
        public IActionResult Create (Actor actor)
        {
            if(actor != null) 
            _dBcontext.Actors.Add(actor);
            _dBcontext.SaveChanges();

            return View();
        }



        public IActionResult Delete(int id)
        {
            var actor = _dBcontext.Actors.Find(id);
            if (actor != null)
            {
                _dBcontext.Actors.Remove(actor);
                _dBcontext.SaveChanges();
            }

            return RedirectToAction(nameof(Index));
        }


    }
}
