using System.ComponentModel.DataAnnotations;

namespace MVCDemo.Models
{
    public class ServiceCategory
    {
        public int Id { get; set; }

        [Required]
        public string CategoryName { get; set; } = string.Empty;

        public string? Description { get; set; }

        public int RecommendedIntervalMonths { get; set; }

        public List<VendorService>? VendorServices { get; set; }

        public List<ServiceLineItem>? ServiceLineItems { get; set; }
    }
}
