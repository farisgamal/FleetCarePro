using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace MVCDemo.ViewModels
{
    // Module 3: Nested Form Submission -> Master (ServiceRecord) + Detail list (ServiceLineItems)
    public class ServiceRecordViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Please select a vehicle")]
        public int VehicleId { get; set; }

        [Required(ErrorMessage = "Please select a service center")]
        public int ServiceCenterId { get; set; }

        [DataType(DataType.Date)]
        public DateTime ServiceDate { get; set; } = DateTime.Now;

        public int CurrentMileage { get; set; }

        public string? Notes { get; set; }

        // PDF Invoice Upload: Max 5MB, .pdf/.jpg/.png only
        public IFormFile? InvoiceDocument { get; set; }

        // Dynamic line items added on the client with JavaScript
        public List<ServiceLineItemViewModel> LineItems { get; set; } = new();

        // Calculated, not posted from the form
        public decimal TotalCost => LineItems?.Sum(li => li.Cost) ?? 0;
    }
}
