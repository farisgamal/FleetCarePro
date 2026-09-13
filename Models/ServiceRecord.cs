using System.ComponentModel.DataAnnotations;

namespace MVCDemo.Models
{
    public class ServiceRecord
    {
        public int Id { get; set; }

        public int VehicleId { get; set; }
        public Vehicle? Vehicle { get; set; }

        public int ServiceCenterId { get; set; }
        public ServiceCenter? ServiceCenter { get; set; }

        [DataType(DataType.Date)]
        public DateTime ServiceDate { get; set; } = DateTime.Now;

        public int CurrentMileage { get; set; }

        public decimal TotalCost { get; set; }

        public string? InvoiceDocumentPath { get; set; }

        public string? Notes { get; set; }

        public ServiceRecordStatus Status { get; set; } = ServiceRecordStatus.Pending;

        public string? CreatedByUserId { get; set; }

        public List<ServiceLineItem>? ServiceLineItems { get; set; }
    }
}
