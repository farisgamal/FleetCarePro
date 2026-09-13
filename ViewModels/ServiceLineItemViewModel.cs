using System.ComponentModel.DataAnnotations;

namespace MVCDemo.ViewModels
{
    public class ServiceLineItemViewModel
    {
        public int ServiceCategoryId { get; set; }

        [Required]
        public string Description { get; set; } = string.Empty;

        [Range(0, double.MaxValue)]
        public decimal Cost { get; set; }
    }
}
