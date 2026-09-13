namespace MVCDemo.Models
{
    public class ServiceLineItem
    {
        public int Id { get; set; }

        public int ServiceRecordId { get; set; }
        public ServiceRecord? ServiceRecord { get; set; }

        public int ServiceCategoryId { get; set; }
        public ServiceCategory? ServiceCategory { get; set; }

        public string? Description { get; set; }

        public decimal Cost { get; set; }
    }
}
