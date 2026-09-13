using MVCDemo.Context;
using Microsoft.AspNetCore.Mvc;
using MVCDemo.Models;
using Microsoft.AspNetCore.Authorization;

namespace MVCDemo.Controllers
{
    [Authorize(Roles = "Admin,FleetManager")]
    public class ServiceCategoriesController : Controller
    {
        ApplicationDbContext db = new ApplicationDbContext();

        public IActionResult Index()
        {
            return View(db.ServiceCategories.ToList());
        }

        [HttpGet]
        public IActionResult Create() => View();

        [HttpPost]
        public IActionResult Create(ServiceCategory sc)
        {
            if (ModelState.IsValid)
            {
                db.ServiceCategories.Add(sc);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(sc);
        }

        public IActionResult Delete(int id)
        {
            var sc = db.ServiceCategories.Find(id);
            if (sc is null) return NotFound();
            db.ServiceCategories.Remove(sc);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
