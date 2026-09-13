using MVCDemo.Context;
using Microsoft.AspNetCore.Mvc;
using MVCDemo.Models;
using Microsoft.AspNetCore.Authorization;
using MVCDemo.Filters;

namespace MVCDemo.Controllers
{
    [Authorize(Roles = "Admin,FleetManager")]
    public class ServiceCentersController : Controller
    {
        ApplicationDbContext db = new ApplicationDbContext();

        public IActionResult Index()
        {
            var centers = db.ServiceCenters.ToList();
            return View(centers);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [AuditLog]
        public IActionResult Create(ServiceCenter sc)
        {
            if (ModelState.IsValid)
            {
                db.ServiceCenters.Add(sc);
                db.SaveChanges();
                TempData["Success"] = "Service center added successfully";
                return RedirectToAction("Index");
            }
            return View(sc);
        }

        [HttpGet]
        public IActionResult Update(int id)
        {
            var sc = db.ServiceCenters.Find(id);
            if (sc is null) return NotFound();
            return View(sc);
        }

        [HttpPost]
        [AuditLog]
        public IActionResult Update(ServiceCenter sc, int id)
        {
            sc.Id = id;
            if (ModelState.IsValid)
            {
                db.ServiceCenters.Update(sc);
                db.SaveChanges();
                TempData["Success"] = "Service center updated successfully";
                return RedirectToAction("Index");
            }
            return View(sc);
        }

        [AuditLog]
        public IActionResult Delete(int id)
        {
            var sc = db.ServiceCenters.Find(id);
            if (sc is null) return NotFound();

            db.ServiceCenters.Remove(sc);
            db.SaveChanges();
            TempData["Success"] = "Service center deleted";
            return RedirectToAction("Index");
        }
    }
}
