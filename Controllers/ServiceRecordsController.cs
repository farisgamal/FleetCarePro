using MVCDemo.Context;
using Microsoft.AspNetCore.Mvc;
using MVCDemo.Models;
using MVCDemo.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using MVCDemo.Filters;

namespace MVCDemo.Controllers
{
    [Authorize(Roles = "Admin,FleetManager")]
    public class ServiceRecordsController : Controller
    {
        ApplicationDbContext db = new ApplicationDbContext();
        private readonly IWebHostEnvironment env;

        // Allowed values for the invoice file (Module 3: PDF Invoice Upload)
        private static readonly string[] AllowedExtensions = { ".pdf", ".jpg", ".jpeg", ".png" };
        private const long MaxFileSize = 5 * 1024 * 1024; // 5MB

        public ServiceRecordsController(IWebHostEnvironment environment)
        {
            env = environment;
        }

        public IActionResult Index()
        {
            var records = db.ServiceRecords
                .Include(r => r.Vehicle)
                .Include(r => r.ServiceCenter)
                .ToList();

            return View(records);
        }

        [HttpGet]
        public IActionResult Create()
        {
            LoadDropdowns();
            return View(new ServiceRecordViewModel());
        }

        [HttpPost]
        [AuditLog]
        public IActionResult Create(ServiceRecordViewModel vm)
        {
            // Module 3: Nested Form Submission -> at least one line item is required
            if (vm.LineItems is null || !vm.LineItems.Any())
            {
                ModelState.AddModelError("", "You must add at least one service line item");
            }

            // Module 3: PDF Invoice Upload -> validate type and size
            string? invoicePath = null;
            if (vm.InvoiceDocument is not null)
            {
                var ext = Path.GetExtension(vm.InvoiceDocument.FileName).ToLower();

                if (!AllowedExtensions.Contains(ext))
                    ModelState.AddModelError("InvoiceDocument", "The file must be a PDF, JPG or PNG only");
                else if (vm.InvoiceDocument.Length > MaxFileSize)
                    ModelState.AddModelError("InvoiceDocument", "File size must not exceed 5MB");
            }

            if (!ModelState.IsValid)
            {
                LoadDropdowns();
                return View(vm);
            }

            // Module 3: Atomic Transactions -> the whole operation (Master + Details) runs in one transaction
            using var transaction = db.Database.BeginTransaction();
            try
            {
                if (vm.InvoiceDocument is not null)
                {
                    var uploadsFolder = Path.Combine(env.WebRootPath, "uploads", "invoices");
                    Directory.CreateDirectory(uploadsFolder);

                    var uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(vm.InvoiceDocument.FileName);
                    var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        vm.InvoiceDocument.CopyTo(stream);
                    }

                    invoicePath = "/uploads/invoices/" + uniqueFileName;
                }

                var record = new ServiceRecord
                {
                    VehicleId = vm.VehicleId,
                    ServiceCenterId = vm.ServiceCenterId,
                    ServiceDate = vm.ServiceDate,
                    CurrentMileage = vm.CurrentMileage,
                    Notes = vm.Notes,
                    InvoiceDocumentPath = invoicePath,
                    TotalCost = vm.LineItems.Sum(li => li.Cost),
                    Status = ServiceRecordStatus.Pending,
                    CreatedByUserId = User.Identity?.Name
                };

                db.ServiceRecords.Add(record);
                db.SaveChanges(); // save first so we get record.Id

                foreach (var item in vm.LineItems)
                {
                    db.ServiceLineItems.Add(new ServiceLineItem
                    {
                        ServiceRecordId = record.Id,
                        ServiceCategoryId = item.ServiceCategoryId,
                        Description = item.Description,
                        Cost = item.Cost
                    });
                }

                db.SaveChanges();

                transaction.Commit();
                TempData["Success"] = "Service record and all its line items were saved successfully";
                return RedirectToAction("Index");
            }
            catch
            {
                transaction.Rollback();
                ModelState.AddModelError("", "An error occurred while saving. The whole operation was rolled back");
                LoadDropdowns();
                return View(vm);
            }
        }

        // Module 1: Policy Authorization -> FleetManagers approve/complete/cancel service records
        [AuditLog]
        public IActionResult Approve(int id)
        {
            var record = db.ServiceRecords.Find(id);
            if (record is null) return NotFound();

            record.Status = ServiceRecordStatus.Approved;
            db.SaveChanges();

            TempData["Success"] = "Service record approved";
            return RedirectToAction("Index");
        }

        [AuditLog]
        public IActionResult Complete(int id)
        {
            var record = db.ServiceRecords.Find(id);
            if (record is null) return NotFound();

            record.Status = ServiceRecordStatus.Completed;
            db.SaveChanges();

            TempData["Success"] = "Service record marked as completed";
            return RedirectToAction("Index");
        }

        [AuditLog]
        public IActionResult Cancel(int id)
        {
            var record = db.ServiceRecords.Find(id);
            if (record is null) return NotFound();

            record.Status = ServiceRecordStatus.Cancelled;
            db.SaveChanges();

            TempData["Success"] = "Service record cancelled";
            return RedirectToAction("Index");
        }

        private void LoadDropdowns()
        {
            ViewBag.Vehicles = db.Vehicles.ToList();
            ViewBag.ServiceCenters = db.ServiceCenters.ToList();
            ViewBag.ServiceCategories = db.ServiceCategories.ToList();
        }
    }
}
