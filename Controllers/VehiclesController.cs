using MVCDemo.Context;
using Microsoft.AspNetCore.Mvc;
using MVCDemo.Models;
using Microsoft.AspNetCore.Authorization;
using MVCDemo.Filters;
using Microsoft.EntityFrameworkCore;

namespace MVCDemo.Controllers
{
    [Authorize(Roles = "Admin,FleetManager,Driver")]
    public class VehiclesController : Controller
    {
        ApplicationDbContext db = new ApplicationDbContext();
        private readonly IWebHostEnvironment env;

        public VehiclesController(IWebHostEnvironment environment)
        {
            env = environment;
        }

        public IActionResult Index()
        {
            var vehicles = db.Vehicles.ToList();
            return View(vehicles);
        }

        public IActionResult Details(int id)
        {
            var vehicle = db.Vehicles
                .Include(v => v.ServiceRecords)
                .FirstOrDefault(v => v.Id == id);

            if (vehicle is null) return NotFound();
            return View(vehicle);
        }

        [Authorize(Roles = "Admin,FleetManager")]
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [Authorize(Roles = "Admin,FleetManager")]
        [HttpPost]
        [AuditLog]
        public IActionResult Create(Vehicle v, IFormFile? VehicleImage)
        {
            if (ModelState.IsValid)
            {
                // Module 2: File Processing -> upload the vehicle image into wwwroot/uploads/vehicles with a unique GUID name
                if (VehicleImage is not null && VehicleImage.Length > 0)
                {
                    var uploadsFolder = Path.Combine(env.WebRootPath, "uploads", "vehicles");
                    Directory.CreateDirectory(uploadsFolder);

                    var uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(VehicleImage.FileName);
                    var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        VehicleImage.CopyTo(stream);
                    }

                    v.VehicleImageURL = "/uploads/vehicles/" + uniqueFileName;
                }

                db.Vehicles.Add(v);
                db.SaveChanges();

                // Module 2: Feedback UX -> TempData notification
                TempData["Success"] = "Vehicle added successfully";
                return RedirectToAction("Index");
            }

            return View(v);
        }

        [Authorize(Roles = "Admin,FleetManager")]
        [HttpGet]
        public IActionResult Update(int id)
        {
            var vehicle = db.Vehicles.Find(id);
            if (vehicle is null) return NotFound();
            return View(vehicle);
        }

        [Authorize(Roles = "Admin,FleetManager")]
        [HttpPost]
        [AuditLog]
        public IActionResult Update(Vehicle v, int id, IFormFile? VehicleImage)
        {
            v.Id = id;

            if (ModelState.IsValid)
            {
                if (VehicleImage is not null && VehicleImage.Length > 0)
                {
                    var uploadsFolder = Path.Combine(env.WebRootPath, "uploads", "vehicles");
                    Directory.CreateDirectory(uploadsFolder);

                    var uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(VehicleImage.FileName);
                    var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        VehicleImage.CopyTo(stream);
                    }

                    v.VehicleImageURL = "/uploads/vehicles/" + uniqueFileName;
                }

                db.Vehicles.Update(v);
                db.SaveChanges();

                TempData["Success"] = "Vehicle updated successfully";
                return RedirectToAction("Index");
            }

            return View(v);
        }

        [Authorize(Roles = "Admin,FleetManager")]
        [AuditLog]
        public IActionResult Delete(int id)
        {
            var vehicle = db.Vehicles.Find(id);
            if (vehicle is null) return NotFound();

            db.Vehicles.Remove(vehicle);
            db.SaveChanges();

            TempData["Success"] = "Vehicle deleted";
            return RedirectToAction("Index");
        }
    }
}
