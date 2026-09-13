using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MVCDemo.Context;
using MVCDemo.Models;

namespace MVCDemo.ViewComponents
{
    // Module 4: View Component 1 -> vehicles not serviced in the last 6 months
    public class OverdueMaintenanceViewComponent : ViewComponent
    {
        private readonly ApplicationDbContext db;

        public OverdueMaintenanceViewComponent(ApplicationDbContext context)
        {
            db = context;
        }

        public IViewComponentResult Invoke()
        {
            var sixMonthsAgo = DateTime.Now.AddMonths(-6);

            var overdueVehicles = db.Vehicles
                .Include(v => v.ServiceRecords)
                .Where(v => v.Status != VehicleStatus.Decommissioned &&
                            (v.ServiceRecords == null || !v.ServiceRecords.Any() ||
                             v.ServiceRecords.Max(sr => sr.ServiceDate) < sixMonthsAgo))
                .ToList();

            return View(overdueVehicles);
        }
    }
}
