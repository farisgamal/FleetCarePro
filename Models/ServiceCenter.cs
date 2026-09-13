using System.ComponentModel.DataAnnotations;

namespace MVCDemo.Models
{
    public class ServiceCenter
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; } = string.Empty;

        [Phone]
        public string? PhoneNumber { get; set; }

        [EmailAddress]
        public string? Email { get; set; }

        public string? Address { get; set; }

        public bool IsActive { get; set; } = true;

        public List<ServiceRecord>? ServiceRecords { get; set; }

        public List<VendorService>? VendorServices { get; set; }
    }
}
