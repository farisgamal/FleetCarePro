using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using MVCDemo.Context;

namespace MVCDemo.Controllers
{
    public class HomeController : Controller
    {
        ApplicationDbContext db = new ApplicationDbContext();

        [Authorize]
        public IActionResult Index()
        {
            var vehicles = db.Vehicles.ToList();
            return View(vehicles);
        }

        // Fleet Analytics & Reports -> Total Cost Per Vehicle + Most Used Service Center
        [Authorize(Roles = "Admin,FleetManager")]
        public IActionResult Analytics()
        {
            var totalCostPerVehicle = db.ServiceRecords
                .GroupBy(sr => sr.Vehicle!.LicensePlate)
                .Select(g => new
                {
                    LicensePlate = g.Key,
                    TotalCost = g.Sum(sr => sr.TotalCost)
                })
                .ToList();

            var mostUsedCenter = db.ServiceRecords
                .GroupBy(sr => sr.ServiceCenter!.Name)
                .Select(g => new
                {
                    ServiceCenterName = g.Key,
                    Visits = g.Count()
                })
                .OrderByDescending(g => g.Visits)
                .FirstOrDefault();

            ViewBag.TotalCostPerVehicle = totalCostPerVehicle;
            ViewBag.MostUsedCenterName = mostUsedCenter?.ServiceCenterName ?? "N/A";
            ViewBag.MostUsedCenterVisits = mostUsedCenter?.Visits ?? 0;

            return View();
        }

        [AllowAnonymous]
        public IActionResult Maintenance() => View();

        [AllowAnonymous]
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error(int? statusCode = null)
        {
            ViewBag.StatusCode = statusCode;
            return View();
        }
    }
}
