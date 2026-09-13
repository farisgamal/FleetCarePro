using System.ComponentModel.DataAnnotations;
using MVCDemo.Validations;

namespace MVCDemo.Models
{
    public class Vehicle
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "VIN is required")]
        [StringLength(17, MinimumLength = 17, ErrorMessage = "VIN must be 17 characters")]
        [ValidVIN]
        public string VIN { get; set; } = string.Empty;

        [Required]
        public string LicensePlate { get; set; } = string.Empty;

        [Required]
        public string Make { get; set; } = string.Empty;

        [Required]
        public string Model { get; set; } = string.Empty;

        [Range(1980, 2100)]
        public int Year { get; set; }

        public decimal PurchasePrice { get; set; }

        public VehicleStatus Status { get; set; } = VehicleStatus.Active;

        [Range(0, int.MaxValue)]
        public int Mileage { get; set; }

        public string? VehicleImageURL { get; set; }

        public List<ServiceRecord>? ServiceRecords { get; set; }

        public string? AssignedDriverId { get; set; }
        public ApplicationUser? AssignedDriver { get; set; }
    }
}
