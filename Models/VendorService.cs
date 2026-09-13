namespace MVCDemo.Models
{
    // Join entity: ServiceCenter <-> ServiceCategory (N-to-N)
    public class VendorService
    {
        public int Id { get; set; }

        public int ServiceCenterId { get; set; }
        public ServiceCenter? ServiceCenter { get; set; }

        public int ServiceCategoryId { get; set; }
        public ServiceCategory? ServiceCategory { get; set; }
    }
}
