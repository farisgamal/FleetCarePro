using Microsoft.AspNetCore.Mvc;
using MVCDemo.Context;

namespace MVCDemo.ViewComponents
{
    // Module 4: View Component 2 -> total maintenance spend for the current month
    public class FleetCostSummaryViewComponent : ViewComponent
    {
        private readonly ApplicationDbContext db;

        public FleetCostSummaryViewComponent(ApplicationDbContext context)
        {
            db = context;
        }

        public IViewComponentResult Invoke()
        {
            var now = DateTime.Now;

            decimal totalThisMonth = db.ServiceRecords
                .Where(sr => sr.ServiceDate.Month == now.Month && sr.ServiceDate.Year == now.Year)
                .Sum(sr => (decimal?)sr.TotalCost) ?? 0;

            return View(totalThisMonth);
        }
    }
}
